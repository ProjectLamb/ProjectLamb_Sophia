using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

public abstract class EntityAction {
    int order = 0;
    //Dictionary<Component, Component> OwnerComponentDic = new Dictionary<Component, Component>(); 
    //Dictionary<Component, Component> TargetComponentDic = new Dictionary<Component, Component>(); 
    public EntityAction(ref GameObject _owner, ref GameObject _target, object[] _parameters){

    }
    public virtual void ToAffect(ref GameObject _self, Object _parameters){}
    public virtual void ToAffect(ref GameObject _owner, ref GameObject _target, Object _parameters){}
}