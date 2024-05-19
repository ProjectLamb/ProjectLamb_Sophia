using UnityEngine;

[System.Serializable] // 구조체가 인스펙터 창에 보이게 하기 위한 작업
public struct TalkData
{
    public string name; // 대사 치는 캐릭터 이름
    public string[] contexts; // 대사 내용
    public string emotionState; // 캐릭터 감정상태
}

[System.Serializable]
public class ShowTalkData
{
    public string eventName;
    public TalkData[] talkDatas;
    
    public ShowTalkData(string name, TalkData[] td)
    {
        eventName = name;
        talkDatas = td;
    }
}

public class Dialogue : MonoBehaviour
{
    // 위에서 선언한 TalkData 배열 
    [SerializeField] TalkData[] talkDatas;

    public TalkData[] GetObjectDialogue()
    {
        return DialogueParse.GetDialogue(TextManager.Instance.storyEventName);
    }
}