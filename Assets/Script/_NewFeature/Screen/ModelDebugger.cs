using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sophia.UserInterface
{
    public class ModelDebugger : MonoBehaviour
    {

#region Serialized Member
        //대충 TMP 가져오기
        [SerializeField] TextMeshProUGUI text;
        //Player도 가져오기 특히 DataAccessible
        [SerializeField] IDataAccessible dataAccessible;
        #endregion
        [SerializeField] GameObject pauseMenuObject;

#region Member
        Queue<GameObject> panalQueue = new Queue<GameObject>();
        Coroutine CoRenderModelData;

        private void OnDestroy() {
            dataAccessible.GetStatReferer().RemoveListenerToStats(StartRender);
        }

#endregion

        // Open
        private void OpenMenu()
        {
            panalQueue.Enqueue(pauseMenuObject);
            var topMenu = panalQueue.Peek();
            topMenu.SetActive(true);
        }
        // Close

        private void CloseMenu()
        {
            var topMenu = panalQueue.Peek();
            topMenu.SetActive(false);
            panalQueue.Dequeue();
        }

        public void ToggleMenu()
        {
            LazyDependency();
            if (panalQueue.Count == 0)
            {
                OpenMenu();
                StartRender();
            }
            else if (panalQueue.Count == 1)
            {
                CloseMenu();
            }
        }

        public void StartRender() => CoRenderModelData = StartCoroutine(GlobalAsync.PerformAndRenderUI( () => text.text = dataAccessible.GetStatsInfo().ToString()));
        // 렌더링 동작 (이벤트에 연결되는 녀석)

        private void LazyDependency() {
            if(dataAccessible == null) {
                dataAccessible = GameManager.Instance.PlayerGameObject.GetComponent<IDataAccessible>();
                dataAccessible.GetStatReferer().AddListenerToStats(StartRender);
            }
        }
    }
}