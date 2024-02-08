using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using TMPro;

namespace Sophia.Composite
{
    using Sophia.Entitys;
    using Sophia.DataSystem;

    public class AffectorManager : MonoBehaviour
    {
        class GarbageAffectorCleaner
        {
            public readonly SortedSet<E_AFFECT_TYPE> GarbageAffector = new();
            Dictionary<E_AFFECT_TYPE, DataSystem.Modifiers.Affector> AffectorStacksRef;
            public GarbageAffectorCleaner(Dictionary<E_AFFECT_TYPE, DataSystem.Modifiers.Affector> affectorStacks) => AffectorStacksRef = affectorStacks;

            public void ClearGarbageAffector()
            {
                if (GarbageAffector.Count == 0) { return; }
                foreach (var E in GarbageAffector) { AffectorStacksRef[E] = default; }
                GarbageAffector.Clear();
            }
        }
        #region SerializeMember

        [SerializeField] private Entity _entity;
        [SerializeField] private List<string> _currentAffectors;

        #endregion
        public IDataAccessible DataAccessible { get; private set; }
        public Dictionary<E_AFFECT_TYPE, DataSystem.Modifiers.Affector> AffectingStacks { get; private set; }
        public Stat Tenacity { get; private set; }
        GarbageAffectorCleaner affectorCleaner;

        public void Init(float baseTenacity)
        {
            Tenacity = new Stat(baseTenacity,
                E_NUMERIC_STAT_TYPE.Tenacity,
                E_STAT_USE_TYPE.Ratio,
                OnTenacityUpdated
            );
        }

        private void OnTenacityUpdated()
        {
            Debug.Log("TenacityUpdated");
        }

        private void Awake()
        {
            AffectingStacks = new Dictionary<E_AFFECT_TYPE, DataSystem.Modifiers.Affector>();
            DataAccessible = _entity;
            foreach (E_AFFECT_TYPE affecType in Enum.GetValues(typeof(E_AFFECT_TYPE))) { AffectingStacks.Add(affecType, default); }
            affectorCleaner = new GarbageAffectorCleaner(AffectingStacks);
        }

        public void Affect(DataSystem.Modifiers.Affector affector)
        {
            E_AFFECT_TYPE newAffectorType = affector.AffectType;
            if (!AffectingStacks.ContainsKey(newAffectorType)) throw new System.Exception("현재 받아온 어펙터는 타입이 존재하지 않음");
            if (AffectingStacks.TryGetValue(newAffectorType, out DataSystem.Modifiers.Affector affectingAffector))
            {
                if (affectingAffector != default)
                {
                    affectingAffector.Revert(_entity);
                    AffectingStacks[affectingAffector.AffectType] = default;
                }
            }
            AffectingStacks[newAffectorType] = affector;
            AffectingStacks[newAffectorType].OnRevert += () => affectorCleaner.GarbageAffector.Add(newAffectorType);
            AffectingStacks[newAffectorType].Invoke(_entity);

            _currentAffectors.Clear();
            foreach (var item in AffectingStacks)
            {
                _currentAffectors.Add(item.Key.ToString());
            }
        }

        public void Recover(DataSystem.Modifiers.Affector affector)
        {
            AffectingStacks[affector.AffectType].Revert(_entity);

            _currentAffectors.Clear();
            foreach (var item in AffectingStacks)
            {
                _currentAffectors.Add(item.Key.ToString());
            }
        }

        private void Update()
        {
            foreach (KeyValuePair<E_AFFECT_TYPE, DataSystem.Modifiers.Affector> affectingAffector in AffectingStacks)
            {
                if (affectingAffector.Equals(default)) continue;
                affectingAffector.Value?.Run(_entity);
            }
        }
    }
}


/*
private AffectorStackRemoveCommend affectorStackRemoveCommend;

public AffectorManager(float baseTenacity)
{
    Tenacity = new Stat(baseTenacity,
        E_NUMERIC_STAT_TYPE.Tenacity,
        E_STAT_USE_TYPE.Ratio,
        OnTenacityUpdated
    );
    foreach (E_AFFECT_TYPE E in Enum.GetValues(typeof(E_AFFECT_TYPE))) { AffectorStacks.Add(E, default); }
    affectorStackRemoveCommend = new AffectorStackRemoveCommend(AffectorStacks);

    AddToUpator();
}


public void ModifiyByAffector(Affector affector)
{
    E_AFFECT_TYPE stateType = affector.AffectType;
    if (!AffectorStacks.ContainsKey(stateType))
    {
        throw new System.Exception("현재 받아온 어펙터는 타입이 존재하지 않음");
    }
    if (AffectorStacks.TryGetValue(stateType, out Affector runnginAffector))
    {
        if (runnginAffector != null)
        {
            runnginAffector.CancleModify();
            AffectorStacks[stateType] = default;
        }
    }
    AffectorStacks[stateType] = affector;
    AffectorStacks[stateType].SetAccelarationByTenacity(this.Tenacity);
    AffectorStacks[stateType].OnRevert += () => affectorStackRemoveCommend.removeCommands.Add(stateType);
    AffectorStacks[stateType].Modifiy(Tenacity);
}

#region  Updator Implements
bool IsUpdatorBinded = false;
public bool GetUpdatorBind() => IsUpdatorBinded;

public void AddToUpator()
{
    GlobalTimeUpdator.CheckAndAdd(this);
    IsUpdatorBinded = true;
}

public void RemoveFromUpdator()
{
    GlobalTimeUpdator.CheckAndRemove(this);
    IsUpdatorBinded = false;
}

public void LateTick()
{
    return;
}

public void FrameTick()
{
    foreach (var affector in AffectorStacks)
    {
        // if(affector.Equals(default)) continue;
        affector.Value?.TickRunning();
    }
    affectorStackRemoveCommend.RemoveAffectorStack();
}

public void PhysicsTick()
{
    return;
}
#endregion
*/