using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChanger : MonoBehaviour
{
    public string commanString;
    public string insertingObjectName;
    public GameObject insertingObject;
    public int[] numRange = new int[2];

    [ContextMenu("Inject Object")]
    public void InjectObject() {
        foreach (Transform child in transform)
        {
            for(int i = numRange[0]; i <= (numRange[1] + 1); i++){
                Debug.Log(commanString + "_" + i.ToString("D3"));
                if(child.name == (commanString + "_" + i.ToString("D3"))) {
                    numRange[0]++;
                    GameObject newItem = Instantiate(insertingObject);
                    newItem.name = insertingObjectName;
                    newItem.transform.SetParent(child);
                    continue;
                }
            }

        }
    }
}
