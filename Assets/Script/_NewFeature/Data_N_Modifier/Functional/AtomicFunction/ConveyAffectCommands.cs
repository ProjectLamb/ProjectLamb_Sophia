
using System.Text;
namespace Sophia.DataSystem.Functional.AtomFunctions
{
    using Sophia.DataSystem.Modifiers.ConcreteAffector;
    using Sophia.Entitys;
    using Sophia.Instantiates;
    using UnityEngine;

    public class ConveyAffectCommand
    {
        public class FactoryExecuteCommand : IFunctionalCommand<Entity>, IRandomlyActivatable<Entity>
        {
            private SerialAffectorData serialAffectorDataRef;

            public string GetDescription() => "처형";
            public string GetName() => "처형";
            public Sprite GetSprite() => null;
            public FactoryExecuteCommand(in SerialAffectorData serialAffectorData) { serialAffectorDataRef = serialAffectorData; }
            public void Invoke(ref Entity referer){ 
                if(IsRandomlyActivate && !GetIsActivated()) return;
                referer.Affect(new ExecutionStrike(in serialAffectorDataRef));
            }


#region Randomly Activate
            private System.Random random;
            private float percentage;
            private bool IsRandomlyActivate = false;
            public IFunctionalCommand<Entity> SetRandomPercentage(int activatePercentage)  { 
                random = new System.Random();
                percentage = activatePercentage;
                IsRandomlyActivate = true;
                return this;
            }
            public bool GetIsActivated() => percentage > random.Next(100);
#endregion
        }
        
        public class FactoryBurnAffectCommand : IFunctionalCommand<Entity>, IRandomlyActivatable<Entity>
        {
            private SerialAffectorData serialAffectorDataRef;
            public FactoryBurnAffectCommand(in SerialAffectorData serialAffectorData) { serialAffectorDataRef = serialAffectorData; }
            public void Invoke(ref Entity referer){ 
                if(IsRandomlyActivate && !GetIsActivated()) return;
                referer.Affect(new BurnAffect(in serialAffectorDataRef));
            }

#region UI Access
            public string GetName() => "화상";
            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }
            public string GetDescription()
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"플레이어가 화염 영역 위 위치할 때 활성화 되며, {serialAffectorDataRef._baseDurateTime}초 뒤 화상효과를 유발한 후 사라집니다\n");
                stringBuilder.Append($"화염 영역을 벗어나게 되는 경우, 화염 영역 위 존재한 시간과 비례해 효과가 유지 된 후 사라집니다.");
                return stringBuilder.ToString();
            }
#endregion

#region Randomly Activate
            private System.Random random;
            private float percentage;
            private bool IsRandomlyActivate = false;
            public IFunctionalCommand<Entity> SetRandomPercentage(int activatePercentage)  { 
                random = new System.Random();
                percentage = activatePercentage;
                IsRandomlyActivate = true;
                return this;
            }
            public bool GetIsActivated() => percentage > random.Next(100);
#endregion
        }

        public class FactoryPoisionAffectCommand : IFunctionalCommand<Entity>, IRandomlyActivatable<Entity>
        {
            private SerialAffectorData serialAffectorDataRef;
            public FactoryPoisionAffectCommand(in SerialAffectorData serialAffectorData) { serialAffectorDataRef = serialAffectorData; }
            public void Invoke(ref Entity referer){ 
                if(IsRandomlyActivate && !GetIsActivated()) return;
                referer.Affect(new PoisonedAffect(in serialAffectorDataRef));
            }

            #region UI Access
            public string GetName() => "중독";
            public string GetDescription()
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"본 효과는 일정시간(0.5초)마다 플레이어에게 피해를 입히며, 그 2배의 시간(1초) 뒤 사라집니다.").Append("\n");
                stringBuilder.Append($"본 효과의 피해는 플레이어 전체 체력에 비례해 적용됩니다.(체력 퍼뎀)").Append("\n");
                stringBuilder.Append($"본 효과는 중첩되며, 효과가 사라지기 전 새롭게 상태이상에 걸리게 된 경우 중첩이 증가하며 효과의 갱신 시간은 초기화됩니다.").Append("\n");
                stringBuilder.Append($"1초가 채 가기 전 새롭게 상태이상에 걸린 경우, 효과는 누적되며 누적된 독 효과는 다시 1초 뒤 사라진다, 데미지는 0.5초마다 꾸준히 들어온다.").Append("\n");
                stringBuilder.Append($"최대 5번까지 효과 중첩이 가능하며, 스택마다 일정시간(0.5초)마다 받는 피해는 산술적으로 증가합니다.").Append("\n");
                return stringBuilder.ToString();
            }
            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }


            #endregion

#region Randomly Activate
            private System.Random random;
            private float percentage;
            private bool IsRandomlyActivate = false;
            public IFunctionalCommand<Entity> SetRandomPercentage(int activatePercentage)  { 
                random = new System.Random();
                percentage = activatePercentage;
                IsRandomlyActivate = true;
                return this;
            }
            public bool GetIsActivated() => percentage > random.Next(100);
#endregion
        }
        
        public class FactoryBleedAffectCommand : IFunctionalCommand<Entity>, IRandomlyActivatable<Entity>
        {
            private SerialAffectorData serialAffectorDataRef;
            public FactoryBleedAffectCommand(in SerialAffectorData serialAffectorData) { serialAffectorDataRef = serialAffectorData; }
            public void Invoke(ref Entity referer){ 
                if(IsRandomlyActivate && !GetIsActivated()) return;
                referer.Affect(new BleedAffect(in serialAffectorDataRef));
            }
            #region UI Access
            public string GetName() => "출혈";

            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }

            public string GetDescription()
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("본 효과는 일정시간(0.5초)마다 플레이어에게 피해를 입히며, 그 2배의 시간(1초) 뒤 사라집니다.").Append("\n");
                stringBuilder.Append("본 효과의 피해는 플레이어의 방어력을 무시하고 적용됩니다.").Append("\n");
                stringBuilder.Append("본 효과는 중첩되지 않으나, 효과가 사라지기 전 새롭게 상태이상에 걸리게 된 경우 효과의 갱신 시간은 초기화됩니다.").Append("\n");
                stringBuilder.Append("(1초가 채 가기 전 새롭게 상태이상에 걸린 경우, 출혈 효과는 다시 1초 뒤 사라진다, 데미지는 0.5초마다 꾸준히 들어온다.").Append("\n");
                return stringBuilder.ToString();
            }

            #endregion
#region Randomly Activate
            private System.Random random;
            private float percentage;
            private bool IsRandomlyActivate = false;
            public IFunctionalCommand<Entity> SetRandomPercentage(int activatePercentage)  { 
                random = new System.Random();
                percentage = activatePercentage;
                IsRandomlyActivate = true;
                return this;
            }
            public bool GetIsActivated() => percentage > random.Next(100);
#endregion
        }

        public class FactoryColdAffectCommand : IFunctionalCommand<Entity>, IRandomlyActivatable<Entity>
        {
            private SerialAffectorData serialAffectorDataRef;
            public FactoryColdAffectCommand(in SerialAffectorData serialAffectorData) { serialAffectorDataRef = serialAffectorData; }
            public void Invoke(ref Entity referer){ 
                if(IsRandomlyActivate && !GetIsActivated()) return;
                referer.Affect(new ColdAffect(in serialAffectorDataRef));
            }
            #region UI Access
            public string GetName() => "냉기";
            public string GetDescription()
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(" 플레이어가 한기 영역 위 위치할 때 활성화 되며, 일정시간동안 플레이어의 이동 속도가 늦춰집니다.").Append("\n");
                stringBuilder.Append(" 일정 시간(1초) 뒤 빙결 효과를 유발한 후 사라집니다.").Append("\n");
                stringBuilder.Append(" 한기 영역을 벗어나게 되는 경우, 한기 영역 위 존재한 시간과 비례해 효과가 유지 된 후 사라집니다.").Append("\n");
                return stringBuilder.ToString();
            }
            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }
            #endregion

#region Randomly Activate
            private System.Random random;
            private float percentage;
            private bool IsRandomlyActivate = false;
            public IFunctionalCommand<Entity> SetRandomPercentage(int activatePercentage)  { 
                random = new System.Random();
                percentage = activatePercentage;
                IsRandomlyActivate = true;
                return this;
            }
            public bool GetIsActivated() => percentage > random.Next(100);
#endregion
        }
        
        public class FactoryStunAffectCommand : IFunctionalCommand<Entity>, IRandomlyActivatable<Entity>
        {
            private SerialAffectorData serialAffectorDataRef;
            public FactoryStunAffectCommand(in SerialAffectorData serialAffectorData) { serialAffectorDataRef = serialAffectorData; }
            public void Invoke(ref Entity referer){

                if(IsRandomlyActivate && !GetIsActivated()) return;
                referer.Affect(new StunAffect(in serialAffectorDataRef));
                Debug.Log(referer.name);
            }

            #region UI Access
            public string GetName() => "스턴";
            public string GetDescription() => $"스턴에 걸린 상대는 {(serialAffectorDataRef._baseDurateTime).ToString()}초 간 행동불능 및 이동불가 상태가 됩니다.";
            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }

            #endregion
#region Randomly Activate
            private System.Random random;
            private float percentage;
            private bool IsRandomlyActivate = false;
            public IFunctionalCommand<Entity> SetRandomPercentage(int activatePercentage)  { 
                random = new System.Random();
                percentage = activatePercentage;
                IsRandomlyActivate = true;
                return this;
            }
            public bool GetIsActivated() => percentage > random.Next(100);
#endregion
        }
        
        public class FactoryBoundedAffectCommand : IFunctionalCommand<Entity>, IRandomlyActivatable<Entity>
        {
            private SerialAffectorData serialAffectorDataRef;
            public FactoryBoundedAffectCommand(in SerialAffectorData serialAffectorData) { serialAffectorDataRef = serialAffectorData; }
            public void Invoke(ref Entity referer){ 
                if(IsRandomlyActivate && !GetIsActivated()) return;
                referer.Affect(new BoundedAffect(in serialAffectorDataRef));
            }

            #region UI Access
            public string GetDescription() => "이동불가 상태가 됩니다(조작 가능)";

            public string GetName() => "속박";

            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }

            #endregion
#region Randomly Activate
            private System.Random random;
            private float percentage;
            private bool IsRandomlyActivate = false;
            public IFunctionalCommand<Entity> SetRandomPercentage(int activatePercentage)  { 
                random = new System.Random();
                percentage = activatePercentage;
                IsRandomlyActivate = true;
                return this;
            }
            public bool GetIsActivated() => percentage > random.Next(100);
#endregion
        }

        public class FactoryKnockbackAffectCommand : IFunctionalCommand<Entity>, IRandomlyActivatable<Entity>
        {
            private SerialAffectorData serialAffectorDataRef;
            private Transform OwnerTransformRef;
            public FactoryKnockbackAffectCommand(in SerialAffectorData serialAffectorData, Transform owner) { 
                serialAffectorDataRef = serialAffectorData; 
                OwnerTransformRef = owner;
            }
            public void Invoke(ref Entity referer){ 
                if(IsRandomlyActivate && !GetIsActivated()) return;
                referer.Affect(new KnockbackAffect(in serialAffectorDataRef, OwnerTransformRef));
            }

            #region UI Access
            public string GetName() => "넉백";
            public string GetDescription() => $"넉백 에 걸린 상대는 {(serialAffectorDataRef._baseDurateTime).ToString()}초 간 강제 행동이 되며, 밀려납니다.";
            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }

            #endregion
#region Randomly Activate
            private System.Random random;
            private float percentage;
            private bool IsRandomlyActivate = false;
            public IFunctionalCommand<Entity> SetRandomPercentage(int activatePercentage)  { 
                random = new System.Random();
                percentage = activatePercentage;
                IsRandomlyActivate = true;
                return this;
            }
            public bool GetIsActivated() => percentage > random.Next(100);
#endregion
        }
        
        public class FactoryBlackHoleAffectCommand : IFunctionalCommand<Entity>, IRandomlyActivatable<Entity>
        {
            private SerialAffectorData serialAffectorDataRef;
            private Transform OwnerTransformRef;
            public FactoryBlackHoleAffectCommand(in SerialAffectorData serialAffectorData, Transform owner) { 
                serialAffectorDataRef = serialAffectorData; 
                OwnerTransformRef = owner;
            }
            public void Invoke(ref Entity referer){ 
                if(IsRandomlyActivate && !GetIsActivated()) return;
                referer.Affect(new BlackHoleAffect(in serialAffectorDataRef, OwnerTransformRef));
            }

            #region UI Access
            public string GetName() => "이끌림";
            public string GetDescription() => $"이끌림 에 걸린 상대는 {(serialAffectorDataRef._baseDurateTime).ToString()}초 간 강제행동을 하며. 끌어당겨집니다.";
            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }

            #endregion
#region Randomly Activate
            private System.Random random;
            private float percentage;
            private bool IsRandomlyActivate = false;
            public IFunctionalCommand<Entity> SetRandomPercentage(int activatePercentage)  { 
                random = new System.Random();
                percentage = activatePercentage;
                IsRandomlyActivate = true;
                return this;
            }
            public bool GetIsActivated() => percentage > random.Next(100);
#endregion
        }
        
        public class FactoryAirborneAffectCommand : IFunctionalCommand<Entity>, IRandomlyActivatable<Entity>
        {
            private SerialAffectorData serialAffectorDataRef;
            public FactoryAirborneAffectCommand(in SerialAffectorData serialAffectorData) { serialAffectorDataRef = serialAffectorData; }
            public void Invoke(ref Entity referer){ 
                if(IsRandomlyActivate && !GetIsActivated()) return;
                referer.Affect(new AirborneAffect(in serialAffectorDataRef));
            }

            #region UI Access
            public string GetName() => "에어본";
            public string GetDescription() => $"에어본 에 걸린 상대는 {(serialAffectorDataRef._baseDurateTime).ToString()}초 간 공중에 떠오릅니다.";
            public Sprite GetSprite()
            {
                throw new System.NotImplementedException();
            }
            #endregion
#region Randomly Activate
            private System.Random random;
            private float percentage;
            private bool IsRandomlyActivate = false;
            public IFunctionalCommand<Entity> SetRandomPercentage(int activatePercentage)  { 
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