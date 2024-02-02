using UnityEngine;

namespace Sophia.DataSystem.Functional
{
    public interface IFunctionalCommand<T> : IUserInterfaceAccessible{
        public void Invoke(ref T referer);
    }

    public interface IUserInterfaceAccessible {
        public string GetName();
        public string GetDescription();
        public Sprite GetSprite();
    }
}