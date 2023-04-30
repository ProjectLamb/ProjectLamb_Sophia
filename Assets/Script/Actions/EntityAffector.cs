using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

/*
public class AffectData<T> {
    public T Owner {get; set;} 
    public T Target {get; set;} 
    public object[] Params {get; set;} 
}

public class AsyncAffectData<T> : AffectData<T>{
    IEnumerator _coroutine;
}
public delegate void DelAsyncAffectSelf<T>(AsyncAffectData<T> _data);
public delegate void DelAffectSelf<T>(AffectData<T> _data);

interface IAsyncAffactableActions {
    public void AsyncToAffect(GameObject _self, object[] _parameters, ref IEnumerator _coroutine){}
    public void AsyncToAffect(GameObject _owner, ref GameObject _target, object[] _parameters, ref IEnumerator _coroutine){}
}

interface IAffactableActions {
    public void ToAffect(GameObject _self, object[] _parameters){}
    public void ToAffect(GameObject _owner, ref GameObject _target, object[] _parameters){}
}
*/

interface IAffectableEntity {
    public void AsyncAffectHandler(IEnumerator _Coroutine);
    public void AffectHandler(UnityAction _Action);
}
//모노비헤이비어를 상속받으면서 
// 어떤 이득을 얻을 수 있나?
//  패러미터의 전달과 코루틴의 직접 실행이 가능하다.
// 굳이 게임 오브젝트를 만들 필요도 없다.

public abstract class EntityAffector {
    protected int order = 0;
    protected GameObject Owner {get; set;} 
    protected GameObject Target {get; set;} 
    protected object[] Params {get; set;} 
    public IEnumerator AsyncAffectorCoroutine;
    public UnityAction Affector;
    public EntityAffector(GameObject _owner, GameObject _target, object[] _params) {
        Owner = _owner; Target = _target; Params = _params;
    }
    public virtual void Affect(){ }
}