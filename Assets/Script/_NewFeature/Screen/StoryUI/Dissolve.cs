using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dissolve : MonoBehaviour
{
    GameObject dissolvePanel;       
    RawImage rwImage;                    
    private bool IsDissolveOver = false;    

    void Awake()
    {
        dissolvePanel = this.gameObject;                        
        rwImage = dissolvePanel.GetComponent<RawImage>();    
    }
    void Update()
    {
        StartCoroutine("DissolveEffect");                     
        if (IsDissolveOver)                                
        {
            Destroy(this.gameObject);       
        }
    }
    IEnumerator DissolveEffect()
    {
        Color color = rwImage.color;                     
        color.a -= Time.deltaTime * 0.05f;             
        rwImage.color = color;    
                                    
        if (rwImage.color.a <= 0)                   
        {
            IsDissolveOver = true;              
        }
        yield return null;                           

    }
}
