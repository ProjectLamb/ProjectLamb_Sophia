using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Sophia.Entitys
{
    using Sophia.Composite;
    using Sophia.Composite.RenderModels;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Referer;
    using Sophia.Instantiates;

    public enum E_MOB_AI_DIFFICULTY {
        None = 0, Offensive, Defensive
    }

    public abstract class Enemy : Entity, IRecogStateAccessible {

#region SerializeMember
        
        [SerializeField] protected SerialBaseEntityData       _baseEntityData;
        [SerializeField] protected SerialFieldOfViewData      _fOVData;
        [SerializeField] protected AffectorManager            _affectorManager;
        [SerializeField] protected ProjectileBucketManager    _projectileBucketManager;
        [SerializeField] protected ProjectileObject[]         _attckProjectiles;
        [SerializeField] protected VisualFXObject             _spawnParticleRef;
        [SerializeField] protected VisualFXObject             _dieParticleRef;
        [SerializeField] public  Entity                     _objectiveEntity;
        [SerializeField] protected E_MOB_AI_DIFFICULTY        _mobDifficulty;

#endregion

        public abstract RecognizeEntityComposite GetRecognizeComposite();

    }
}