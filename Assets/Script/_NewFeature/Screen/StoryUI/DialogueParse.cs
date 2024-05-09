using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParse : MonoBehaviour
{

public static Dictionary<string, TalkData[]> DialogueDictionary = 
				new Dictionary<string, TalkData[]>();
[SerializeField] private TextAsset csvFile = null;
[SerializeField] List<ShowTalkData> ShowTalkDataList = 
					    new List<ShowTalkData>();
                        
void SetShowTalkData()
{
    // 딕셔너리의 키 값들을 가진 리스트
    List<string> eventNames = 
        		new List<string>(DialogueDictionary.Keys);
    // 딕셔너리의 밸류 값들을 가진 리스트
    List<TalkData[]> talkDatasList = 
        		new List<TalkData[]>(DialogueDictionary.Values);

    // 딕셔너리의 크기만큼 추가
    for(int i = 0; i < eventNames.Count; i++)
    {
        ShowTalkData showTalk = 
        	new ShowTalkData (eventNames[i], talkDatasList[i]);
        
        ShowTalkDataList.Add(showTalk);
    }
 }

public static TalkData[] GetDialogue(string eventName)
{
    return DialogueDictionary[eventName];
}

private void Awake()
{
    SetTalkDictionary();
    SetShowTalkData();
}
public void SetTalkDictionary()
{
    // 아래 한 줄 빼기
    string csvText = csvFile.text.Substring(0, csvFile.text.Length - 1);
    // 줄바꿈(한 줄)을 기준으로 csv 파일을 쪼개서 string배열에 줄 순서대로 담음
    string[] rows = csvText.Split(new char[] { '\n' }); 

    // 엑셀 파일 1번째 줄은 편의를 위한 분류이므로 i = 1부터 시작
    for (int i = 1; i < rows.Length; i++)
    {
        // A, B, C열을 쪼개서 배열에 담음
        string[] rowValues = rows[i].Split(new char[] { ',' });

        // 유효한 이벤트 이름이 나올때까지 반복
        if (rowValues[0].Trim() == "" || rowValues[0].Trim() == "end") continue;

        List<TalkData> talkDataList = new List<TalkData>();
        string eventName = rowValues[0]; // 이벤트 이름

        while(rowValues[0].Trim() != "end") // talkDataList 하나를 만드는 반복문
        {
            // 캐릭터가 한번에 치는 대사의 길이를 모르므로 리스트로 선언
            List<string> contextList = new List<string>();

            TalkData talkData;
            talkData.name = rowValues[1].Trim(); // 캐릭터 이름이 있는 B열
            talkData.emotionState = rowValues[3].Trim(); // 캐릭터의 감정상태를 나타내는 D열

            do // talkData 하나를 만드는 반복문
            {
                contextList.Add(rowValues[2].ToString());
                if(++i < rows.Length) 
                    rowValues = rows[i].Split(new char[] { ',' });
                else break;
            } while (rowValues[1] == "" && rowValues[0] != "end"); // 화자의 이름이 비어있지 않고, 이벤트가 END가 아닐때까지

            talkData.contexts = contextList.ToArray();
            talkDataList.Add(talkData);
        }

        DialogueDictionary.Add(eventName, talkDataList.ToArray());
    }
}
}
