using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AlignChildObjects : MonoBehaviour
{
    [SerializeField] Vector2Int Table;
    [SerializeField] int Gap;
    [ContextMenu("정렬하기")]
    public void Align() {
        Queue<Transform> chillds = new Queue<Transform>();
        foreach(Transform c in transform) {chillds.Enqueue(c);}
        for(int i = 0; i < Table.x; i++) {
            for(int j = 0; j < Table.y; j++){
                if(chillds.Count != 0) 
                    chillds.Dequeue().SetPositionAndRotation(new Vector3Int(Gap * i, 0, Gap * j), Quaternion.identity);
            }
        }
    }
}