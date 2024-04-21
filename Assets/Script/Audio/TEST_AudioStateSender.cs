
using UnityEngine;
using FMODPlus;
using UnityEngine.Events;

public enum STAGE_TYPE {None, Normal, Shop, Hidden, Boss}
public class TEST_AudioStateSender : MonoBehaviour {
    
    [SerializeField] public AudioStateSender _audioStateSender;
    [SerializeField] public STAGE_TYPE _departType;
    [SerializeField] public STAGE_TYPE _arriveType;

    [ContextMenu("타입에 따른 오디오 샌딩")]
    public void TEST_EnterStage() {
        _audioStateSender.EnterStageByType(_departType.ToString(), _arriveType.ToString());
    }
    
    [ContextMenu("오디오 센딩")]
    public void TEST_BattleEnd() {
        _audioStateSender.BattleEndByVoid();
    }
}
