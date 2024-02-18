using System.Collections;
using Sophia.Instantiates;
using Sophia.Instantiates.Skills;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
namespace Sophia.test {
    public class TEST_SkillManager : MonoBehaviour {
        #region Serialized Member
            [SerializeField] private Entitys.Entity _ownerEntity;
            [SerializeField] private bool _isBarrierSkill;
            [SerializeField] private bool _isMoveFaseterSkill;
            [SerializeField] private bool _isPowerUpSkill;
            [SerializeField] private bool _isStunConveySkill;
            #region Barrier
             [SerializeField] private SerialUserInterfaceData     _uiBarrierData;
            // [SerializeField] private SerialBarrierData           _barrierData;
            // [SerializeField] private SerialVisualData            _visualData;
            // [SerializeField] private SerialSkinData              _skinData;
            #endregion

            #region Move Faseter
            // [SerializeField] private SerialUserInterfaceData _uiFasterData;
            // [SerializeField] private SerialAffectorData _moveFasterData;

            #endregion

            #region Power Up

            // [SerializeField] private SerialUserInterfaceData _uiPowerUpData;
            // [SerializeField] private SerialAffectorData _PowerUpData;

            #endregion

            #region Add Stun Conveyer
            
            [SerializeField] private SerialUserInterfaceData _uiStunConveyerData;
            [SerializeField] private SerialAffectorData _stunConveyData;
            
            #endregion 

        #endregion
        
        UnityAction UseAction;
        BarrierSkill barrierSkill;
        MoveFasterSkill moveFasterSkill;
        PowerUpSkill powerUpSkill;
        AddStunConveyerSkill stunConveyerSkill;
        private void Awake() {
            StartCoroutine(LateInitialize(() => {
                if(_isBarrierSkill){
                    // barrierSkill = new BarrierSkill(in _uiData)
                    //                     .SetBarrierData(in _barrierData)
                    //                     .SetVisualFxData(in _visualData)
                    //                     .SetOwnerEntity(_ownerEntity);
                    // barrierSkill.AddToUpator();
                    // UseAction = barrierSkill.Use;
                }
                if(_isMoveFaseterSkill) {
                    // moveFasterSkill = new MoveFasterSkill(in _uiFasterData)
                    //                         .SetMoveFasterAffect(in _moveFasterData)
                    //                         .SetOwnerEntity(_ownerEntity);
                    // moveFasterSkill.AddToUpator();
                    // UseAction = moveFasterSkill.Use;
                }
                if(_isPowerUpSkill) {
                    // powerUpSkill = new PowerUpSkill(in _uiPowerUpData)
                    //                     .SetPowerUpAffect(in _PowerUpData)
                    //                     .SetOwnerEntity(_ownerEntity);
                    // powerUpSkill.AddToUpator();
                    // UseAction = powerUpSkill.Use;
                }
                if(_isStunConveySkill) {
                    stunConveyerSkill = new AddStunConveyerSkill(in _uiStunConveyerData)
                                            .SetStunData(_stunConveyData)
                                            .SetOwnerEntity(_ownerEntity);
                    stunConveyerSkill.AddToUpator();
                    UseAction = stunConveyerSkill.Use;
                }
            }));
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.Q)) {
                UseAction.Invoke();
            }
        }

        IEnumerator LateInitialize(UnityAction action) {
            yield return YieldInstructionCache.WaitForEndOfFrame;
            action.Invoke();
        }
    }
}