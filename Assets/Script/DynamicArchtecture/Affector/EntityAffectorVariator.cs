using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 기존 List<IEnumerator>()과 달라진점은 오직.
// 조건에 따른 다른 IEnumerator을 제출한다는점이다.
// count에 따른 List<IEnumerator>을 다르게 뱉는다는것이다.
// 그리고 그 조건을 적는것은.... 일단, 새로운 버프를 만드는것으로 해보자.
// 즉 State의 패키지가 되는것.

[CreateAssetMenu(fileName = "EntityAffectorVariator", menuName = "ScriptableObject/EntityAffectorVariator", order = int.MaxValue)]
public class EntityAffectorVariator : ScriptableObject, IModifier {
    private int mAffectCount = 0;
    protected int affectCount {
        get {return mAffectCount;}
        set {
            if(mAffectCount == 0){this.isAlreadyReseted = false;}
            else {this.isAlreadyReseted = true;}
            mAffectCount = value;
        }
    }
    public EntityAffector[]  AsyncAffectorVariations;
    protected Entity targetEntity;
    protected Entity ownerEntity;
    protected bool  isAlreadyReseted = false;

        /// <summary>
    /// 장비나 버프에서 사용하게 될 녀석
    /// </summary>
    /// <param name="_owner"></param>
    /// <param name="_target"></param>
    public void Init(Entity _owner, Entity _target){
        if(this.isAlreadyReseted) return;
        for(int i = 0; i < AsyncAffectorVariations.Length ; i++){
            AsyncAffectorVariations[i].Init(_owner, _target);
        }
    }
    
    /// <summary>
    /// 장비나 버프에서 사용될 녀석
    /// </summary>
    /// <param name="affectableEntity"></param>
    public void Modifiy(IAffectable affectableEntity){
        if(!this.isAlreadyReseted) {throw new System.Exception("Affector 초기화 안됨 초기화 하고 사용해야함");}
        AsyncAffectorVariations[affectCount++].Modifiy(affectableEntity);
    }
}