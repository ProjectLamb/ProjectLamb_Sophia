using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//그니깐 이걸 스크립터블화 할 수 있겠느냐 그것이다.
public class RedAttackCmd : I_MA_Command {

    private readonly MA_GameController owner;
    MA_TextAnime gameObjectText;
    public UnityAction OnFinished {get;set;}

    public RedAttackCmd(MA_GameController _owner){
        owner = _owner;
    }

    public void Execute(){
        owner.prefebs.textData = owner.TD[(int)TextDataIndex.red];
        gameObjectText = GameObject.Instantiate(owner.prefebs, owner.transform);
        gameObjectText.onDestroyAction += AfterExcute;
    }
    
    private void AfterExcute(){
        gameObjectText.onDestroyAction -= AfterExcute;
        GameObject.Destroy(gameObjectText.gameObject);
        OnFinished?.Invoke();
    }
}