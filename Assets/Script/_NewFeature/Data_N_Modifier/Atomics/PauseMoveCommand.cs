using System;
using System.Threading;
using UnityEngine;

namespace Sophia.DataSystem.Functional
{
    using Cysharp.Threading.Tasks;
    using Sophia.Entitys;
    using Sophia.Instantiates;
    public class PauseMoveCommand : IFunctionalToggleCommand<Entitys.Entity>
    {
        private Entitys.Entity OwnerRef;

        public string GetDescription() => "대상의 움직임을 정지시킵니다.";
        public string GetName() => "움직임 정지";

        public Sprite GetSprite()
        {
            throw new NotImplementedException();
        }

        public void Invoke(ref Entity referer)
        {
            if (referer is IMovable)
            {
                IMovable movableEntity = referer as IMovable;
                movableEntity.SetMoveState(false);
            }
        }

        public void Revert(ref Entity referer)
        {
            if (referer is IMovable)
            {
                IMovable movableEntity = referer as IMovable;
                movableEntity.SetMoveState(true);
            }
        }
    }
}
/*
private PauseMoveCommand    FunctionalPauseMoveCommand;

                Timer.OnFinished += Revert;
                
                FunctionalPauseMoveCommand = new PauseMoveCommand();

                Timer.OnStart       += () => FunctionalPauseMoveCommand.Invoke(ref TargetRef);
                Timer.OnFinished    += () => FunctionalPauseMoveCommand.Revert(ref TargetRef);
            }
*/