using UnityEngine;

namespace ConfigMe.EditorCM
{
    public class ConfigMeEditor
    {
        #region Default Components
        public const string COMPONENT_DROPDOWN = "DropdownParameter";
        public const string COMPONENT_FLOAT_SLIDER = "FloatSliderParameter";
        public const string COMPONENT_INT_SLIDER = "IntSliderParameter";
        public const string COMPONENT_SLIDE_TOGGLE = "SlideToggleParameter";
        public const string COMPONENT_STEPPER = "StepperParameter";
        public const string COMPONENT_TOGGLE = "ToggleParameter";
        #endregion

        #region Paths
        public const string USER_FOLDER = "Assets/Plugins/ConfigMe/";
        public const string PACKAGE_FOLDER = "Packages/com.gaton.config-me/";

        public const string DEFAULT_DEFINITIONS_PATH = USER_FOLDER;

        public const string DEFAULT_CONFIG_MANAGER_PATH = USER_FOLDER + "ConfigManager.prefab";

        public const string VALIDATOR_VISUAL_TREE_ASSET = PACKAGE_FOLDER + "Editor/Core/Validator/WarningMessage.uxml";
        #endregion


        #region EditorPrefs Keys

        public const string KEY_CONFIG_MANAGER_GUID = "Key_ConfigManagerGUID";

        #endregion
    }
}
