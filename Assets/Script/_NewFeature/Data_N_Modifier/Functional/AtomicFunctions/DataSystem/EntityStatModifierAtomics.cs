namespace Sophia.DataSystem.Functional
{
    using Sophia.DataSystem.Modifiers;
    using Sophia.DataSystem.Referer;
    class EntityStatModifierAtomics {
        private StatModifier statModifier;
        private EntityStatReferer statRefererRef;
        private E_NUMERIC_STAT_TYPE statTypeRef;
        public EntityStatModifierAtomics(SerialStatModifireDatas datas, E_NUMERIC_STAT_TYPE statType) {
            statModifier = new StatModifier(datas.amount, datas.calType, statType);
            statTypeRef = statType;
        }

        public void Invoke() {
            statRefererRef.GetStat(statTypeRef).AddModifier(statModifier);
        }
        public void Revert() {
            statRefererRef.GetStat(statTypeRef).RemoveModifier(statModifier);
        }
    }
}