using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    public class ConfigMeWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset mainWindow = null;

        [SerializeField] private VisualTreeAsset definitionsWindow = null;

        [SerializeField] private VisualTreeAsset homeWindow = null;

        List<ConfigMeEditorTab> tabs;

        public ConfigMeWindow()
        {
            tabs = new List<ConfigMeEditorTab>();

            EditorApplication.projectChanged += OnRefreshProject;
        }

        private void OnRefreshProject()
        {
            
        }

        private void OnDestroy()
        {
            foreach (var tab in tabs)
            {
                tab.Dispose();
            }

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

            VisualElement instantiatedMainWindow = mainWindow.Instantiate();
            instantiatedMainWindow.style.height = Length.Percent(100);


            rootVisualElement.Add(ConfigMeValidator.SetupWarning(instantiatedMainWindow));


            AddTab(new DefinitionsTab(), definitionsWindow, "tab-definitions__content-container");
            AddTab(new HomeTab(), homeWindow, "tab-home__content-container");
        }

        void AddTab(ConfigMeEditorTab tab, VisualTreeAsset template, string containerName)
        {
            var tabRoot = template.Instantiate();
            tab.InitTab(tabRoot);
            tabs.Add(tab);

            rootVisualElement.Q<VisualElement>(containerName).Add(tabRoot);
        }
    } 
}
