using UnityEngine;

namespace ConfigMe
{
    [CreateAssetMenu(fileName = "VSync", menuName = GlobalConfigMe.PARAMETER_MENU_PATH + "VSync")]
    public class VSyncParameter : ChoicesParameter<int>
    {
        public override void ApplyValue(int value)
        {
            QualitySettings.vSyncCount = value;
        }
    }
}
