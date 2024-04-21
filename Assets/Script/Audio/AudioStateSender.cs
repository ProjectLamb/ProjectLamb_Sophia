
using UnityEngine;
using FMODPlus;
using UnityEngine.Events;

public class AudioStateSender : MonoBehaviour {

    public CommandSender _battleStartSender;
    public CommandSender _battleEndSender;
    public CommandSender _chapterStateSender;
    public CommandSender _shopStateSender;
    public CommandSender _dataStateSender;
    public CommandSender _bossStateSender;

    private bool IsInitialized = false;

    public void InitByStage() {
        if(IsInitialized) throw new System.Exception("이미 스테이지 이동에 따른 이벤트 설정 됨");
        GameManager.Instance.GlobalEvent.OnStageEnter.Add( new UnityAction<Stage,Stage>(EnterStage) );
        GameManager.Instance.GlobalEvent.OnStageClear.Add( new UnityAction<Stage>(BattleEnd) );
        IsInitialized = true;
    }

    public void BattleStart(Stage departStage, Stage arriveStage) {
        if(!arriveStage.IsClear) _battleStartSender.SendCommand();
        Debug.Log($"떠나온 방 {departStage.name} \n도착한 방 {arriveStage.name}");
    } 
    public void BattleEnd(Stage clearedStage) {
        _battleEndSender.SendCommand();
        Debug.Log($"클리어 방 {clearedStage.name}");
    }

    public void EnterStage(Stage departStage, Stage arriveStage) {
        switch (arriveStage.Type) {
            case "normal"   : {
                if(!arriveStage.IsClear) {_battleStartSender.SendCommand(); break;}
                if(departStage.Type == "shop") {_chapterStateSender.SendCommand(); break;}
                break;
            }
            case "shop"     : {
                _shopStateSender.SendCommand();
                break;
            }
            case "hidden"   : {
                
                break;
            }
            case "boss"     : {
                _bossStateSender.SendCommand();
                // 이 커맨드 샌더를 멈춰야 하는지 아니면 플레이를 해도 되는지 좀 확인해야 할듯.
                break;
            }
        }
    }

    
    public void EnterStageByType(string departType, string arriveType) {
        switch (arriveType) {
            case "Normal"   : {
                if(departType == "Shop") {_chapterStateSender.SendCommand(); break;}
                _battleStartSender.SendCommand();
                break;
            }
            case "Shop"     : {
                _shopStateSender.SendCommand();
                break;
            }
            case "Hidden"   : {
                
                break;
            }
            case "Boss"     : {
                _bossStateSender.SendCommand();
                // 이 커맨드 샌더를 멈춰야 하는지 아니면 플레이를 해도 되는지 좀 확인해야 할듯.
                break;
            }
        }
    }

    public void BattleEndByVoid() {
        _battleEndSender.SendCommand();
    }

    [ContextMenu("데이터 폭주")]
    public void DataMadding() {
        _dataStateSender.SendCommand();
    }
}