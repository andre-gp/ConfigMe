using UnityEngine;

namespace ConfigMe
{
    [CreateAssetMenu(fileName = "Mute Audio", menuName = GlobalConfigMe.PARAMETER_MENU_PATH + "Mute Audio")]
    public class MuteAudio : BoolParameter
    {
        public override void ApplyValue(bool value)
        {
            Debug.Log($"Mutei: {value}");
        }
    }
}
