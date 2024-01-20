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
        LifeCompositeRef ??= GetComponentInParent<ILifeAccessable>().GetLifeComposite();
        LifeCompositeRef.AddOnDamageEvent(Generate);
    }

    public void Generate(float _damageAmount)
    {
        if (StakingText == null)
        {
            StakingText = Instantiate(textUI, transform)
                                                .SetText(_damageAmount)
                                                .SetAnimationSpeed(5f)
                                                .SetDestroyTimer(3f);

            StakingText.ActivatedTextUI();
        }
        else
        {
            StakingText.SetPosition(transform.position)
                        .ReactivateTextUI((int)_damageAmount);
        }

    }

    public void Generate(List<float> _damageAmount)
    {
        LayerdTextList = new List<DamageTextUI>();
        Vector3 addingPosition = transform.position;
        _damageAmount.ForEach((E) =>
        {
            LayerdTextList.Add(
                Instantiate(textUI, transform)
                    .SetText(E)
                    .SetAnimationSpeed(5f)
                    .SetDestroyTimer(3f)
            );
            LayerdTextList.Last().SetPosition(addingPosition += textUI.GetNextPosition());
        });
        LayerdTextList.ForEach((T) => { T.ActivatedTextUI(); });
    }
}