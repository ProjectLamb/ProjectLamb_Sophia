using UnityEngine;

namespace Sophia.DataSystem.Functional {
    public class TickDamageCommand : IFunctionalCommand<Entitys.Entity>
    {
        private Entitys.Entity OwnerRef;
        private float BaseTickDamage;
        private float TickDamageRatio;
        private float IntervalTime;

        public TickDamageCommand(SerialTickDamageAffectData tickDamageAffectData) {
            BaseTickDamage = tickDamageAffectData._baseTickDamage;
            TickDamageRatio = tickDamageAffectData._tickDamageRatio;
            IntervalTime = tickDamageAffectData._intervalTime;
        }

        public TickDamageCommand() : this(new SerialTickDamageAffectData{ _baseTickDamage = 1f, _tickDamageRatio = 1.0f, _intervalTime = Time.deltaTime}){
        }

        public void Invoke(ref Entitys.Entity referer)
        {
            referer.GetDamaged((int)CalcTickDamage());
        }
        public void Revert(Entitys.Entity referer) {return;}

        private float CalcTickDamage() => TickDamageRatio * BaseTickDamage;

        public string GetName() => "틱 데미지";

        public string GetDescription() => $"대상의 체력을 {IntervalTime}초당 {BaseTickDamage * TickDamageRatio}만큼의 피해를 입힙니다.";

        public Sprite GetSprite()
        {
            throw new System.NotImplementedException();
        }
    }
}
/*
private TickDamageCommand FunctionalTickDamageCommand;


public BurnAffect SetTickDamage(SerialTickDamageAffectData serialTickDamageAffectData)
{
    if(FunctionalTickDamageCommand != null) return this;

    FunctionalTickDamageCommand = new TickDamageCommand(serialTickDamageAffectData);
    void TickDamage() => FunctionalTickDamageCommand.Invoke(ref TargetRef);
    
    Timer.SetIntervalTime(CalcDurateTime(serialTickDamageAffectData._intervalTime));
    Timer.OnInterval += TickDamage;
    return this;
}
*/