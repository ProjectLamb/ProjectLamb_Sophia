using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerImage : MonoBehaviour
{
    // 대화창 화자 이미지
    public Image speakerImage; 
    string speaker; // 화자

    [Header("OffusiaSprite")]
    public Sprite offusiaSprite;

    [Header("DecussSprite")]
    public Sprite decuss_Default;
    public Sprite decuss_Angry;
    public Sprite decuss_AngryLook;
    public Sprite decuss_Smile;
    public Sprite decuss_BigSmile;
    public Sprite decuss_OpenMouth;
    public Sprite decuss_Panic;
    public Sprite decuss_PanicOpenMouth;
    public Sprite decuss_Sad;

    // Start is called before the first frame update
    void Start()
    {
        speakerImage.sprite = decuss_Default;
        speaker = "Decuss";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSprite(string speakerName, string emotionState)
    {
        speaker = speakerName;
        
        // decuss -> offusia로 스프라이트 변환
        if (speaker.Equals("오프시아")) 
        {
            this.speakerImage.sprite = offusiaSprite;
        }

        // offusia -> decuss로 스프라이트 변환
        else if (speaker.Equals("데커스")) 
        {
            if(emotionState.Equals("Default"))
                this.speakerImage.sprite = decuss_Default;
            else if(emotionState.Equals("Angry"))
                this.speakerImage.sprite = decuss_Angry;
            else if(emotionState.Equals("AngryLook"))
                this.speakerImage.sprite = decuss_AngryLook;
            else if(emotionState.Equals("Smile"))
                this.speakerImage.sprite = decuss_Smile;
            else if(emotionState.Equals("BigSmile"))
                this.speakerImage.sprite = decuss_BigSmile;
            else if(emotionState.Equals("OpenMouth"))
                this.speakerImage.sprite = decuss_OpenMouth;
            else if(emotionState.Equals("Panic"))
                this.speakerImage.sprite = decuss_Panic;
            else if(emotionState.Equals("PanicOpenMouth"))
                this.speakerImage.sprite = decuss_PanicOpenMouth;
            else if(emotionState.Equals("Sad"))
                this.speakerImage.sprite = decuss_Sad;
        }
    }
}
