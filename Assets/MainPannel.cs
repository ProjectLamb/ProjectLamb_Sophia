using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPannel : MonoBehaviour
{
    [SerializeField] private GameObject NewGameElement;
    [SerializeField] private GameObject ContinueGameElement;
    [SerializeField] private GameObject SettingElement;
    [SerializeField] private GameObject ExitElement;

    private void OnEnable() {
        StartCoroutine(StartAtNextFrame());
    }

    IEnumerator StartAtNextFrame() {
        yield return new WaitForEndOfFrame();
        
        // Continue 버튼 활성화 조건
        if(!DontDestroyGameManager.Instance.SaveLoadManager.GetIsDataExist() || DontDestroyGameManager.Instance.SaveLoadManager.Data.IsTutorial) {
            ContinueGameElement.SetActive(false);
        }
        else {
            // 활성화
            ContinueGameElement.SetActive(true);
        }
    }

    public void NewGame() {
        DontDestroyGameManager.Instance.SaveLoadManager.ResetData();
    }

    public void Continue() {
        // 
    }
}
