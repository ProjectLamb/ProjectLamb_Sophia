using UnityEngine;

public enum TextDataIndex {
    red, green, blue
}

[CreateAssetMenu(fileName = "Scriptable/ScriptableTextData", menuName = "TEST_MA_Events/TextData.cs/TextData", order = int.MaxValue)]
public class MA_TextData : ScriptableObject {
    //여기서 드는 생각은 GameObject가 있어야 인스턴시에트 할수 있지
    //그런데.. Text라도 인스턴시에트 할 수 있나?
    
    [TextArea]
    public string innerStrs;
    [ColorUsage(showAlpha: true)]
    public Color color;
}
