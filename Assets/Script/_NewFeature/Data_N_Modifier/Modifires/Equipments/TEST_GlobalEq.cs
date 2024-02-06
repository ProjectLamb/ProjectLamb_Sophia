using System;
using UnityEngine;

using Sophia;
using Sophia.DataSystem;
using Sophia.Instantiates;
using Sophia.DataSystem.Functional;
using Sophia.DataSystem.Modifiers;

public class TEST_GlobalEq : Carrier, IUserInterfaceAccessible
{ //, IPlayerDataApplicant{
    [SerializeField] public string _equipmentName;
    [SerializeField] public Sprite _icon;
    [SerializeField] public string _description;

    [SerializeField] public SerialAffectorData _serialAffectorData;
    ExtrasModifier<object> extrasModifier;
    IFunctionalCommand<Sophia.Entitys.Entity> affectCommand;
    private void InitializeExtrasModifiers(Sophia.Entitys.Entity owner)
    {
        extrasModifier = new ExtrasModifier<object>(
            CommandConverter.Convert(new MoveSpeedUpCommand(owner, _serialAffectorData), owner),
            E_EXTRAS_PERFORM_TYPE.Start,
            E_FUNCTIONAL_EXTRAS_TYPE.EnemyDie
        );
    }
    public Sophia.Entitys.Player PlayerRef;
    private void Awake() {
        PlayerRef = GameManager.Instance.PlayerGameObject.GetComponent<Sophia.Entitys.Player>();
    }

    protected override void OnTriggerLogic(Collider entity)
    {
        if (entity.TryGetComponent(out Sophia.Entitys.Player player))
        {
            IFunctionalCommand<Sophia.Entitys.Entity> command =  new MoveSpeedUpCommand(player, _serialAffectorData);
            Sophia.Entitys.Entity entity1 = player;
            command.Invoke(ref entity1);

            InitializeExtrasModifiers(player);

            Extras<object> extrasRef = player.GetExtras<object>(E_FUNCTIONAL_EXTRAS_TYPE.EnemyDie);
            extrasRef.AddModifier(this.extrasModifier);
            extrasRef.RecalculateExtras();
            
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("entity는 Player가 아니거나 못찾음");
        }
    }

    public string GetDescription()
    {
        throw new NotImplementedException();
    }

    public string GetName()
    {
        throw new NotImplementedException();
    }

    public Sprite GetSprite()
    {
        throw new NotImplementedException();
    }
}