using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class TEST_Player : MonoBehaviour
{
    public TEST_NumericData numericData;
    public TEST_AttributeData attributeData;

    [HideInInspector]
    public Material material;
    public TMP_Text numTextMeshPro;
    public TMP_Text attTextMeshPro;
    JsonSerializerSettings setting;
    float targetPosx = 5f;
    private void Awake()
    {
        numericData = GetComponent<TEST_NumericData>();
        attributeData = GetComponent<TEST_AttributeData>();

        material = GetComponent<Renderer>().material;
        setting = new JsonSerializerSettings();
        setting.Formatting = Formatting.Indented;
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    }

    private void Update()
    {
        numTextMeshPro.text = JsonUtility.ToJson(numericData, true);
        attTextMeshPro.text = JsonConvert.SerializeObject(attributeData.tempAttributeData, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosx, 1f, -1.5f), Time.deltaTime * numericData.MoveSpeed);
        if (transform.position.x >= 4.5f)
        {
            targetPosx = -5f;
        }
        if (transform.position.x <= -4.5f)
        {
            targetPosx = 5f;
        }
    }
}

