using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

// public enum E_FUNCTIONAL_EXTRAS_TYPE
// {
//     None = 0,
//     ENTITY_TYPE = 10,
//         Move, Damaged, Attack, ConveyAffect, Dead, Idle, PhysicTriggered,
//     PLAYER_TYPE = 20,
//         Dash, Skill, GearcoinTriggered, HealthTriggered,
//     WEAPON_TYPE = 30, 
//         WeaponUse, ProjectileRestore, WeaponConveyAffect, 
//     SKILL_TYPE = 40,
//         SkillUse, SkillRefilled, SkillConveyAffect,
//     PROJECTILE_TYPE = 50,
//         Created, Triggerd, Released, Forwarding,
//     GLOBAL_TYPE = 60,
//         EnemyHit, EnemyDie, StageClear, StageEnter
// }

// public enum E_AFFECT_TYPE {
//     None = 0,

//     // 화상, 독, 출혈, 수축, 냉기, 혼란, 공포, 스턴, 속박, 처형
//     // 블랙홀
//     Debuff = 100,
//     Burn, Poisoned, Bleed, Contracted, Cold, Confused, Fear, Stun, Bounded, 
//     Knockback, BlackHole, Airborne, Execution,

//     // 이동속도증가, 고유시간가속, 공격력증가, 보호막상태, CC저항, 은신, 무적, 방어/페링, 투사체생성, 회피,
//     Buff = 200,
//     MoveSpeedUp, Accelerated, PowerUp, Barrier, Resist, Invisible, Invincible, Defence, ProjectileGenerate, Dodgeing, 
// }   

namespace Sophia.Composite.RenderModels
{
    public class MaterialVFXManager : MonoBehaviour {
        [SerializeField] private GameObject _skinObject;
        [SerializeField] private DB.ScriptableCommonEntityMaterial _commonEntityMaterialRefer;
        [SerializeField] private List<Material> _materials = new List<Material>();
        /// <summary>
        /// 한 엔티티에 적용되는 마테리얼이 아트팀 모델러가 2개 이상 넘겨줌에 대비하기 위함.  
        /// </summary>
        [SerializedDictionary("MaterialKey", "Renderer")]
        [SerializeField] private SerializedDictionary<Material, List<Renderer>> _originVarientMaterial;
        public Material CommonFunctionalActMaterial;
        public Material CommonAffectMaterial;
        private readonly Dictionary<Material, Material[]> sharedMaterialsByVarientMaterial = new Dictionary<Material, Material[]>();

        [ContextMenu("InitMaterials")]
        public void Init() {
            CommonFunctionalActMaterial = _commonEntityMaterialRefer.CopyFunctionalActMaterialInstant();
            CommonAffectMaterial = _commonEntityMaterialRefer.CopyAffectMaterialInstant();
        
            InitializeShaderMaterials();
            SetSharedMaterialInRenderer();
        }

        public void InitializeShaderMaterials() {
            _originVarientMaterial ??= new SerializedDictionary<Material, List<Renderer>>();
            foreach(Material mat in _materials) {
                _originVarientMaterial.Add(mat, new List<Renderer>());
                sharedMaterialsByVarientMaterial.Add(mat, new Material[3] {mat, CommonFunctionalActMaterial, CommonAffectMaterial});
            }
        }

        public void SetSharedMaterialInRenderer() {
            foreach(Transform skinTransform in _skinObject.transform) {
                Renderer renderer = skinTransform.GetComponent<Renderer>();
                foreach(Material mat in _materials) {
                    if(mat == renderer.sharedMaterial) {
                        _originVarientMaterial[mat].Add(renderer);
                        renderer.sharedMaterials = sharedMaterialsByVarientMaterial[mat];
                    }
                }
            }
        }
    }
}