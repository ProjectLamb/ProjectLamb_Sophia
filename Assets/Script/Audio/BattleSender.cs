
using UnityEngine;
using FMODPlus;
using UnityEngine.Events;

public class BattleSender : MonoBehaviour {

    public CommandSender _battleStartSender;
    public CommandSender _battleEndSender;

    private void OnEnable() {
        GameManager.Instance.GlobalEvent.OnStageEnter.Add(
            new UnityAction<Stage,Stage>((departStage, arrvieStage) => {
                if(!arrvieStage.IsClear)
                    _battleStartSender.SendCommand();
                Debug.Log($"떠나온 방 {departStage.name} \n도착한 방 {arrvieStage.name}");
            })
        );

        GameManager.Instance.GlobalEvent.OnStageClear.Add(
            new UnityAction<Stage>((currentStage) => {
                _battleEndSender.SendCommand();
                Debug.Log($"클리어 방 {currentStage.name}");
            })
        );
    }
}