using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SophiaCollections
{

    public class EventQueue : MonoBehaviour
    {
        private readonly Queue<IEventCommand> mEventCommnads;
        private bool mIsRunning;

        public EventQueue() {
            mEventCommnads = new Queue<IEventCommand>();
            mIsRunning = false;
        }
        public void Enqueue(IEventCommand _eventCommand)
        {
            mEventCommnads.Enqueue(_eventCommand);
            if(mIsRunning) DoNext();
        }

        public void DoNext()
        {
            if(mEventCommnads.Count == 0){throw new System.Exception("큐가 비어있음");}
            IEventCommand curEventCmd = mEventCommnads.Dequeue();
            mIsRunning = true;
            curEventCmd.OnFinished += OnEventCommandsFinished;
            curEventCmd.Execute();
        }

        private void OnEventCommandsFinished()
        {
            mIsRunning = false;
        }
    }
}