using UnityEngine;
using UnityEngine.Audio;

namespace ConfigMe
{
    [CreateAssetMenu(fileName = "Mixer Volume", menuName = GlobalConfigMe.PARAMETER_MENU_PATH + "Mixer Volume")]
    public class MixerVolume : RangeParameter<int>
    {
        [SerializeField] AudioMixer mixer;
        [SerializeField] string exposedParameterName = "volume";

        protected new void Reset()
        {
            base.Reset();

            Debug.Log($"Reset Mixer {this.name}");

            lowValue = 0;
            highValue = 100;
            defaultValue = 100;
        }

        protected override void ApplyValue(int value)
        {
            float dbValue = Mathf.Log10(Mathf.Max(0.001f, value / 100f)) * 20;

            mixer.SetFloat(exposedParameterName, dbValue);
        }
    }
}
