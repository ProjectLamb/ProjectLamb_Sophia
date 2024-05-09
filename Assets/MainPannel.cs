using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPannel : MonoBehaviour
{
    [SerializeField] private GameObject NewGameElement;
    [SerializeField] private GameObject ContinueGameElement;
    [SerializeField] private GameObject SettingElement;
    [SerializeField] private GameObject ExitElement;

    private void Awake() {
            
    }

    private void OnEnable() {
        if(!DontDestroyGameManager.Instance.SaveLoadManager.GetIsDataExist()) {
            // 컨티뉴 버튼 비활성화
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
        DontDestroyGameManager.Instance.SaveLoadManager.LoadFromJson();
    }
}
