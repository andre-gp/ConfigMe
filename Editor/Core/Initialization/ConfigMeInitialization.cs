using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    public class ConfigMeInitialization : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset mainWindow = default;

        [MenuItem("ConfigMe/ConfigMe Editor")]
        public static void ShowExample()
        {
            ConfigMeInitialization wnd = GetWindow<ConfigMeInitialization>();
            wnd.titleContent = new GUIContent("ConfigMeInitialization");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            VisualElement instantiatedMainWindow = mainWindow.Instantiate();
            instantiatedMainWindow.style.height = Length.Percent(100);
            root.Add(instantiatedMainWindow);
        }
    } 
}
