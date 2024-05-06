using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TEST_GroundCastController : MonoBehaviour
{
    [SerializeField] public UnityEvent<Vector3> HitGroundHander;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) TEST_MouseClickGround();
    }

    public void TEST_MouseClickGround() {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);   
        if (Physics.Raycast(camRay, out RaycastHit hit, 500, 1 << LayerMask.NameToLayer("Map"))){
            MouseClickHandler(hit);
        }
    }

    public void MouseClickHandler(RaycastHit hit){
        Debug.Log("Clicked");
        Debug.Log($"Position : {hit.point}");
        HitGroundHander.Invoke(hit.point);
    }
}
