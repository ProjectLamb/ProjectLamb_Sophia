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

public class AsyncAffectorStructure {
    public int affectCount = 0;
    public int variationSize = 0;
    private List<IEnumerator>        curCoroutune;
    private List<List<IEnumerator>>  affectorVariations;

    public AsyncAffectorStructure() {
        curCoroutune        = new List<IEnumerator>();
        affectorVariations  = new List<List<IEnumerator>>();
    }

    public void PushAffctorVariation(List<IEnumerator> input){
        variationSize++;
        affectorVariations.Add(input);
    }

    public void PopAffectorVariation(){
        variationSize--;
        affectorVariations.RemoveAt(affectorVariations.Count);
    }

    public List<IEnumerator> GetNextCoroutune() {
        //조건 작성
        return affectorVariations[0];
    }
}