using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;
using Cysharp.Threading.Tasks;
using Sophia_Carriers;

public class AttackSkill_001 : AbstractSkill {
//  public Player                                   player;
//  public string                                   skillName;
//  public SKILL_RANK                               skillRank;
//  public SKILL_TYPE                               skillType;
//  public string                                   description;
//  public bool                                     IsReady = true;
//  public float                                    PassedTime = 0f;
//  public SerializedDictionary<SKILL_RANK, float>  coolTime = new SerializedDictionary<SKILL_RANK, float>();
//  protected SkillManager                          skillManager;
    public Weapon               weapon;
    public List<Projectile>     ProjectileQ;
    public Projectile           ProjectileE;
    public Projectile           ProjectileR;
    public List<float>          NumericRatioQ = new List<float> {1, 1.5f, 2f};
    public List<float>          NumericRatioE = new List<float> {0.8f, 1f, 1.5f};
    public List<float>          NumericRatioR = new List<float> {1, 1.5f, 2f};

    public Vector3              positionBeforeR;

    
    private void Awake() {
        this.skillName = "바람 처럼";
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

    protected override void UseQ() {AsyncUseQ().Forget();}
    protected override void UseE() {AsyncUseE().Forget();}
    protected override void UseR() {AsyncUseR().Forget();}
    
    private async UniTaskVoid AsyncUseQ(){
        Projectile useProjectile;
        float skillDamage = (float)(
            PlayerDataManager.GetEntityData().Power * 
            PlayerDataManager.GetWeaonData().DamageRatio *
            NumericRatioQ[(int)skillRank]
        );
        foreach(Projectile E in ProjectileQ){
            useProjectile = E.CloneProjectile();
            useProjectile.Init(player);
            useProjectile.ProjecttileDamage = skillDamage;
            player.carrierBucket.CarrierTransformPositionning(player, useProjectile);
            await UniTask.Delay(TimeSpan.FromSeconds(0.15f));
        }
    }

    private async UniTaskVoid AsyncUseE(){
        await AsyncDash();
        Projectile useProjectile;
        float skillDamage = (float)(
            PlayerDataManager.GetEntityData().Power * 
            PlayerDataManager.GetWeaonData().DamageRatio *
            NumericRatioQ[(int)skillRank]
        );
        useProjectile = ProjectileE.CloneProjectile();
        useProjectile.Init(player);
        useProjectile.ProjecttileDamage = skillDamage;
        player.carrierBucket.CarrierTransformPositionning(player, useProjectile);
    }

    private async UniTaskVoid AsyncUseR(){
        positionBeforeR = player.transform.position;
        await AsyncDash();
        Projectile useProjectile;
        float skillDamage = (float)(
            PlayerDataManager.GetEntityData().Power * 
            PlayerDataManager.GetWeaonData().DamageRatio *
            NumericRatioQ[(int)skillRank]
        );
        useProjectile = ProjectileE.CloneProjectile();
        useProjectile.Init(player);
        useProjectile.ProjecttileDamage = skillDamage;
        player.carrierBucket.CarrierTransformPositionning(player, useProjectile);
        float passedTime = 0;
        while(5f > passedTime){
            if(Input.GetKeyDown(KeyCode.R)){
                player.transform.position = positionBeforeR;
                return;
            }
            passedTime += Time.deltaTime;
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

    async UniTask AsyncDash(){
        player.Dash();
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
    }
}