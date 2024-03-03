using UnityEngine;
namespace Sophia
{
    [System.Serializable]
    public struct SerialUserInterfaceData {
        [SerializeField] public string _name;
        [SerializeField] public string _description;
        [SerializeField] public Sprite _icon;
    }
}