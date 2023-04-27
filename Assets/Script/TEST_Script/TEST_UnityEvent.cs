using UnityEngine;
using UnityEngine.Events;

public class TEST_UnityEvent : MonoBehaviour
{
    static internal UnityEvent<int, float, Vector3> inputScriptEvents = new UnityEvent<int, float, Vector3>();
    [ContextMenu("Invoke scriptEvent")]
    void InvokeScriptEvent(){
        inputScriptEvents.Invoke(1004, 3f, new Vector3(6, 7 ,2));
    }
}