using FMODUnity;

namespace Sophia.DataSystem.Atomics
{
    using System.Linq;
    using Sophia.Composite;
    public class AudioAtomics {
        public readonly EventReference FmodEventReference;
        public readonly ParamRef[] StartParamRefs;
        public readonly ParamRef[] ExitParamRefs;

        public AudioAtomics(in SerialAudioData audioData) {
            FmodEventReference  =   audioData._fmodEventRef;
            StartParamRefs      =   audioData._startParamRefs;
            ExitParamRefs       =   audioData._exitParamRefs;
        }

        public void Invoke(IAudioAccessible audioAccessible) {
            audioAccessible.GetAudioManager().AddSFX(FmodEventReference);
            audioAccessible.GetAudioManager().PlaySFX(FmodEventReference);
            if(StartParamRefs == null || StartParamRefs.Length == 0) {return;}
            audioAccessible.GetAudioManager().ApplyParameterSFX(FmodEventReference, StartParamRefs);
        }

        public void Revert(IAudioAccessible audioAccessible) {
            if(StartParamRefs == null || StartParamRefs.Length == 0) {return;}
            audioAccessible.GetAudioManager().ApplyParameterSFX(FmodEventReference, ExitParamRefs);
        }
    }
}