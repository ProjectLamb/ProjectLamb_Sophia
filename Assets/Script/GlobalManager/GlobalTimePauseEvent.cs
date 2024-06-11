    // [Range(0, 1)]
    // public float GameTimeScale = 1f;
    // public UnityEvent<string> OnPlayEvent = new UnityEvent<string>();
    // public UnityEvent<string> OnPausedEvent = new UnityEvent<string>();


    // [SerializedDictionary("PauseCauseKey", "Flags")]
    // private SerializedDictionary<string, bool> _IsGamePaused =  new SerializedDictionary<string, bool>();
    
    // public const float PAUSE_SCALE = 0;

    // public bool IsGamePaused {
    //     get {return _IsGamePaused.All(x => x.Value == true); }
    // }

    // public void SetTimeStateByHandlersString(string handler, bool timeState) {
    //     if(!_IsGamePaused.ContainsKey(handler)) {
    //         _IsGamePaused.TryAdd(handler, timeState); return;
    //     }
    //     _IsGamePaused[handler] = timeState;
    // }

    // public void Pause(string handler) {
    //     bool PrevTimeState = IsGamePaused;
    //     SetTimeStateByHandlersString(handler, false);
    //     if(PrevTimeState == false && IsGamePaused == true) {
    //         OnPausedEvent?.Invoke(gameObject.name);
    //         GameTimeScale = PAUSE_SCALE;
    //     }
    // }
    // public void Play(string handler) {
    //     bool PrevTimeState = IsGamePaused;
    //     SetTimeStateByHandlersString(handler, true);
    //     if(PrevTimeState == true && IsGamePaused == false) {
    //         OnPlayEvent?.Invoke(gameObject.name);
    //         GameTimeScale = mCurrentTimeScale; 
    //     }
    // }
    