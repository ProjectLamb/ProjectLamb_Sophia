using UnityEngine;
using System.Collections.Generic;

namespace Sophia.Entitys
{
    using Cysharp.Threading.Tasks;
    using Sophia.Composite;
    using Sophia.Instantiates;

    public enum E_ROBUWA_ATTACK_STATE {
        None = 0, NoramlAttack
    }

    public abstract class Enemy : Entity {

#region SerializeMember
    
    [SerializeField] private SerialBaseEntityData       _baseEntityData;
    [SerializeField] private AffectorManager            _affectorManager;
    [SerializeField] private ProjectileBucketManager    _projectileBucketManager;
    [SerializeField] private ProjectileObject[]         _attckProjectiles;
    [SerializeField] private VisualFXObject             _spawnParticleRef;
    [SerializeField] private VisualFXObject             _dieParticleRef;
    [SerializeField] public  Entity                     _objectiveEntity;
    
#endregion

#region Member

    public LifeComposite Life {get; private set;}
    /*
    1. isRecog 직접 커스텀 해서 true false;
    2. field of view
    */

#endregion

    }
}