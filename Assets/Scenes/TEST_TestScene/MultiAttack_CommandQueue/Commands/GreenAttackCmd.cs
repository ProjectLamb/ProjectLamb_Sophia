using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GreenAttackCmd : I_MA_Command {
    private readonly MA_GameController owner;
    public UnityAction OnFinished {get;set;}
    
    MA_TextAnime gameObjectText;

    public GreenAttackCmd(MA_GameController _owner){
        owner = _owner;
    }
    public void Execute(){
        owner.prefebs.textData = owner.TD[(int)TextDataIndex.green];
        gameObjectText = GameObject.Instantiate(owner.prefebs, owner.transform);
        gameObjectText.onDestroyAction += AfterExcute;
    }
    private void AfterExcute(){
        gameObjectText.onDestroyAction -= AfterExcute;
        GameObject.Destroy(gameObjectText.gameObject);
        OnFinished?.Invoke();
    }
}