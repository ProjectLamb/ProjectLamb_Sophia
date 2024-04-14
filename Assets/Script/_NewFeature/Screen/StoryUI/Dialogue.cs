using UnityEngine;

[System.Serializable] // 구조체가 인스펙터 창에 보이게 하기 위한 작업
public struct TalkData
{
    public string name; // 대사 치는 캐릭터 이름
    public string[] contexts; // 대사 내용
}

[System.Serializable]
public class DebugTalkData
{
    public string eventName;
    public TalkData[] talkDatas;
    
    public DebugTalkData(string name, TalkData[] td)
    {
        eventName = name;
        talkDatas = td;
    }
}

public class Dialogue : MonoBehaviour
{
    // 대화 이벤트 이름
    [SerializeField] string eventName;
    // 위에서 선언한 TalkData 배열 
    [SerializeField] TalkData[] talkDatas;

    public TalkData[] GetObjectDialogue()
    {
        return DialogueParse.GetDialogue(eventName);
    }
}