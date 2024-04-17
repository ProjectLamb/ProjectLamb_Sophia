
using UnityEngine;

namespace Sophia.Instantiates
{
    using Sophia.Entitys;
    using UnityEngine.VFX;

    public class HealthItemObject : ItemObject
    {
        [SerializeField] public int Health;
        [SerializeField] PurchaseComponent _purchaseComponent;
        public void SetHealth(int data) => Health = data;

        protected override void OnTriggerLogic(Collider entity)
        {
            if (!IsReadyToTrigger) { return; }
            if (entity.TryGetComponent<Player>(out Player player))
            {
                if (TryGetComponent<PurchaseComponent>(out _purchaseComponent))
                {
                    if (!_purchaseComponent.Purchase(player)) return;
                }
                player.GetLifeComposite().Healed(Health);
                _audioSource?.Play();
                _lootVFX.Stop();
                _lootObject.SetActive(false);
                IsReadyToTrigger = false;
                if (this._isDestroyable) Destroy(gameObject, 2f);
            }
        }
    }
}