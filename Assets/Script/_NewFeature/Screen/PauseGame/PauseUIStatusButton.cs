using System.Collections;
using System.Collections.Generic;
using Sophia.UserInterface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Sophia
{
    public class PauseUIStatusButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI[] StatNum = new TextMeshProUGUI[6];
        DataSystem.Referer.PlayerStatReferer playerReferer;
        private void Awake()
        {
            playerReferer ??= GameManager.Instance.PlayerGameObject.GetComponent<Entitys.Player>().GetStatReferer() as DataSystem.Referer.PlayerStatReferer;
        }


        private void OnEnable()
        {
            UIUpdate();
        }

        public void UIUpdate()
        {
            StartCoroutine(AsyncRender.Instance.PerformAndRenderUI(() =>
            {
                DOVirtual.Int(0, (int)playerReferer.GetStat(E_NUMERIC_STAT_TYPE.MaxHp).GetValueForce(), 0.5f, (E) => {StatNum[0].text = E.ToString();}).SetUpdate(true);
                DOVirtual.Int(0, (int)playerReferer.GetStat(E_NUMERIC_STAT_TYPE.MoveSpeed).GetValueForce(), 0.5f, (E) => {StatNum[1].text = E.ToString();}).SetUpdate(true);
                DOVirtual.Int(0, (int)playerReferer.GetStat(E_NUMERIC_STAT_TYPE.Defence).GetValueForce(), 0.5f, (E) => {StatNum[2].text = E.ToString();}).SetUpdate(true);
                DOVirtual.Int(0, (int)playerReferer.GetStat(E_NUMERIC_STAT_TYPE.Power).GetValueForce(), 0.5f, (E) => {StatNum[3].text = E.ToString();}).SetUpdate(true);
                DOVirtual.Int(0, 100, 0.5f, (E) => {StatNum[4].text = E.ToString();}).SetUpdate(true);
                DOVirtual.Int(0, (int)playerReferer.GetStat(E_NUMERIC_STAT_TYPE.MaxStamina).GetValueForce(), 0.5f, (E) => {StatNum[5].text = E.ToString();}).SetUpdate(true);
            }));
        }
    }
}