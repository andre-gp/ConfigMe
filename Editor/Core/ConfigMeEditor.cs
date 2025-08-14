using UnityEngine;

namespace ConfigMe.EditorCM
{
    public class ConfigMeEditor
    {
        #region Paths
        public const string USER_FOLDER = "Assets/Plugins/ConfigMe/";
        public const string DEFAULT_DEFINITIONS_PATH = USER_FOLDER;

        #endregion

        #region Default Components
        public const string COMPONENT_DROPDOWN = "DropdownParameter";
        public const string COMPONENT_FLOAT_SLIDER = "FloatSliderParameter";
        public const string COMPONENT_INT_SLIDER = "IntSliderParameter";
        public const string COMPONENT_SLIDE_TOGGLE = "SlideToggleParameter";
        public const string COMPONENT_STEPPER = "StepperParameter";
        public const string COMPONENT_TOGGLE = "ToggleParameter";
        #endregion
    }
}
