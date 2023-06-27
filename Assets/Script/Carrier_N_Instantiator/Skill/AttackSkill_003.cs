using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;
using Cysharp.Threading.Tasks;
using Sophia_Carriers;

public class AttackSkill_003 : AbstractSkill {
    //  public Player                                   player;
    //  public string                                   skillName;
    //  public SKILL_RANK                               skillRank;
    //  public SKILL_TYPE                               skillType;
    //  public string                                   description;
    //  public bool                                     IsReady = true;
    //  public float                                    PassedTime = 0f;
    //  public SerializedDictionary<SKILL_RANK, float>  coolTime = new SerializedDictionary<SKILL_RANK, float>();
    
    public Weapon weapon;
    public MoveProjectile       moveProjectileShort;
    public MoveProjectile       moveProjectileWide;
    public List<float>          NumericRatioQ = new List<float> {1, 1.4f, 1.6f};
    public List<float>          NumericRatioE = new List<float> {0.8f, 1f, 1.2f};
    public List<float>          NumericRatioR = new List<float> {1.2f, 1.6f, 1.8f};
    
    private void Awake() {
        this.skillName = "바람의 상처";
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

    protected override void UseQ() {
        Projectile  useProjectile;
        float skillDamage = (float)(
            PlayerDataManager.GetEntityData().Power * 
            PlayerDataManager.GetWeaonData().DamageRatio *
            NumericRatioQ[(int)skillRank]
        );
        useProjectile = moveProjectileShort.CloneProjectile();
        useProjectile.Init(player);
        useProjectile.ProjecttileDamage = skillDamage;
        player.carrierBucket.CarrierTransformPositionning(player, useProjectile);
    }
    protected override void UseE() {
        Projectile  useProjectile;
        float skillDamage = (float)(
            PlayerDataManager.GetEntityData().Power * 
            PlayerDataManager.GetWeaonData().DamageRatio *
            NumericRatioE[(int)skillRank]
        );
        useProjectile = moveProjectileWide.CloneProjectile();
        useProjectile.Init(player);
        useProjectile.ProjecttileDamage = skillDamage;
        player.carrierBucket.CarrierTransformPositionning(player, useProjectile);
    }
    protected override void UseR() {
        Projectile  useProjectile;
        float skillDamage = (float)(
            PlayerDataManager.GetEntityData().Power * 
            PlayerDataManager.GetWeaonData().DamageRatio *
            NumericRatioR[(int)skillRank]
        );
        useProjectile = moveProjectileWide.CloneProjectile();
        useProjectile.Init(player);
        useProjectile.ProjecttileDamage = skillDamage;
        player.carrierBucket.CarrierTransformPositionning(player, useProjectile);
    }
}