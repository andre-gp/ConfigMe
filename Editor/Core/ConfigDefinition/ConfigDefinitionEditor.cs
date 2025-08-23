using System;
using System.Collections.Generic;
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
            root.Bind(serializedObject);

            definition = target as ConfigDefinition;

            Label title = new Label();
            title.style.backgroundColor = Color.gray2;
            //title.style.color = Color.black;
            title.style.SetBorderColor(Color.black);
            title.style.SetBorderWidth(1f);
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.fontSize = 15;
            title.style.unityTextAlign = TextAnchor.MiddleCenter;
            title.style.SetMargin(20, 10, 10, 10);
            title.style.maxWidth = 400;
            title.style.alignSelf = Align.Center;
            title.style.SetPadding(0, 0, 50, 50);
            title.text = definition.name;
            title.bindingPath = "m_Name";

            title.RegisterCallback<MouseDownEvent>(evt =>
            {
                EditorGUIUtility.PingObject(definition);
            });

            root.Add(title);

            SerializedProperty iterator = serializedObject.GetIterator();

            iterator.NextVisible(true); // Steps into the children.

            while (iterator.NextVisible(false))
            {
                if (iterator.name == "parameters")
                    continue;

                var propertyField = new PropertyField(iterator);

                propertyField.Bind(serializedObject);
                root.Add(propertyField);
            }

            parametersProperty = serializedObject.FindProperty("parameters");

            VisualElement customList = CustomList.GenerateCustomList("Parameters");

            var btn = customList.Q<Button>();
            btn.clicked += () =>
            {
                var popup = new AddAssetPopup<Parameter>(GetMissingParameters());

                popup.OnSelect += (selected) =>
                {
                    serializedObject.UpdateIfRequiredOrScript();

                    parametersProperty.arraySize += 1;
                    var newElement = parametersProperty.GetArrayElementAtIndex(parametersProperty.arraySize - 1);
                    newElement.boxedValue = selected;
                    serializedObject.ApplyModifiedProperties();
                    popup.AssetsToDisplay = GetMissingParameters();
                    RebuildList();
                };

                UnityEditor.PopupWindow.Show(btn.worldBound, popup);

            };


            scroll = customList.Q<ScrollView>();
            scroll.TrackPropertyValue(parametersProperty, OnChangeParameters);

            RebuildList();

            root.Add(customList);


            return root;
        }

        private IEnumerable<Parameter> GetMissingParameters()
        {
            var res = ParameterTracker.Parameters
                    .Where(p => !DefinitionsTracker.Definitions.Any(d => d.Parameters.Contains(p)));
            return res;
        }

        private void RebuildList()
        {
            scroll.Clear();

            var sortedParameters = definition.Parameters.ToList();
            sortedParameters.RemoveAll(param => param.Equals(null));
            sortedParameters.Sort((x, y) => x.name.CompareTo(y.name));

            foreach (var parameter in sortedParameters)
            {
                VisualElement paramRow = new VisualElement();
                paramRow.AddToClassList("custom-list-row-container");


                Action removeParameter = () =>
                {
                    serializedObject.UpdateIfRequiredOrScript();

                    for (int i = 0; i < parametersProperty.arraySize; i++)
                    {
                        var prop = parametersProperty.GetArrayElementAtIndex(i);

                        if (Equals(prop.boxedValue, parameter))
                        {
                            parametersProperty.DeleteArrayElementAtIndex(i); // sets to null

                            break;
                        }
                    }

                    serializedObject.ApplyModifiedProperties();
                };

                Button removeBtn = new Button(removeParameter) { text = "-" };

                paramRow.RegisterCallback<MouseDownEvent>(evt => { EditorGUIUtility.PingObject(parameter); });

                paramRow.AddManipulator(new ContextualMenuManipulator((ContextualMenuPopulateEvent callback) =>
                {
                    callback.menu.AppendAction("Select Asset", (x) =>
                    {
                        EditorGUIUtility.PingObject(parameter);                        
                    });

                    callback.menu.AppendAction("Remove Parameter", (x) =>
                    {
                        removeParameter?.Invoke();
                    });

                    callback.menu.AppendAction("Delete Parameter", (x) =>
                    {
                        if(EditorUtility.DisplayDialog("Delete the parameter", $"Are you sure you want to delete the parameter {parameter.name}?", "DELETE", "Cancel"))
                        {
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(parameter));
                        }
                    });

                }));

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
            RebuildList();
        }
    }
}
