using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 모든 VFX는 파티클이다.
/// </summary>
public class VFXObject : MonoBehaviour {
    public bool IsNoneStacking = false;
    public float durateTime;
    public E_StateType affectorType;
    //단, 이 오브젝트는 무조건 파티클의 부모, 자식으로 구성된 놈만.
    public UnityEvent onDestroyEvent;
    
    public void Initialize(float durateTime){
        this.durateTime = durateTime;
        Destroy(gameObject, durateTime);
    }
    private void OnDestroy() {
        onDestroyEvent.Invoke();
    }
}