using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerImage : MonoBehaviour
{
    public Image speakerImage;
    public Sprite decussSprite;
    public Sprite offusiaSprite;
    // Start is called before the first frame update
    void Start()
    {
        speakerImage.sprite = decussSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSprite()
    {
        if (this.speakerImage.sprite == decussSprite)
        { // 여기서 switch문으로 나눠야할듯
            this.speakerImage.sprite = offusiaSprite;
        }

        else if (this.speakerImage.sprite == offusiaSprite)
        {
            this.speakerImage.sprite = decussSprite;
        }
    }
}
