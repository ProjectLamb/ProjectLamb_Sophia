using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODPlus;
using UnityEngine.Events;

public class StageEnterSender : MonoBehaviour
{
    public CommandSender _shopEnterSender;
    private void OnEnable() {
        GameManager.Instance.GlobalEvent.OnStageClear.Add(
            new UnityAction<Stage>((currentStage) => {
                if(currentStage.GetComponent<Stage>().Type == Stage.STAGE_TYPE.SHOP)
                    _shopEnterSender.SendCommand();
            })
        );
    }

    [ContextMenu("상점 입장")]
    public void EnterShop() {
        _shopEnterSender.SendCommand();
    }
}
