using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlueAttackCmd : I_MA_Command {

    public readonly MA_GameController owner;
    MA_TextAnime gameObjectText;
    public UnityAction OnFinished {get;set;}
    public BlueAttackCmd(MA_GameController _owner) {
        owner = _owner;
    }
    public void Execute(){
        owner.prefebs.textData = owner.TD[(int)TextDataIndex.blue];
        gameObjectText = GameObject.Instantiate(owner.prefebs, owner.transform);
        gameObjectText.onDestroyAction += AfterExcute;
    }
    
    private void AfterExcute(){
        gameObjectText.onDestroyAction -= AfterExcute;
        GameObject.Destroy(gameObjectText.gameObject);
        OnFinished?.Invoke();
    }
}