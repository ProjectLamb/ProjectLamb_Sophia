using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraEffect : MonoBehaviour {
    IEnumerator mCoZoom;
    public float mDurateTime;
    public float mAmount;
    public float mOriginFOV = 50f;
    bool mIsZoomed = false;
    private void Start(){
        GameManager.Instance.globalEvent.OnHitEvents += handleZoomIn;
    }

    public void handleZoomIn(){
        if(mIsZoomed == true) return;
        mCoZoom = ZoomCoroutine();
        StartCoroutine(mCoZoom);
    }

    IEnumerator ZoomCoroutine(){
        mIsZoomed = true;
        Camera.main.fieldOfView = mAmount;
        float valueGap = mOriginFOV - 10;
        float passedTime = 0f;
        while(mDurateTime > passedTime && Camera.main.fieldOfView <= mOriginFOV){
            passedTime += Time.deltaTime;
            Camera.main.fieldOfView += (valueGap * Time.deltaTime);
            yield return null;
        }
        Camera.main.fieldOfView = mOriginFOV;
        mIsZoomed = false;
    }
}