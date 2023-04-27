/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TEST_ActivateAttribute : MonoBehaviour
{
    public GameObject entity;
    public TEST_Player player;

    [ColorUsage(true)]
    public List<Color> Colors = new List<Color>();
    Color mDefaultColor;
    private void Awake() {
        mDefaultColor = Colors[0];
    }

    
    [ExecuteInEditMode] 
	public void ReturnBurn(int _durationTime){
        if(!entity.GetComponent<TEST_AttributeData>().tempAttributeData.mDebuffState.ContainsKey(E_DebuffState.Burn)){
            entity.GetComponent<TEST_AttributeData>().tempAttributeData.mDebuffState.Add(E_DebuffState.Burn, false);
        }
        bool BurnState = entity.GetComponent<TEST_AttributeData>().tempAttributeData.mDebuffState[E_DebuffState.Burn];
        bool StartBurn = false;
        IEnumerator thisDurate = DurateCoroutine();
        IEnumerator thisCo = Burncoroutine();
        
        IEnumerator DurateCoroutine(){
            if(BurnState == true){yield break;}
            Debug.Log("Burn Start");
            entity.GetComponent<TEST_AttributeData>().tempAttributeData.mDebuffState[E_DebuffState.Burn] = true;
            StartBurn = true;
            float curTime = 0;
            while(_durationTime > curTime) {
                curTime += Time.deltaTime;
                yield return null;
            }
            entity.GetComponent<TEST_AttributeData>().tempAttributeData.mDebuffState[E_DebuffState.Burn] = false;
        }

        IEnumerator Burncoroutine(){
            if(StartBurn == true){yield break;}
            StartBurn = true;
            player.material.color = Colors[1];
            float curTime = 0;
            while(_durationTime > curTime){
                curTime += 0.5f;
                player.numericData.CurHP -= 10;
                int currentHp = player.numericData.CurHP;
                yield return YieldInstructionCache.WaitForSeconds(0.5f);
            }
            StartBurn = false;
            player.material.color = mDefaultColor;
        }
        
        StartCoroutine(thisCo);
        StartCoroutine(thisDurate);
    }


    [ExecuteInEditMode] 
	public void ReturnPoisend(){

        Debug.Log("ReturnPoisend");
    }
    [ExecuteInEditMode] 
	public void ReturnBleed(){

        Debug.Log("ReturnBleed");
    }
    [ExecuteInEditMode] 
	public void ReturnContracted(){

        Debug.Log("ReturnContracted");
    }
    [ExecuteInEditMode] 
	
    public void ReturnSlow (int _durationTime){
        if(!entity.GetComponent<TEST_AttributeData>().tempAttributeData.mDebuffState.ContainsKey(E_DebuffState.Slow)){
            entity.GetComponent<TEST_AttributeData>().tempAttributeData.mDebuffState.Add(E_DebuffState.Slow, false);
        }
        bool BurnState = entity.GetComponent<TEST_AttributeData>().tempAttributeData.mDebuffState[E_DebuffState.Slow];
        IEnumerator thisCo = Burncoroutine();
        
        IEnumerator Burncoroutine(){
            if(BurnState == true){yield break;}
            Debug.Log("Burn Start");
            entity.GetComponent<TEST_AttributeData>().tempAttributeData.mDebuffState[E_DebuffState.Slow] = true;
            player.material.color = Colors[1];
            float curTime = 0;
            while(_durationTime > curTime){
                curTime += Time.deltaTime;
                yield return null;
            }
            player.material.color = mDefaultColor;
            entity.GetComponent<TEST_AttributeData>().tempAttributeData.mDebuffState[E_DebuffState.Slow] = false;
        }
        StartCoroutine(thisCo);
        Debug.Log("ReturnSlow");
    }

    [ExecuteInEditMode] 
	public void ReturnConfused (){

        Debug.Log("ReturnConfused");
    }
    [ExecuteInEditMode] 
	public void ReturnFearing (){

        Debug.Log("ReturnFearing");
    }
    [ExecuteInEditMode] 
	public void ReturnStern (){

        Debug.Log("ReturnStern");
    }
    [ExecuteInEditMode] 
	public void ReturnBounded(){

        Debug.Log("ReturnBounded");
    }
}

*/