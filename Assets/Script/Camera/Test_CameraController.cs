using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;
using DG.Tweening;

public class Test_CameraController : MonoBehaviour
{
    //0�� �Ϲ�ī�޶� 1�� Ÿ��ī�޶�
    public GameObject[] cineCamera; 
    public CinemachineTargetGroup targetGroup;
    public Vector3 OriginCameraDamping;
    private float baseFOV;

    Stage stage;

    void Start()
    {
        //�⺻ ī�޶�
        SwitchCamera(0);
        //ī�޶� Ÿ�� �׷쿡 �÷��̾� �߰�
        targetGroup.AddMember(GameManager.Instance.PlayerGameObject.transform, 5f, 5f);

        OriginCameraDamping = cineCamera[0].GetComponent<CinemachineFollow>().TrackerSettings.PositionDamping;
        //�������� ����� �� �߻��ϴ� �̺�Ʈ�� �Լ��߰�       
        GameManager.Instance.GlobalEvent.OnStageEnter.Add(new UnityAction<Stage, Stage>(StartDampingCoroutine));
        //GameManager.Instance.GlobalEvent.OnStageEnter.Add(new UnityAction<Stage, Stage>(CheckStage));

        baseFOV = cineCamera[0].GetComponent<CinemachineCamera>().Lens.FieldOfView;
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

    public void FastZoomIn(int basezoom, float duratetime)
    {
        cineCamera[0].GetComponent<CinemachineCamera>().Lens.FieldOfView = baseFOV - basezoom;
        DOTween.To(() => cineCamera[0].GetComponent<CinemachineCamera>().Lens.FieldOfView, x => cineCamera[0].GetComponent<CinemachineCamera>().Lens.FieldOfView = x, baseFOV, duratetime).SetEase(Ease.InQuad);
    }

    public IEnumerator SetDamping(Stage departStage, Stage arriveStage, float time = 0)
    {
        yield return new WaitForSeconds(time);
        cineCamera[0].GetComponent<CinemachineFollow>().TrackerSettings.PositionDamping = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        cineCamera[0].GetComponent<CinemachineFollow>().TrackerSettings.PositionDamping = OriginCameraDamping;
    }

    public void StartDashDamping()
    {
        //cineCamera[0].GetComponent<CinemachineFollow>().TrackerSettings.PositionDamping = Vector3.zero;
        DOTween.To(()=> cineCamera[0].GetComponent<CinemachineFollow>().TrackerSettings.PositionDamping, x=> cineCamera[0].GetComponent<CinemachineFollow>().TrackerSettings.PositionDamping = x, Vector3.zero, 0.1f).SetEase(Ease.OutQuad);
    }
}
