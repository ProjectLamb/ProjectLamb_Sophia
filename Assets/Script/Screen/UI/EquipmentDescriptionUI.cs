using System.Collections;
using System.Collections.Generic;
using Sophia.Composite;
using UnityEngine;
using TMPro;
using NUnit.Framework.Internal;

public class EquipmentDescriptionUI : MonoBehaviour
{
    #region Members
    private enum E_TEXT { EquipmentName = 0, EquipmentDescription };
    [SerializeField] private float displayTime = 5f;
    GameObject[] gameObjectList = new GameObject[3];
    TextMeshProUGUI[] text = new TextMeshProUGUI[2];
    #endregion

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            gameObjectList[i] = transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < text.Length; i++)
        {
            text[i] = gameObjectList[i + 1].GetComponent<TextMeshProUGUI>();
        }
        DisplayOff();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(string name, string description)
    {
        this.text[(int)E_TEXT.EquipmentName].text = name;
        this.text[(int)E_TEXT.EquipmentDescription].text = description;
        this.displayTime = 5f;
        CancelInvoke();
    }

    public void Init(string name, string description, float displayTime)
    {
        this.text[(int)E_TEXT.EquipmentName].text = name;
        this.text[(int)E_TEXT.EquipmentDescription].text = description;
        this.displayTime = displayTime;
        CancelInvoke();
    }

    public void DisplayOn()
    {
        foreach (GameObject gameObject in gameObjectList)
        {
            gameObject.SetActive(true);
        }
        Invoke("DisplayOff", displayTime);
    }

    public void DisplayOff()
    {
        foreach (GameObject gameObject in gameObjectList)
        {
            gameObject.SetActive(false);
        }
    }
}
