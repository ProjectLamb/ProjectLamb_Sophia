using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;

public class Test_CameraController : MonoBehaviour
{
    //0번 일반카메라 1번 타겟카메라
    public GameObject[] cineCamera; 
    public CinemachineTargetGroup targetGroup; 

    Stage stage;

    void Start()
    {
        //기본 카메라
        SwitchCamera(0);
        //카메라 타겟 그룹에 플레이어 추가
        targetGroup.AddMember(GameManager.Instance.PlayerGameObject.transform, 5f, 5f);
        //스테이지 변경될 때 발생하는 이벤트에 함수추가       
        GameManager.Instance.GlobalEvent.OnStageEnter.Add(new UnityAction<Stage, Stage>(StartDampingCoroutine));
        GameManager.Instance.GlobalEvent.OnStageEnter.Add(new UnityAction<Stage, Stage>(CheckStage));   
    }

    public void CheckStage(Stage departStage, Stage arriveStage)
    {
        //보스방이면 카메라 변경
        if (arriveStage.Type == Stage.STAGE_TYPE.BOSS) 
        {
            //타겟 카메라
             SwitchCamera(1);  
        }

        StartCoroutine(SetDamping(departStage, arriveStage, arriveStage.Type == Stage.STAGE_TYPE.BOSS ? 1 : 0));
    }


    public void SwitchCamera(int index)
    {
        for (int i = 0; i < cineCamera.Length; i++)
        {
            cineCamera[i].SetActive(i == index);
        }
    }

    public void StartDampingCoroutine(Stage departStage, Stage arriveStage)
    {
        StartCoroutine(SetDamping(departStage, arriveStage));
    }

    public IEnumerator SetDamping(Stage departStage, Stage arriveStage, float time=0)
    {
        yield return new WaitForSeconds(time);
        cineCamera[0].GetComponent<CinemachineFollow>().TrackerSettings.PositionDamping = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        cineCamera[0].GetComponent<CinemachineFollow>().TrackerSettings.PositionDamping = new Vector3(2f, 2f, 2f);
    }
}
