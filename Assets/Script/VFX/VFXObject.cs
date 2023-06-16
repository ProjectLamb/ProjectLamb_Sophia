using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 모든 VFX는 파티클이다.
/// </summary>
public class VFXObject : MonoBehaviour {
    //Stacking은, 단 하나만 존재하는것이 아닌 여러번 소환 가능한지
    private ParticleSystem PS;
    public bool IsNoneStacking = false;
    public float durateTime;
    public STATE_TYPE affectorType;
    //단, 이 오브젝트는 무조건 파티클의 부모, 자식으로 구성된 놈만.
    public UnityEvent onDestroyEvent;
    private void Awake() {
        if(TryGetComponent<ParticleSystem>(out PS)){ durateTime = PS.main.duration + 0.01f; }
    }
    
    public void InitializeByTime(float durateTime){
        this.durateTime = durateTime;
        Destroy(gameObject, durateTime);
    }
    public void Initialize(){
        Destroy(gameObject, this.durateTime);
    }
    private void OnDestroy() { onDestroyEvent.Invoke(); }
}