using FMODUnity;

namespace Sophia.DataSystem.Atomics
{
    using Sophia.Composite;
    public class AudioAtomics {
        public readonly EventReference FmodEventReference;
        public readonly ParamRef[] StartParamRefs;
        public readonly ParamRef[] ExitParamRefs;

        public AudioAtomics(EventReference fmodEventRef, ParamRef[] startParamRefs, ParamRef[] exitParamRefs) {
            FmodEventReference  =   fmodEventRef;
            StartParamRefs      =   startParamRefs;
            ExitParamRefs       =   exitParamRefs;
        }

        public void Invoke(IAudioAccessible audioAccessible) {
            audioAccessible.GetAudioManager().AddSFX(FmodEventReference);
            audioAccessible.GetAudioManager().PlaySFX(FmodEventReference);
            audioAccessible.GetAudioManager().ApplyParameterSFX(FmodEventReference, StartParamRefs);
        }

        public void Revert(IAudioAccessible audioAccessible) {
            audioAccessible.GetAudioManager().ApplyParameterSFX(FmodEventReference, ExitParamRefs);
        }
    }
}