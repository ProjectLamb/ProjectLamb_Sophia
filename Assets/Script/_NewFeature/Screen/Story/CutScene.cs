namespace Sophia.CutScene {
    // Enter → Running         → Exit
    //         WaitingInput    ↗
    public enum E_CUTSCENE_STATE {
        None,
        Init, Enter, 
        Run, Wait,
        Exit, Skip, End
    }

    // VideoCutScene, CineCutscene, UICutScene
}