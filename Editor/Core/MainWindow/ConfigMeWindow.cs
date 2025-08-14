using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    public class ConfigMeWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset mainWindow = null;

        [SerializeField] private VisualTreeAsset definitionsWindow = null;

        DefinitionsTab definitionsTab;

        public ConfigMeWindow()
        {
            EditorApplication.projectChanged += OnRefreshProject;
        }

        private void OnRefreshProject()
        {
            
        }

        private void OnDestroy()
        {
            definitionsTab.Dispose();
            EditorApplication.projectChanged -= OnRefreshProject;
        }


        [MenuItem("ConfigMe/ConfigMe Editor")]
        public static void OpenWindow()
        {
            ConfigMeWindow wnd = GetWindow<ConfigMeWindow>();
            wnd.titleContent = new GUIContent("ConfigMe");
            wnd.minSize = new Vector2(400, 400);
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            VisualElement instantiatedMainWindow = mainWindow.Instantiate();
            instantiatedMainWindow.style.height = Length.Percent(100);
            root.Add(instantiatedMainWindow);


            var rootDefinitions = definitionsWindow.Instantiate();

            DefinitionsTab definitionsTab = new DefinitionsTab(rootDefinitions);            


            root.Q<VisualElement>("tab-definitions__content-container").Add(rootDefinitions);

        }
    } 
}
