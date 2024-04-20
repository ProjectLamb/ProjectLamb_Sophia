using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyGameManager : MonoBehaviour
{
    private static DontDestroyGameManager _instance;
    public static DontDestroyGameManager Instance
    {
        get {
            if(_instance == null)
            {
                _instance = FindFirstObjectByType(typeof(DontDestroyGameManager)) as DontDestroyGameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private GlobalAudioManager _audioManager;

    public GlobalAudioManager AudioManager {get { return _audioManager; } }
}