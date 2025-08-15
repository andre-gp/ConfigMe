using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    public static class ConfigMeValidator
    {
        public static VisualElement SetupWarning(VisualElement content)
        {
            VisualElement root = new VisualElement();

            root.Add(content);

            /* --- WARNING CONTENT --- */
            var warningRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ConfigMeEditor.VALIDATOR_VISUAL_TREE_ASSET).Instantiate();            
            root.Add(warningRoot);

            var createConfigManagerButton = warningRoot.Q<Button>();
            createConfigManagerButton.clickable.clicked += () =>
            {
                ConfigManagerTracker.CreateNewConfigManager();
            };

            Label warningLabel = new Label("ConfigManager is not set!");
            warningRoot.Add(new Label());


            Action<ConfigManager> OnChangeConfigManager = (manager) =>
            {
                content.style.display = manager == null || manager.Equals(null) ? DisplayStyle.None : DisplayStyle.Flex;
                warningRoot.style.display = manager == null || manager.Equals(null) ? DisplayStyle.Flex : DisplayStyle.None;
            };

            OnChangeConfigManager?.Invoke(ConfigManagerTracker.ConfigManager);
            ConfigManagerTracker.OnChangeConfigManager += OnChangeConfigManager;


            return root;
        }
    }
}
