using UnityEngine;

namespace ConfigMe
{
    [CreateAssetMenu(fileName = "FullScreen Mode", menuName = GlobalConfigMe.PARAMETER_MENU_PATH + "FullScreen Mode")]
    public class FullScreenParameter : ChoicesParameter<FullScreenMode>
    {
        public override void ApplyValue(FullScreenMode value)
        {
            Screen.fullScreenMode = value;
        }
    }
}
