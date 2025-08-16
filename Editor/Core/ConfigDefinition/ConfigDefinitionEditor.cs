using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    [CustomEditor(typeof(ConfigDefinition))]
    public class ConfigDefinitionEditor : Editor
    {
        private ConfigDefinition definition = null;
        private ScrollView scroll = null;
        private SerializedProperty parametersProperty = null;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            definition = target as ConfigDefinition;

            ParameterEditor.AddScriptableObjectField(root, definition);

            SerializedProperty iterator = serializedObject.GetIterator();

            iterator.NextVisible(true); // Steps into the children.

            while (iterator.NextVisible(false))
            {
                

                var propertyField = new PropertyField(iterator);
                
                propertyField.Bind(serializedObject);
                root.Add(propertyField);
            }

            parametersProperty = serializedObject.FindProperty("parameters");

            VisualElement customList = CustomList.GenerateCustomList();           

            scroll = customList.Q<ScrollView>();
            scroll.TrackPropertyValue(parametersProperty, OnChangeParameters);

            RebuildList();

            root.Add(customList);


            return root;
        }

        private void RebuildList()
        {
            scroll.Clear();

            var sortedParameters = definition.Parameters.ToList();
            sortedParameters.Sort((x, y) => x.name.CompareTo(y.name));

            foreach (var parameter in sortedParameters)
            {
                VisualElement paramRow = new VisualElement();
                paramRow.AddToClassList("custom-list-row-container");

                Button removeBtn = new Button() { text = "-"};
                removeBtn.AddToClassList(CustomList.ussButtonRemoveClassName);
                paramRow.Add(removeBtn);

                //Texture2D icon = AssetPreview.GetMiniThumbnail(parameter);
                //VisualElement iconElement = new VisualElement();
                //iconElement.style.backgroundImage = icon;
                ////iconElement.style.backgroundColor = Color.white;
                //iconElement.style.width = 15;
                //iconElement.style.height = 15;
                //paramRow.Add(iconElement);

                var label = new Label($"{parameter.name}");
                label.AddToClassList(CustomList.ussLabelClassName);
                paramRow.Add(label);


                var desc = new Label($"({parameter.GetType().Name})");
                desc.style.fontSize = 10;
                desc.style.opacity = 0.7f;
                desc.style.marginLeft = 6;
                desc.style.alignSelf = Align.Center;
                paramRow.Add(desc);

                scroll.Add(paramRow);
            }
        }

        private void OnChangeParameters(SerializedProperty property)
        {
            
        }
    }
}
