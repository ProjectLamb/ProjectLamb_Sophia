using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public interface IAffectable {
    public void AsyncAffectHandler(List<IEnumerator> _Coroutine);
    public void AffectHandler(List<UnityAction> _Action);
}
