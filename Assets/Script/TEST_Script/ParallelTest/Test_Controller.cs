using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Test_Controller : MonoBehaviour
{
    public bool Triggers = true;
    public int InvokeCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        var clickStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));

        clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(300)))
          .Where(x => x.Count >= 2)             //(마우스클릭) 이벤트가 2회 이상 발생한 경우만 필터링
          .Subscribe(_ =>Debug.Log("더블클릭"));
    }
}
