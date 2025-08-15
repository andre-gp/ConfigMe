using ConfigMe.EditorCM.UIElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    public class DefinitionsTab : ConfigMeEditorTab
    {
        ScrollView definitionsScrollView;

        public DefinitionsTab(VisualElement root)
        {
            var createDefinitionBtn = root.Q<Button>();
            createDefinitionBtn.clickable.clicked += CreateDefinition;

            definitionsScrollView = root.Q<ScrollView>();

            DefinitionsTracker.OnChangeDefinitions += OnChangeDefinitions;

            CreateDefinitionsPropertyFields(definitionsScrollView, DefinitionsTracker.Definitions);
        }

        public override void Dispose()
        {
            DefinitionsTracker.OnChangeDefinitions -= OnChangeDefinitions;
        }

        private void OnChangeDefinitions(IReadOnlyList<ConfigDefinition> definitions)
        {
            CreateDefinitionsPropertyFields(definitionsScrollView, definitions);
        }

        private void CreateDefinitionsPropertyFields(VisualElement root, IReadOnlyList<ConfigDefinition> definitions)
        {
            root.Clear();

            foreach (var definition in definitions)
            {
                VisualElement definitionRoot = new VisualElement();
                definitionRoot.style.SetBorderWidth(1f);
                definitionRoot.style.SetBorderColor(Color.orangeRed);
                definitionRoot.style.SetPadding(10f);
                definitionRoot.style.SetMargin(10f);

                var editor = Editor.CreateEditor(definition, typeof(ConfigDefinitionEditor));

                definitionRoot.Add(editor.CreateInspectorGUI());

                root.Add(definitionRoot);

                root.Add(new Separator()); 
            }
        }

        private void CreateDefinition()
        {
            if (DefinitionsTracker.Definitions.Count >= 1)
            {
                if (!EditorUtility.DisplayDialog("Create new definition", "Are you sure you want to create multiple definitions?\n" +
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
