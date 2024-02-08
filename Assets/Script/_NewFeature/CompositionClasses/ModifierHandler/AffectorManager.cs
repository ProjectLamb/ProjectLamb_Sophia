using UnityEngine;
using System;
using System.Collections.Generic;

namespace Sophia.Composite
{
    using Sophia.Entitys;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Modifiers;

    public class AffectorManager : MonoBehaviour
    {
        
        #region SerializeMember

        [SerializeField] private Entity _entity;
        [SerializeField] private List<string> _currentAffectors;

        #endregion

        #region Member
        public IDataAccessible DataAccessible { get; private set; }
        public Dictionary<E_AFFECT_TYPE, Affector> AffectingStacks { get; private set; }
        public Stat Tenacity { get; private set; }
        Array AffectKeys;
        public void Init(float baseTenacity)
        {
            Tenacity = new Stat(baseTenacity,
                E_NUMERIC_STAT_TYPE.Tenacity,
                E_STAT_USE_TYPE.Ratio,
                OnTenacityUpdated
            );
        }

        private void OnTenacityUpdated()
        {
            Debug.Log("TenacityUpdated");
        }
        #endregion

        private void Awake()
        {
            AffectingStacks = new Dictionary<E_AFFECT_TYPE, Affector>();
            DataAccessible = _entity;
            AffectKeys = Enum.GetValues(typeof(E_AFFECT_TYPE));
        }

        public void Affect(Affector affector)
        {   
            E_AFFECT_TYPE newAffectorType = affector.AffectType;
            if (AffectingStacks.TryGetValue(newAffectorType, out Affector affectingAffector))
            {
                TerminateAffector(affectingAffector);
                Debug.Log("Updateed");
            }
            StartAffector(affector);
            UpdateDebugAffectList();
        }

        public void Recover(Affector affector)
        {
            TerminateAffector(affector);
            AffectingStacks.Remove(affector.AffectType);
            UpdateDebugAffectList();
        }

        private void Update()
        {
            foreach(E_AFFECT_TYPE affectType in AffectKeys) {
                if(AffectingStacks.TryGetValue(affectType, out Affector affectingAffector)) 
                {
                    Debug.Log(affectingAffector.AffectType.ToString());
                    RunAffector(affectingAffector);
                }
            }
        }
        
        #region Helper
        private void StartAffector(Affector affector) {
            affector.ChangeState(AffectorStartState.Instance);
            AffectingStacks.Add(affector.AffectType, affector);
            AffectingStacks[affector.AffectType].ExecuteState(_entity);
        } 
        
        private void TerminateAffector(Affector affector) {
            affector.ChangeState(AffectorTerminateState.Instance);
            affector.ExecuteState(_entity);
            AffectingStacks.Remove(affector.AffectType);
        }

        private void RunAffector(Affector affector) {
            if(affector.GetCurrentState() == AffectorRunState.Instance){
                affector.ExecuteState(_entity);
                affector.GetTimer().FrameTick(Time.deltaTime);
            }
            else if(affector.GetCurrentState() == AffectorTerminateState.Instance){
                TerminateAffector(affector);
            }
        }

        private void UpdateDebugAffectList() {
            _currentAffectors.Clear();
            foreach(E_AFFECT_TYPE affectType in AffectKeys) {
                if(AffectingStacks.TryGetValue(affectType, out Affector e)){
                    if(e != null && e != default) _currentAffectors.Add(affectType.ToString());
                }
            }
        }
        #endregion
    }
}


/*
private AffectorStackRemoveCommend affectorStackRemoveCommend;

public AffectorManager(float baseTenacity)
{
    Tenacity = new Stat(baseTenacity,
        E_NUMERIC_STAT_TYPE.Tenacity,
        E_STAT_USE_TYPE.Ratio,
        OnTenacityUpdated
    );
    foreach (E_AFFECT_TYPE E in Enum.GetValues(typeof(E_AFFECT_TYPE))) { AffectorStacks.Add(E, default); }
    affectorStackRemoveCommend = new AffectorStackRemoveCommend(AffectorStacks);

    AddToUpator();
}


public void ModifiyByAffector(Affector affector)
{
    E_AFFECT_TYPE stateType = affector.AffectType;
    if (!AffectorStacks.ContainsKey(stateType))
    {
        throw new System.Exception("현재 받아온 어펙터는 타입이 존재하지 않음");
    }
    if (AffectorStacks.TryGetValue(stateType, out Affector runnginAffector))
    {
        if (runnginAffector != null)
        {
            runnginAffector.CancleModify();
            AffectorStacks[stateType] = default;
        }
    }
    AffectorStacks[stateType] = affector;
    AffectorStacks[stateType].SetAccelarationByTenacity(this.Tenacity);
    AffectorStacks[stateType].OnRevert += () => affectorStackRemoveCommend.removeCommands.Add(stateType);
    AffectorStacks[stateType].Modifiy(Tenacity);
}

#region  Updator Implements
bool IsUpdatorBinded = false;
public bool GetUpdatorBind() => IsUpdatorBinded;

public void AddToUpator()
{
    GlobalTimeUpdator.CheckAndAdd(this);
    IsUpdatorBinded = true;
}

public void RemoveFromUpdator()
{
    GlobalTimeUpdator.CheckAndRemove(this);
    IsUpdatorBinded = false;
}

public void LateTick()
{
    return;
}

public void FrameTick()
{
    foreach (var affector in AffectorStacks)
    {
        // if(affector.Equals(default)) continue;
        affector.Value?.TickRunning();
    }
    affectorStackRemoveCommend.RemoveAffectorStack();
}

public void PhysicsTick()
{
    return;
}
#endregion
*/