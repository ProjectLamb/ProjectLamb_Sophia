using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonDestroyObjectReferer : MonoBehaviour
{
    [SerializeField] private DontDestroyGameManager _dontDestroyGameManager;
    public DontDestroyGameManager DontDestroyGameManager {
        get {
            if(_dontDestroyGameManager == null) {
                _dontDestroyGameManager = FindFirstObjectByType<DontDestroyGameManager>();
            }
            return _dontDestroyGameManager;
        }
    }

    public void ActivateAudioScreenObject() {
        DontDestroyGameManager.AudioSetterScreenObject.SetActive(true);
    }
}
