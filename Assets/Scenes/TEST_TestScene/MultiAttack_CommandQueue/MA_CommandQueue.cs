using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MA_CommandQueue {
    private Queue<I_MA_Command> mQueue;
    private bool mIsPadding;

    public MA_CommandQueue(){
        mQueue = new Queue<I_MA_Command>();
        mIsPadding = false;
    }

    public void Enqueue(I_MA_Command cmd){
        mQueue.Enqueue(cmd);

        if(!mIsPadding){
            DoNext();
        }
    }

    private void DoNext(){
        if(mQueue.Count > 0){
            I_MA_Command top = mQueue.Dequeue();
            mIsPadding = true;
            
            top.OnFinished = OnCmdFinished;

            top.Execute();
        }
    }

    private void OnCmdFinished (){
        mIsPadding  = false;
        DoNext();
    }
}