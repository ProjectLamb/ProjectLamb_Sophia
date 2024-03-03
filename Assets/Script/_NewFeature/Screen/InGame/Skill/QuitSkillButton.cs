using System;
using FMODPlus;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sophia.UserInterface
{
    public class QuitSkillButton : MonoBehaviour, IPointerClickHandler
    {
        public UnityAction<bool, KeyCode> func;
        public void OnPointerClick(PointerEventData eventData)
        {
            func.Invoke(false, KeyCode.None);
        }

    }
}