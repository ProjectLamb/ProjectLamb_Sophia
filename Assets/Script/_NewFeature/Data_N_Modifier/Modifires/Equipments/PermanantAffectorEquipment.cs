using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sophia;
using Sophia.DataSystem.Modifiers.ConcreteAffectors;
using Sophia.DataSystem.Modifiers.Affector;
using UnityEngine.UIElements;
using Sophia.DataSystem;
using Sophia.Instantiates;
using Sophia.DataSystem.Functional;
using Sophia.DataSystem.Modifiers;
using Sophia.DataSystem.Numerics;

public class PermanantAffectorEquipment : Carrier, IUserInterfaceAccessible
{ //, IPlayerDataApplicant{
    [SerializeField] public string _equipmentName;
    [SerializeField] public Sprite _icon;
    [SerializeField] public string _description;

    [SerializeField] public E_AFFECT_TYPE _affectType;
    [SerializeField] public Material _material;
    [SerializeField] public Sophia.Instantiates.VisualFXObject _visualFx;
    [SerializeField] public float _baseDurateTime;
    [SerializeField] public float _intervalTime;
    [SerializeField] public float _tickDamageRatio;
    [SerializeField] private float _tickDamage;
    [SerializeField] SerialCalculateDatas _calculateDatas;

    ExtrasModifier<Sophia.Entitys.Entity> extrasModifier;
    Sophia.Entitys.Entity OwnerRef;
    IFunctionalCommand<Sophia.Entitys.Entity> affectCommand;
    private void Awake()
    {

        /******************************************************

        Extras<Sophia.Entitys.Entity> TargetAffectedExtrasRef
            = OwnerRef.GetExtras<Sophia.Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected);
        
        ******************************************************/

        /******************************************************
        
        ExtrasModifier<Sophia.Entitys.Entity> extrasModifier 
            = new ExtrasModifier<Sophia.Entitys.Entity>( UnityActionRef<Sophia.Entitys.Entity> ));

            UnityActionRef<Sophia.Entitys.Entity> 내부는 반드시 Affector 객체가 생겨야 한다.
        
        ******************************************************/

        /******************************************************
        
        TargetAffectedExtrasRef.AddModifier(this.extrasModifier);
        
        ******************************************************/
        switch (_affectType)
        {
            case E_AFFECT_TYPE.Poisoned:
                {
                    affectCommand = new PoisionAffectConveyerCommand(
                        this.OwnerRef,
                        _material, _visualFx,
                        _baseDurateTime, _intervalTime, _tickDamage, _tickDamageRatio
                    );
                    break;
                }
            case E_AFFECT_TYPE.Stern:
                {
                    affectCommand = new SternAffectConveyerCommand(
                        this.OwnerRef,
                        _material, _visualFx,
                        _baseDurateTime, _intervalTime, _tickDamage, _tickDamageRatio
                    );
                    break;
                }
            case E_AFFECT_TYPE.Airborne:
                {
                    affectCommand = new AirborneAffectConveyerCommand(
                        this.OwnerRef,
                        _material, _visualFx,
                        _baseDurateTime, _intervalTime, _tickDamage, _tickDamageRatio,
                        3
                    );
                    break;
                }
            case E_AFFECT_TYPE.Frozen:
                {
                    affectCommand = new FrozenAffectConveyerCommand(
                        this.OwnerRef,
                        _material, _visualFx,
                        _baseDurateTime, _intervalTime, _tickDamage, _tickDamageRatio,
                        _calculateDatas
                    );
                    break;
                }
            case E_AFFECT_TYPE.BlackHole :
                {
                    affectCommand = new BlackHoleAffectConveyerCommand(
                        this.OwnerRef,
                        null, null,
                        _baseDurateTime, _intervalTime, -1, -1,
                        50
                    );
                    break;
                }
            default: { throw new System.Exception("현재 알맞는 어펙터가 없음"); }
        }

        extrasModifier = new ExtrasModifier<Sophia.Entitys.Entity>(affectCommand, E_EXTRAS_PERFORM_TYPE.Tick, E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected);
    }

    protected override void OnTriggerLogic(Collider entity)
    {
        if (entity.TryGetComponent(out Sophia.Entitys.Player player))
        {
            Debug.Log("Triggerd");
            this.OwnerRef = player;
            Extras<Sophia.Entitys.Entity> extrasRef = OwnerRef.GetExtras<Sophia.Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected);
            extrasRef.AddModifier(this.extrasModifier);
            (extrasModifier.Functional as AffectConveyerCommand).SetOwner(player);
            Debug.Log($"아이템 이름 : {GetName()} 효과 : {GetDescription()}");
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("entity는 Player가 아니거나 못찾음");
        }
    }

    public string GetName()
    {
        return _equipmentName;
    }

    public string GetDescription()
    {
        string res = $"{_description}\n {affectCommand.GetName()} : {affectCommand.GetDescription()}";
        return res;
    }

    public Sprite GetSprite()
    {
        return _icon;
    }
}
