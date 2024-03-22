using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour {
    public Player player;
    public Weapon weapon;
    public UnityEvent OnChangeEvent; // -> 이놈을 서브스크라이브 해야된다.
    private void Awake() {
        if(OnChangeEvent == null) { throw new System.Exception("OnChangeEvent가 Null임 이렇게 되면 PlayerDataManger에 값변경 구독이 안되어 있는것. 인스펙터 확인 ㄱㄱ"); }   
    }
    private void Start() {
        this.weapon.Initialisze(player);
        OnChangeEvent.Invoke();
    }
    public void AssignWeapon(Weapon _weapon){
        foreach(Transform child in transform){ Destroy(child); }
        this.weapon = _weapon;
        this.weapon.Initialisze(player);
        OnChangeEvent.Invoke();
    }
}