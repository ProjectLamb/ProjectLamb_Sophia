using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class ParticleModulator : MonoBehaviour
{
    public void ActivateParticle(ParticleSystem _particle, float _durationTime){
        var ActivatingParticle = Instantiate(_particle.gameObject, transform);
        Destroy(ActivatingParticle, _durationTime);
    }
}
