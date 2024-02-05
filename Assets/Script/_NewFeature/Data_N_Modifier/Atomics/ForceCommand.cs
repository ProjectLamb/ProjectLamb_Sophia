using System;
using System.Threading;
using UnityEngine;

namespace Sophia.DataSystem.Functional
{
    using System.Text;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using Sophia.Entitys;
    using Sophia.Instantiates;
    public class ImpulseForceCommand : IFunctionalCommand<Entitys.Entity>
    {
        private Entitys.Entity OwnerRef;
        private float ForceAmount;
        private float IntervalTime;

        public ImpulseForceCommand(SerialPhysicsAffectData serialPhysicsAffectData, Entity owner) {
            ForceAmount     = serialPhysicsAffectData._physicsForce;
            IntervalTime    = serialPhysicsAffectData._intervalTime;
            OwnerRef        = owner;
        }

        public string GetDescription() => "Entity을 즉각적으로 특정 방향으로 보냅니다.";

        public string GetName() => "강제 이동";

        public Sprite GetSprite() {
            return null;
        }

        public void Invoke(ref Entity referer)
        {
            UnityEngine.Vector3 Dir = UnityEngine.Vector3.Normalize(referer.entityRigidbody.position - OwnerRef.entityRigidbody.position);
            referer.entityRigidbody.AddForce(Dir * ForceAmount, ForceMode.Impulse);
        }
    }

    public class GradualForceCommand :IFunctionalCommand<Entitys.Entity> 
    {
        private Entitys.Entity OwnerRef;
        private float ForceAmount;
        private float IntervalTime;

        public GradualForceCommand(SerialPhysicsAffectData serialPhysicsAffectData, Entity owner) {
            ForceAmount     = serialPhysicsAffectData._physicsForce;
            IntervalTime    = serialPhysicsAffectData._intervalTime;
            OwnerRef        = owner;
        }

        public string GetDescription() => "Entity을 점진적으로 특정 방향으로 보냅니다.";

        public string GetName() => "강제 이동";

        public Sprite GetSprite()
        {
            throw new NotImplementedException();
        }

        public void Invoke(ref Entity referer)
        {
            UnityEngine.Vector3 Dir = UnityEngine.Vector3.Normalize(referer.entityRigidbody.position - OwnerRef.entityRigidbody.position);
            referer.entityRigidbody.AddForce(Dir * ForceAmount * IntervalTime, ForceMode.VelocityChange);
        }
    }

    public class DotweenForceCommand : IFunctionalCommand<Entitys.Entity>
    {
        private Entitys.Entity OwnerRef;
        private float ForceAmount;
        private float BaseDurateTime;

        public DotweenForceCommand(SerialPhysicsAffectData serialPhysicsAffectData, float baseDurateTime) {
            ForceAmount = serialPhysicsAffectData._physicsForce;
            BaseDurateTime = baseDurateTime;
        }

        public string GetDescription() => "Entity을 특정 방향으로 보냅니다.";
        public string GetName() => "강제 이동";

        public Sprite GetSprite()
        {
            throw new NotImplementedException();
        }

        public void Invoke(ref Entity referer)
        {
            GameObject targetModel = referer.GetModelManger().GetModelObject();
            targetModel.transform.DOLocalJump(UnityEngine.Vector3.zero, ForceAmount, 1, BaseDurateTime);
        }
    }
}

/*
            public PushBackCommand FunctionalKnockbackAffectCommand;

*/