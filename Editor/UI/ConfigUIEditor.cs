using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    [CustomEditor(typeof(ConfigUI))]
    public class ConfigUIEditor : Editor
    {
        List<ConfigDefinition> definitions;

        ConfigUI configUI;

        public override VisualElement CreateInspectorGUI()
        {
            configUI = target as ConfigUI;

            VisualElement root = new VisualElement();

            root.Add(new PropertyField(serializedObject.FindProperty("tabButton")));
            root.Add(new PropertyField(serializedObject.FindProperty("parameterListTemplate")));
            root.Add(new PropertyField(serializedObject.FindProperty("definitionsToShow")));

            definitions = ConfigManagerTracker.ConfigManager.Definitions.ToList();

            ListView list = new ListView(definitions, 20, MakeItem, BindItem);
            list.reorderable = true;
            list.reorderMode = ListViewReorderMode.Animated;
            list.itemIndexChanged += IndexChanged;
            root.Add(list);

            return root;
        }

        private void IndexChanged(int oldIndex, int newIndex)
        {
            ConfigDefinition definition = configUI.definitionsToShow[oldIndex];
            configUI.definitionsToShow.RemoveAt(oldIndex);
            configUI.definitionsToShow.Insert(newIndex, definition);

            Debug.Log(oldIndex + " " + newIndex);
        }

        private void BindItem(VisualElement root, int index)
        {
            ConfigDefinition definition = definitions[index];

            Toggle toggle = root.Q<Toggle>();
            toggle.value = configUI.definitionsToShow.Contains(definition);
            toggle.RegisterValueChangedCallback((evt) =>
            {
                if (evt.newValue)
                {
                    if (!configUI.definitionsToShow.Contains(definition))
                    {
                        int indexToInsert = index;

                        configUI.definitionsToShow.Insert(Mathf.Clamp(index, 0, configUI.definitionsToShow.Count), definition);
                    }                        
                }
                else
                {
                    configUI.definitionsToShow.Remove(definition);
                }
            });

            root.Q<ObjectField>().value = definitions[index];
        }

        VisualElement MakeItem()
        {
            VisualElement root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;

            root.Add(new Toggle());

            ObjectField objField = new ObjectField();
            objField.enabledSelf = false;
            root.Add(objField);

            return root;
        }
    }
}
