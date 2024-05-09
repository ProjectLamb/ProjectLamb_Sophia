using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sophia.CutScene {
    // Enter → Running         → Exit
    //         WaitingInput    ↗
    public class CutSceneQueue {
        public string CutSceneName {get; set;}
        private Queue<ICutScene> CutScenes;
        public CutSceneQueue(string cutSceneName) {
            CutSceneName = cutSceneName;
        }
    }

    public class CutSceneManager : MonoBehaviour
    {
        private Dictionary<string, Queue<ICutScene>> cutScenes;
        public Queue<ICutScene> CurrentCutScene;
        private void Awake() {
            cutScenes = new Dictionary<string, Queue<ICutScene>>();
        }

        public void EnqueueCutScene(string key, ICutScene newCutScene) {
            if(cutScenes.TryGetValue(key, out Queue<ICutScene> queue)) {
                queue.Peek().OnExitEvent += () => queue.Dequeue();
                queue.Peek().OnSkipEvent += () => queue.Dequeue();
                queue.Peek().OnEndEvent += NextScene;
                queue.Enqueue(newCutScene);
            }
            else {
                cutScenes.Add(key, new Queue<ICutScene>());
                cutScenes[key].Enqueue(newCutScene);
            }
        }
        
        public void SetCurrentCutScene(string key) {
            if(cutScenes.TryGetValue(key, out Queue<ICutScene> queue)) {
                CurrentCutScene = queue;
            }
            else 
            {
                throw new System.Exception("키에 없는 컷씬을 세팅하려고 함");
            }
        }

        public void Enter() => CurrentCutScene.Peek().Enter();
        public void Run() => CurrentCutScene.Peek().Run();
        public void NextScene() => CurrentCutScene.Dequeue().Enter();
    }
}