using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.Analytics;

namespace Sophia.DataSystem.Modifiers
{
    using System;
    
    using Sophia.Entitys;
    using UnityEngine;

    public class ExtrasModifier<T> : IUserInterfaceAccessible
    {
        public readonly E_EXTRAS_PERFORM_TYPE PerfType = E_EXTRAS_PERFORM_TYPE.None;
        public readonly E_FUNCTIONAL_EXTRAS_TYPE FunctionalType = E_FUNCTIONAL_EXTRAS_TYPE.None;

        public readonly IFunctionalCommand<T> Value;
        public readonly int Order;
        private IFunctionalCommand<DamageInfo> damageInfoCommand;
        private object value;

        public ExtrasModifier(IFunctionalCommand<T> value, E_EXTRAS_PERFORM_TYPE perfType, E_FUNCTIONAL_EXTRAS_TYPE functionalType, int order) {
            this.Value = value;
            this.PerfType = perfType;
            this.FunctionalType = functionalType;
            Order = order;  
        }

        public ExtrasModifier(IFunctionalCommand<T> value, E_EXTRAS_PERFORM_TYPE perfType, E_FUNCTIONAL_EXTRAS_TYPE functionalType) : this(value, perfType, functionalType, 999){}

        public ExtrasModifier(IFunctionalCommand<DamageInfo> damageInfoCommand, object value)
        {
            this.damageInfoCommand = damageInfoCommand;
            this.value = value;
        }

        public ExtrasModifier()
        {
        }

        public string GetName() => Value.GetName();
        public string GetDescription() => Value.GetDescription();
        public Sprite GetSprite() => Value.GetSprite();

    }
}
/*

*/