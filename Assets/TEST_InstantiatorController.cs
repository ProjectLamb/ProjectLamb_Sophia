using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TEST_InstantiatorController : MonoBehaviour
{
    [SerializeField] public GameObject _prefeb;
    [SerializeField] Vector3 offset;

    public void InstantiatePrefebByPosition(Vector3 vector) {
        Instantiate(_prefeb, vector + offset, Quaternion.identity);
    }
}