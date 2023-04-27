using System.Diagnostics;
using System.Collections;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TEST_PrinterInputAmount : MonoBehaviour
{
    Stopwatch sw = new Stopwatch();
    private void Awake() {
        TEST_UnityEvent.inputScriptEvents.AddListener(printAmount);
    }
    public void printAmount(int _amount, float _time, Vector3 _position){
        StartCoroutine(CoPrint(_amount, _time, _position));
    }
    IEnumerator CoPrint(int _amount, float _time, Vector3 _position){
        sw.Start();
        Debug.Log("sw : " + sw.ElapsedMilliseconds.ToString()+"ms");

        yield return new WaitForSeconds(_time);

        Debug.Log($"Input Amount : {_amount}\n Input Position {_position}");
        transform.position = _position;
        
        Debug.Log("sw : " + sw.ElapsedMilliseconds.ToString()+"ms");
        sw.Stop();
    }
}
