using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sophia
{
    using Instantiates;
    using UserInterface;
    public class CarrierModelManager : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] bool _isSkillItem;
        [SerializeField] bool _isEquipmentItem;

        public void SetBySkill()
        {
            SkillItemObject carrier = GetComponent<SkillItemObject>();
            StartCoroutine(AsyncRender.Instance.PerformAndRenderUI(() =>
            {
                _spriteRenderer.sprite = carrier._userInterfaceData._icon;
                _spriteRenderer.color = new Color(0, 0, 0, 1);
            }));
        }

        void Start()
        {
            if (_isSkillItem) { SetBySkill(); return; }
            if (_isEquipmentItem) { }
        }
    }

}