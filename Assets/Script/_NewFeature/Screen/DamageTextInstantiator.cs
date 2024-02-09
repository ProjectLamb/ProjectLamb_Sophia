using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sophia;
using Sophia.Composite;

/*********************************************************************************
메이플과 같은 데미지 UI를 띄울것이다.

그냥과 버퍼드 UI
    AlertUIManager;

*********************************************************************************/
public enum DamageInstant_Type
{
    Stacking, Layerd
}

public class DamageTextInstantiator : MonoBehaviour
{
    public LifeComposite LifeCompositeRef;
    [SerializeField]
    private DamageTextUI textUI;
    private DamageTextUI StakingText;
    private List<DamageTextUI> LayerdTextList;

    private void Start() {
        LifeCompositeRef ??= GetComponentInParent<ILifeAccessible>().GetLifeComposite();
        LifeCompositeRef.OnDamaged += Generate;
    }

    private void OnDisable() {
        LifeCompositeRef.OnDamaged -= Generate;   
    }
    
    public void Generate(DamageInfo _damageInfo)
    {
        if (StakingText == null)
        {
            StakingText = Instantiate(textUI, transform)
                            .SetTextByString(_damageInfo.GetAmountByString())
                            .SetAnimationSpeed(5f)
                            .SetDestroyTimer(3f);
            StakingText.ActivatedTextUI();
        }
        else
        {
            StakingText.SetPosition(transform.position)
                        .ReactivateTextUI(_damageInfo);
        }

    }

    public void Generate(List<DamageInfo> _damageInfo)
    {
        LayerdTextList = new List<DamageTextUI>();
        Vector3 addingPosition = transform.position;
        _damageInfo.ForEach((E) =>
        {
            LayerdTextList.Add(
                Instantiate(textUI, transform)
                    .SetTextByString(E.GetAmountByString())
                    .SetAnimationSpeed(5f)
                    .SetDestroyTimer(3f)
            );
            LayerdTextList.Last().SetPosition(addingPosition += textUI.GetNextPosition());
        });
        LayerdTextList.ForEach((T) => { T.ActivatedTextUI(); });
    }
}