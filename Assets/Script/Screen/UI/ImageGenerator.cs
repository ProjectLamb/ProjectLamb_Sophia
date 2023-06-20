using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageGenerator : MonoBehaviour {
    public DamageNum damageNum;

    [ContextMenu("Generate")]
    public void GenerateImage(int _damageAmount){
        Instantiate(damageNum, transform).Initialize(_damageAmount);
    }
}