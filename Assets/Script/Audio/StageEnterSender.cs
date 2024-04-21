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
                if(currentStage.GetComponent<Stage>().Type == "shop")
                    _shopEnterSender.SendCommand();
            })
        );
    }

    [ContextMenu("상점 입장")]
    public void EnterShop() {
        _shopEnterSender.SendCommand();
    }
}
