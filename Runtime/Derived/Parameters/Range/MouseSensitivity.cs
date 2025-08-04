using UnityEngine;

namespace ConfigMe
{
    [CreateAssetMenu(fileName = "Mouse Sensitivity", menuName = GlobalConfigMe.PARAMETER_MENU_PATH + "Mouse Sensitivity")]
    public class MouseSensitivity : RangeParameter<int>
    {
        protected override void ApplyValue(int value)
        {
            Debug.Log($"New Mouse Sensitivity: {value}");
        }
    }
}
