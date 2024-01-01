using UnityEngine;

namespace Feature_NewData
{
    public enum E_AFFECT_TYPE {
        None = 0,

        // 화상, 독, 출혈, 수축, 냉기, 혼란, 공포, 스턴, 속박, 처형
        // 블랙홀
        Debuff = 100,
        Burn, Poisoned, Bleed, Contracted, Freeze, Confused, Fear, Stern, Bounded, Execution,
        BlackHole,

        // 이동속도증가, 고유시간가속, 공격력증가, 보호막상태, CC저항, 은신, 무적, 방어/페링, 투사체생성, 회피,
        Buff = 200,
        MoveSpeedUp, Accelerated, PowerUp, Barrier, Resist, Invisible, Invincible, Defence, ProjectileGenerate, Dodgeing, 
    }
    public class AffectorManager : MonoBehaviour {
        
    }
}