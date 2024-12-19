using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Sophia.UserInterface
{
    public class UIChapterTimer : MonoBehaviour
    {
        private bool isInitialized;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private float _currentTime;
        
        public void Initialize()
        {
            if (isInitialized)
            {
                return;
            }
            isInitialized = true;
        }
        
        public void SetTime(float time)
        {
            _currentTime = time;
            Debug.Log($"{((int)(_currentTime / 60)):D2} : {((int)(_currentTime % 60)):D2}");
            StartCoroutine(GlobalAsync.PerformAndRenderUI(() =>
            {
                _timerText.text = $"{((int)(_currentTime / 60)):D2} : {((int)(_currentTime % 60)):D2}"; 
            }));
        }

        public void TimeFinishedHandler()
        {
            // TODO 빨간색 정지 
            StartCoroutine(GlobalAsync.PerformAndRenderUI(() =>
            {
                _timerText.text = "!!DATA MAD!!";
                _timerText.color = Color.red;
            }));
        }

        public void TimeIntervalHandler()
        {
            _currentTime--;
            Debug.Log(_timerText.text);
            StartCoroutine(GlobalAsync.PerformAndRenderUI(() =>
            {
                _timerText.text = $"{((int)(_currentTime / 60)):D2} : {((int)(_currentTime % 60)):D2}";
            }));
            // 타임 하나씩 감소중
        }
    }
}