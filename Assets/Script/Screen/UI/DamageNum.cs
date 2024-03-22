using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNum : MonoBehaviour
{
    public Rigidbody rigid;
    public Text text;

    private void Awake() {
        TryGetComponent<Rigidbody>(out rigid);
    }
    private void Start() {
        rigid.velocity = Vector3.up * 5;
        Destroy(gameObject, 1f);
    }

    public void Initialize(int amount){
        text.text = amount.ToString();
    }
}
