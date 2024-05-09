using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTitle : MonoBehaviour
{
    public GameObject NewGameButton;
    [SerializeField] private bool IsDebugMode;
    [SerializeField] private string originNewGameScene;
    [SerializeField] private string debugNewGameScene;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsDebugMode)
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                NewGameButton.GetComponent<UIButton>().OnClickEvent.RemoveListener(() => { GlobalSceneLoader.LoadScene(originNewGameScene); });
                NewGameButton.GetComponent<UIButton>().OnClickEvent.AddListener(() => { GlobalSceneLoader.LoadScene(debugNewGameScene); });
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (IsDebugMode)
                IsDebugMode = false;
            else
                IsDebugMode = true;
        }
    }
}
