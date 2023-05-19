using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MA_TextAnime : MonoBehaviour {
    public UnityAction onDestroyAction;
    Text text;
    public MA_TextData textData {
        get {
            return textData;
        }
        set {
            if(text == null) {TryGetComponent<Text>(out text);}
            text.color = value.color;
            text.text = value.innerStrs;
        }
    }

    private void Init() {
    }
    private void Start() {
        StartCoroutine(CoCounter());
    }
    private void FixedUpdate() {
        transform.position += (Vector3.up * 1f) * Time.deltaTime;
    }
    IEnumerator CoCounter(){
        yield return new WaitForSeconds(0.5f);
        onDestroyAction?.Invoke();
    }
}