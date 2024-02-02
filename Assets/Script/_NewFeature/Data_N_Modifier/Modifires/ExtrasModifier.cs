using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.Analytics;

namespace Sophia.DataSystem.Modifiers
{
    using Sophia.DataSystem.Functional;
    using UnityEngine;

    public class ExtrasModifier<T> : IUserInterfaceAccessible
    {
        public readonly E_EXTRAS_PERFORM_TYPE PerfType = E_EXTRAS_PERFORM_TYPE.None;
        public readonly E_FUNCTIONAL_EXTRAS_TYPE FunctionalType = E_FUNCTIONAL_EXTRAS_TYPE.None;

        public readonly IFunctionalCommand<T> Functional;
        public readonly int Order;

        public ExtrasModifier(IFunctionalCommand<T> functional, E_EXTRAS_PERFORM_TYPE perfType, E_FUNCTIONAL_EXTRAS_TYPE functionalType, int order) {
            this.Functional = functional;
            this.PerfType = perfType;
            this.FunctionalType = functionalType;
            Order = order;
        }
        public ExtrasModifier(IFunctionalCommand<T> functional, E_EXTRAS_PERFORM_TYPE perfType, E_FUNCTIONAL_EXTRAS_TYPE functionalType) : this(functional, perfType, functionalType, 999){}

        public string GetName() => Functional.GetName();
        public string GetDescription() => Functional.GetDescription();
        public Sprite GetSprite() => Functional.GetSprite();
    }
}