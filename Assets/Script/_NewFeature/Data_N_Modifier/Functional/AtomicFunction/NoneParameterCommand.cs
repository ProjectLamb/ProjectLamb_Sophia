
using System.Text;
namespace Sophia.DataSystem.Functional.AtomFunctions
{
    using JetBrains.Annotations;
    using Sophia.Composite;
    using UnityEngine;
    using UnityEngine.Events;

    public static class GeneralCommand
    {
        public class NoneParameterCommand : IFunctionalCommand<object>, IRandomlyActivatable<object>
        {
            private UnityAction actionRef;
            public NoneParameterCommand(UnityAction unityAction) {
                actionRef = unityAction;
            }
            public void Invoke(ref object referer)
            {
                actionRef.Invoke();
            }

#region User Interface
            private string name;
            private string description;
            private Sprite icon;
            public IFunctionalCommand<object> SerUserInterfaceInfo(SerialUserInterfaceData userInterface) {
                name = userInterface._name;
                description = userInterface._description;
                icon = userInterface._icon;
                return this;
            }
            public string GetName()         => name;
            public string GetDescription()  => description;
            public Sprite GetSprite()       => icon;
#endregion

#region Randomly Activate
            private System.Random random;
            private float percentage;
            private bool IsRandomlyActivate = false;
            public IFunctionalCommand<object> SetRandomPercentage(int activatePercentage)  { 
                random = new System.Random();
                percentage = activatePercentage;
                IsRandomlyActivate = true;
                return this;
            }
            public bool GetIsActivated() => percentage > random.Next(100);
#endregion         
        }
    }
}