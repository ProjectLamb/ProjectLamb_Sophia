using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dissolve : MonoBehaviour
{
    GameObject dissolvePanel;               //판넬오브젝트
    RawImage rwImage;                            //판넬 이미지
    private bool IsDissolveOver = false;     //투명도 조절 논리형 변수

    void Awake()
    {
        dissolvePanel = this.gameObject;                         //스크립트 참조된 오브젝트
        rwImage = dissolvePanel.GetComponent<RawImage>();    //판넬오브젝트에 이미지 참조
    }
    void Update()
    {
        StartCoroutine("DissolveEffect");                        //코루틴    //판넬 투명도 조절
        if (IsDissolveOver)                                            //만약 IsDissolveOver 이 참이면
        {
            Destroy(this.gameObject);                        //판넬 파괴, 삭제
        }
    }
    IEnumerator DissolveEffect()
    {
        Color color = rwImage.color;                            //color 에 판넬 이미지 참조
        color.a -= Time.deltaTime * 0.075f;               //이미지 알파 값을 타임 델타 값 * 0.01
        rwImage.color = color;    
                                    //판넬 이미지 컬러에 바뀐 알파값 참조
        if (rwImage.color.a <= 0)                        //만약 판넬 이미지 알파 값이 0보다 작으면
        {
            IsDissolveOver = true;                              //IsDissolveOver 참 
        }
        yield return null;                                        //코루틴 종료

    }
}
