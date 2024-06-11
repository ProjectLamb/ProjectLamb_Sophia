using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;
using Cysharp.Threading.Tasks;
using Sophia_Carriers;


public class AttackSkill_002 : AbstractSkill {
//  public Player                                   player;
//  public string                                   skillName;
//  public SKILL_RANK                               skillRank;
//  public SKILL_TYPE                               skillType;
//  public string                                   description;
//  public bool                                     IsReady = true;
//  public float                                    PassedTime = 0f;
//  public SerializedDictionary<SKILL_RANK, float>  coolTime = new SerializedDictionary<SKILL_RANK, float>();

    public Weapon weapon;
    public Projectile           circleProjectile;
    public List<float>          NumericRatioQ = new List<float> {0.8f, 1f, 1.5f};
    public List<float>          NumericRatioE = new List<float> {0.8f, 1f, 1.5f};
    public List<float>          NumericRatioR = new List<float> {1.2f, 1.5f, 2f};
    float  duration;
    float  skillNumeric;
    private void Awake() {
        this.skillName = "갈아버리기";
        //coolTime?.Add(SKILL_RANK.NORMAL  , 15f);
        //coolTime?.Add(SKILL_RANK.RARE    , 15f);
        //coolTime?.Add(SKILL_RANK.EPIC    , 15f);
    }

    public override void Init(Player _player)
    {
        base.Init(_player);
        skillType = SKILL_TYPE.ATTACK;
        weapon = _player.weaponManager.weapon;
    }
    protected override void Indicate(){
        
    }
    protected override void UseQ() {
        duration = 2f;
        skillNumeric = NumericRatioQ[(int)skillRank];
        AsyncUse().Forget();
    }
    protected override void UseE() {
        duration = 2f;
        skillNumeric = NumericRatioE[(int)skillRank];
        AsyncUse().Forget();
    }
    protected override void UseR() {
        duration = 5f;
        skillNumeric = NumericRatioR[(int)skillRank];
        AsyncUse().Forget();
    }
    private async UniTaskVoid AsyncUse(){
        Projectile useProjectile;
        float useGap = 0.25f;
        int useCount = (int)(duration / useGap);
        float skillDamage = (float)(
            PlayerDataManager.GetEntityData().Power * 
            PlayerDataManager.GetWeaonData().DamageRatio *
            skillNumeric
        );
        for(int i = 0 ; i < useCount; i++){
            useProjectile = circleProjectile.CloneProjectile();
            useProjectile.Init(player);
            useProjectile.ProjecttileDamage = skillDamage;
            player.carrierBucket.CarrierTransformPositionning(player, useProjectile);
            await UniTask.Delay(TimeSpan.FromSeconds(useGap));
        }
    }

}