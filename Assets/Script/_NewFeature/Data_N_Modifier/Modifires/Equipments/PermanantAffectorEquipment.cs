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

    [SerializeField] public SerialAffectorData _serialAffectorData;

    ExtrasModifier<Sophia.Entitys.Entity> extrasModifier;
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
    }

    private void InitializeExtrasModifiers(Sophia.Entitys.Entity owner) {

        extrasModifier = new ExtrasModifier<Sophia.Entitys.Entity>(
            AffectCommandFactory.GetAffect(owner, _serialAffectorData),
            E_EXTRAS_PERFORM_TYPE.Tick, 
            E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected
        );

    }

    protected override void OnTriggerLogic(Collider entity)
    {
        if (entity.TryGetComponent(out Sophia.Entitys.Player player))
        {
            InitializeExtrasModifiers(player);
            Extras<Sophia.Entitys.Entity> extrasRef = player.GetExtras<Sophia.Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected);
            extrasRef.AddModifier(this.extrasModifier);
            extrasRef.RecalculateExtras();
            // Debug.Log("Triggerd");
            // Debug.Log($"아이템 이름 : {GetName()} 효과 : {GetDescription()}");
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
