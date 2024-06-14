using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;

public class Test_CameraController : MonoBehaviour
{
    //0�� �Ϲ�ī�޶� 1�� Ÿ��ī�޶�
    public GameObject[] cineCamera; 
    public CinemachineTargetGroup targetGroup; 

    Stage stage;

    void Start()
    {
        //�⺻ ī�޶�
        SwitchCamera(0);
        //ī�޶� Ÿ�� �׷쿡 �÷��̾� �߰�
        targetGroup.AddMember(GameManager.Instance.PlayerGameObject.transform, 5f, 5f);
        //�������� ����� �� �߻��ϴ� �̺�Ʈ�� �Լ��߰�       
        GameManager.Instance.GlobalEvent.OnStageEnter.Add(new UnityAction<Stage, Stage>(StartDampingCoroutine));
        //GameManager.Instance.GlobalEvent.OnStageEnter.Add(new UnityAction<Stage, Stage>(CheckStage));   
    }

    public void CheckStage(Stage departStage, Stage arriveStage)
    {
        //�������̸� ī�޶� ����
        if (arriveStage.Type == Stage.STAGE_TYPE.BOSS) 
        {
            //Ÿ�� ī�޶�
             SwitchCamera(1);  
        }

        StartCoroutine(SetDamping(departStage, arriveStage, arriveStage.Type == Stage.STAGE_TYPE.BOSS ? 2 : 0));
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

    public IEnumerator SetDamping(Stage departStage, Stage arriveStage, float time = 0)
    {
        yield return new WaitForSeconds(time);
        cineCamera[0].GetComponent<CinemachineFollow>().TrackerSettings.PositionDamping = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        cineCamera[0].GetComponent<CinemachineFollow>().TrackerSettings.PositionDamping = new Vector3(2f, 2f, 2f);
    }
}
