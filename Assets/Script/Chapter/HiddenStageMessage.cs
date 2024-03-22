using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HiddenStageMessage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.ChapterGenerator.GetComponent<ChapterGenerator>().hiddenStage)
        {
            gameObject.GetComponent<TextMeshProUGUI>().enabled = true;
            Destroy(gameObject, 2f);
        }
    }
}
