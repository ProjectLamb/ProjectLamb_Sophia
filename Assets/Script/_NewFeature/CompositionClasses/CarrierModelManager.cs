using System.Collections;
using System.Collections.Generic;
using Sophia.Instantiates;
using Sophia.UserInterface;
using UnityEngine;

public class CarrierModelManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] bool _isSkillItem;
    [SerializeField] bool _isEquipmentItem;

    public void SetBySkill() {
        SkillItem carrier = GetComponent<SkillItem>();
        StartCoroutine(AsyncRender.PerformAndRenderUI(() => {
            _spriteRenderer.sprite = carrier._userInterfaceData._icon;
            _spriteRenderer.color = new Color(0,0,0,1);
        }));
    }

    void Start() {
        if(_isSkillItem) { SetBySkill(); return; }
        if(_isEquipmentItem) {}
    }
}
