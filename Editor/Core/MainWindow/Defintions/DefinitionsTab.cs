using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    public class DefinitionsTab : ConfigMeEditorTab
    {
        private List<ConfigDefinition> definitions = null;

        public DefinitionsTab(VisualElement root)
        {
            EditorApplication.projectChanged += OnRefreshProject;

            var createDefinitionBtn = root.Q<Button>();
            createDefinitionBtn.clickable.clicked += CreateDefinition;

            FindDefinitions();
        }

        private void OnRefreshProject()
        {
            FindDefinitions();
        }

        public override void Dispose()
        {
            Debug.Log("Disposing");
            EditorApplication.projectChanged -= OnRefreshProject;
        }



        private void FindDefinitions()
        {
            var definitionsGUIDs = AssetDatabase.FindAssets("t:ConfigDefinition");
            definitions = definitionsGUIDs.Select(guid => AssetDatabase.LoadAssetAtPath<ConfigDefinition>(AssetDatabase.GUIDToAssetPath(guid))).ToList();
        }

        private void CreateDefinition()
        {
            if(definitions.Count >= 1)
            {
                if(!EditorUtility.DisplayDialog("Create new definition", "Are you sure you want to create multiple definitions?\n" +
                                            "Check the 'Getting Started' tab before proceeding. For a basic settings menu, a single definition is usually all you need.", "Create", "Cancel"))
                {
                    return;
                }
            }

            ConfigDefinition definition = ScriptableObject.CreateInstance<ConfigDefinition>();

            Directory.CreateDirectory(ConfigMeEditor.DEFAULT_DEFINITIONS_PATH);

            string path = ConfigMeEditor.DEFAULT_DEFINITIONS_PATH + "Settings.asset";
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(definition, path);

            ConfigMeLogger.LogInfo($"Created Definition at {path}", definition);
            EditorGUIUtility.PingObject(definition);
        }

        
    }
}
