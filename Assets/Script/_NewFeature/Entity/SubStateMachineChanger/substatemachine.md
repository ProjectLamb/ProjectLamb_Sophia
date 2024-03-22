I was looking for a way to do this in unity but after coming here it looks like its unsupported so I wrote my own StateMachineBehaviour script that would handle this.

Spoiler: AnySubstateTranistionController_SMB
```cs
Code (CSharp):
using System.Collections.Generic;
using UnityEngine;
using System;
 
#if UNITY_EDITOR
using System.Reflection;
using System.Linq;
using UnityEditor.Animations;
using AnimatorController = UnityEditor.Animations.AnimatorController;
#endif
 
    public class AnySubstateTranistionController_SMB : StateMachineBehaviour
    {
 
 
        [SerializeField][HideInInspector] private TransitionInfo[] bakedTransitions;
        bool inTransition;
 
#if UNITY_EDITOR
        [SerializeField] private List<TransitionConfig> config = new List<TransitionConfig>();
 
        public void OnValidate() => BakeTransitions();
 
        //for some reason the needed function to grab the direct parent is internal so need to use reflection to find it
        private static AnimatorStateMachine FindParentStateMacihine(AnimatorStateMachine root,AnimatorStateMachine child)
        {
            var type = typeof(AnimatorStateMachine);
            var method_FindParent = type.GetMethod("FindParent", BindingFlags.Instance | BindingFlags.NonPublic);
            var parent = (AnimatorStateMachine)method_FindParent.Invoke(root,new object[] { child});
            return parent;
        }
 
        public void BakeTransitions()
        {
 
            var map = config.ToDictionary(c => c.editorReference, c => c.transition);
            var newMap = new Dictionary<AnimatorTransition, TransitionInfo>();
 
            var contexts = AnimatorController.FindStateMachineBehaviourContext(this);
            foreach (var c in contexts)
            {
                if (!(c.animatorObject is AnimatorStateMachine asm))
                {
                    continue;
                }
                var rootMachine = c.animatorController.layers[c.layerIndex].stateMachine;
                var parent = FindParentStateMacihine(rootMachine, asm);
 
                var allExitTransitions = parent.GetStateMachineTransitions(asm);
 
                foreach(var t in allExitTransitions)
                {
                    if (!map.TryGetValue(t, out var oldInfo))
                    {
                        oldInfo = TransitionInfo.Create(t, c.animatorController, c.layerIndex);
                    }
                    else
                    {
                        oldInfo = TransitionInfo.Create(t, c.animatorController, c.layerIndex,oldInfo.crossfadeDuration);
                    }
 
                    if(oldInfo.conditons.Length >0)
                    {
                        oldInfo.name = t.GetDisplayName(asm);
                        newMap[t] = oldInfo;
                    }
                }
            }
 
            config = newMap.Select(kvp=> new TransitionConfig() {name=kvp.Value.name, editorReference = kvp.Key, transition = kvp.Value})
                .ToList();
 
            bakedTransitions = newMap.Values.ToArray();
 
        }
    #endif
 
        #region StateMachineBehaviour
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (inTransition)
            {
                inTransition = false;
            }
        }
 
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (inTransition)
            {
                return;
            }
 
            for (int i = 0; i < bakedTransitions.Length; i++)
            {
                var info = bakedTransitions[i];
 
                if (info.TryTransition(animator, stateInfo, false, layerIndex))
                {
                    inTransition = true;
 
                    return;
                }
 
            }
        }
        #endregion
 
        #region typedefs
#if UNITY_EDITOR
        [Serializable]
        private class TransitionConfig
        {
            [HideInInspector] public string name;// hiding field will hide it on the element ,but will appear as the summary title when TransitionConfig is in an array in the inspector
            [HideInInspector]public AnimatorTransition editorReference;
            public TransitionInfo transition;
        }
#endif
        [Serializable]
        private struct TransitionInfo
        {
            [HideInInspector]public string name;
            [HideInInspector]public int destinationStateHash;
            public float crossfadeDuration;
            public TransitionCondition[] conditons;
 
    #if UNITY_EDITOR
            public static TransitionInfo Create(AnimatorTransition data,AnimatorController controller,int layer, float crossfadeDuration = 0.25f)
            {
                var targetState = data.destinationState?? data.destinationStateMachine?.defaultState ?? controller.layers[layer].stateMachine.defaultState;
 
                var conditions = data.conditions.Select(c=> TransitionCondition.Create(c, controller));
 
                return new TransitionInfo()
                {
                    destinationStateHash = targetState.nameHash,
                    crossfadeDuration = crossfadeDuration,
                    conditons = conditions.ToArray()
                };
            }
    #endif
 
            public bool TryTransition(Animator animator, AnimatorStateInfo stateInfo,bool isExit, int layer)
            {
                if (stateInfo.fullPathHash == destinationStateHash) return false;
 
                for(int i=0;i< conditons.Length;i++)
                {
 
                    if (!conditons[i].Evaluate(animator)) return false;
 
 
                }
 
                if(conditons.Length < 1 && !isExit)
                    return false;
     
 
                for(int i=0;i<conditons.Length;i++)
                    conditons[i].Consume(animator);
 
                animator.CrossFadeInFixedTime(destinationStateHash, crossfadeDuration, layer);
                return true;
            }
        }
 
        [Serializable]
        private struct TransitionCondition
        {
            /// <summary>
            /// the type of condition to eval, this is a mirroe of UnityEditor.Animations.AnimatorConditionMode
            /// </summary>
            public enum Condition
            {
                If = 1,
                IfNot = 2,
                Greater = 3,
                Less = 4,
                Equals = 6,
                NotEqual = 7
            }
            public enum DataType { Float=1, Int=3, Bool=4, Trigger=9 }
 
            [HideInInspector]public DataType dataType;
            [HideInInspector] public int paramHash;
            [Tooltip("This field cannot be edited, to change this condition edit the original transition then try to update this behaviour")]
            public string parameter;
            [Tooltip("This field cannot be edited, to change this condition edit the original transition then try to update this behaviour")]
            public Condition mode;
            [Tooltip("This field cannot be edited, to change this condition edit the original transition then try to update this behaviour")]
            public float threshold;
 
        #if UNITY_EDITOR
            public static TransitionCondition Create(AnimatorCondition data, AnimatorController editorAnimator)
            {
                var parameters = editorAnimator.parameters;
                AnimatorControllerParameter parameter = default;
                for(int i =0;i< parameters.Length;i++ )
                {
                    if(parameters[i].nameHash == Animator.StringToHash(data.parameter))
                    {
                        parameter = parameters[i];
                        break;
                    }
                }
 
                return new TransitionCondition()
                {
                    dataType = (DataType)parameter.type,
                    paramHash = parameter.nameHash,
                    parameter = parameter.name,
                    mode = (Condition)data.mode,
                    threshold = data.threshold
                };
            }
        #endif
 
 
            public bool Evaluate(Animator animator)
            {
                switch(dataType)
                {
                    case DataType.Float:
                        var floatValue = animator.GetFloat(paramHash);
                        switch(mode)
                        {
                            case Condition.Greater:
                                return floatValue > threshold;
                            case Condition.Less:
                                return floatValue < threshold;
                            case Condition.Equals:
                                return floatValue == threshold;
                            case Condition.NotEqual:
                                return floatValue != threshold;
 
                            default: return false;
 
                        }
                    case DataType.Int:
                        var intValue = animator.GetInteger(paramHash);
                        switch (mode)
                        {
                            case Condition.Greater:
                                return intValue > threshold;
                            case Condition.Less:
                                return intValue < threshold;
                            case Condition.Equals:
                                return intValue == threshold;
                            case Condition.NotEqual:
                                return intValue != threshold;
 
                            default: return false;
 
                        }
                    case DataType.Bool:
                        var boolValue = animator.GetBool(paramHash);
                        switch (mode)
                        {
                            case Condition.If:
                                return boolValue;
                            case Condition.IfNot:
                                return !boolValue;
 
                            default:
                                return false;
 
                        }
                    case DataType.Trigger:
                        var triggerValue = animator.GetBool(paramHash); // Triggers can be "read" as bool parameters
                        return triggerValue;
 
                    default: return false;
                }
            }
 
            public void Consume(Animator animator)
            {
                if (dataType == DataType.Trigger)
                {
                    animator.ResetTrigger(paramHash);
                }
            }
        }
        #endregion
    }
``` 

You place this on the statemachine parent and then instead of running all your transitions from an anystate inside that sub-statemachine node, you run it from this parent node. That statemachine will now suddenly behave like an "Any Internal State" node.

A little unknown fact about state machine behaviours is that at runtime Unity copies all StateMachineBehaviours that exist on a statemachine node to every AnimatorState inside that statemachine. This script basically bakes nearly any transition you make in the editor from that parent node to the SMB (which unity then copies to every internal node) and checks those conditions on Update to see if it should transition.

Spoiler: Some Caveats
```
For some reason NaughtyAttributes is not supported on SMBs and I didn't want to write a drawer. Also for some reason even though SMBs derive from ScriptableObjects they don't seem to raise Awake/OnEnable/OnDisable messages at expected times like they do for normal scriptable object assets. So I resorted to using OnValidate to get the data to bake transitions. This means when you change your transitions you have to manually change anything in its inspector before the transitions copy over.
I wasn't able to find a way, in code, to tell an animator to transition to just any node (as in, including AnimatorStateMachine nodes not just AnimatorState nodes) like you can in an animator controller. Thus transitions you define on statemachines that you intend to be copied cannot point to another statemachine directly (the transition simply won't work), its better to have it point to a default state within that statemachine thats tailored to specifically handle these types of transitions
Cause it didn't make sense in practice, This smb does not copy transitions that don't have any conditions. Exit times are also not baked (mostly cause that data is not available in UnityEditor.Animations.AnimatorTransition, for some reason)
```