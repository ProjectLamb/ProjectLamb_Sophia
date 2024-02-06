using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

namespace Sophia
{
    using Sophia.DataSystem;
    public class GlobalEvent : MonoBehaviour {
        public Extras<object> OnEnemyDie;

        private void Awake() {
            OnEnemyDie = new Extras<object>(E_FUNCTIONAL_EXTRAS_TYPE.EnemyDie, null);
        }
        private void Start() {
            GameManager.Instance.PlayerGameObject
                .GetComponent<Sophia.Entitys.Player>()
                .GetExtrasReferer()
                .SetRefExtras<object>(this.OnEnemyDie);
        }
    }
}