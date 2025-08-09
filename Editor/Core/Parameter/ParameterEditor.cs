using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    public class ParameterEditor
    {
        public const string DEFAULT_COMPONENTS_SEARCH_FILTER = "glob:\"Packages/com.gaton.config-me/UI/Uxml/DefaultComponents/*.uxml\"";

        const string MISSING_FIELDS_MESSAGE = "Fill in all required fields.";

        public static void AddScriptableObjectField(VisualElement root, ScriptableObject obj)
        {
            ObjectField scriptableField = new ObjectField();
            scriptableField.value = obj;
            scriptableField.enabledSelf = false;
            root.Add(scriptableField);
        }

        public static void AddParameterFields(VisualElement root, SerializedObject serializedObject, string[] allowedDefaultComponents)
        {
            /* --- PARAMETER NAME --- */
            SerializedProperty nameProperty = serializedObject.FindProperty("parameterName");
            PropertyField name = new PropertyField(nameProperty);
            root.Add(name);

            /* --- SAVE KEY --- */
            SerializedProperty saveKeyProperty = serializedObject.FindProperty("saveKey");
            PropertyField saveKey = new PropertyField(saveKeyProperty);
            root.Add(saveKey);

            /* --- UXML COMPONENT --- */
            SerializedProperty componentProperty = serializedObject.FindProperty("component");
            PropertyField componentField = new PropertyField(componentProperty);

            Parameter parameter = serializedObject.targetObject as Parameter;
            VisualTreeAsset selectedComponent = serializedObject.FindProperty("component").boxedValue as VisualTreeAsset;

            List<VisualTreeAsset> defaultComponents = GetDefaultComponents(allowedDefaultComponents);
            int currentChoice = -1;

            for (int i = 0; i < defaultComponents.Count; i++)
            {
                if (defaultComponents[i] == selectedComponent)
                {
                    currentChoice = i;
                }
            }

            List<string> choices = new List<string>();

            choices.AddRange(defaultComponents.Select(x =>
            {
                // Regex to add white spaces to 'CamelCasedNames -> Camel Cased Names'
                return Regex.Replace(x.name.Replace("Parameter", ""), "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
            }));

            choices.Add("Custom");

            if (currentChoice < 0)
            {
                currentChoice = choices.Count - 1;
            }

            DropdownField field = new DropdownField("Component Type", choices, currentChoice);

            Action<int> OnUpdateFieldValue = newIndex =>
            {
                componentField.style.display = newIndex == choices.Count - 1 ? DisplayStyle.Flex : DisplayStyle.None;

                if (newIndex == choices.Count - 1) // If Selecting Custom Component
                {                    
                    VisualTreeAsset selectedComponent = serializedObject.FindProperty("component").boxedValue as VisualTreeAsset;

                    foreach (var defaultComp in defaultComponents)
                    {
                        if (selectedComponent == defaultComp)
                        {
                            componentProperty.objectReferenceValue = null;
                            serializedObject.ApplyModifiedProperties();
                            break;
                        }
                    }

                    return;
                }

                componentProperty.objectReferenceValue = defaultComponents[newIndex];
                serializedObject.ApplyModifiedProperties();
            };

            OnUpdateFieldValue?.Invoke(currentChoice);
            field.RegisterValueChangedCallback(evt => { OnUpdateFieldValue?.Invoke(field.index); });

            root.Add(field);
            root.Add(componentField);


            HelpBox warningNoSaveKey = new HelpBox(MISSING_FIELDS_MESSAGE, HelpBoxMessageType.Warning);
            warningNoSaveKey.Q<Label>().style.fontSize = 12;
            root.Add(warningNoSaveKey);

            Action UpdateWarningVisibility = () =>
            {
                bool showWarning = IsMissingInput(nameProperty, saveKeyProperty, componentProperty);
                warningNoSaveKey.style.display = showWarning ? DisplayStyle.Flex : DisplayStyle.None;
            };

            UpdateWarningVisibility();

            name.RegisterValueChangeCallback(evt => { UpdateWarningVisibility(); });
            saveKey.RegisterValueChangeCallback(evt => { UpdateWarningVisibility(); });
            componentField.RegisterValueChangeCallback(evt => { UpdateWarningVisibility(); });
        }

        private static List<VisualTreeAsset> GetDefaultComponents(string[] allowedDefaultComponents)
        {
            string[] defaultComponentsFilters = new string[] { DEFAULT_COMPONENTS_SEARCH_FILTER };

            var defaultComponents = new List<VisualTreeAsset>();

            for (int i = 0; i < defaultComponentsFilters.Length; i++)
            {
                var guids = AssetDatabase.FindAssets(defaultComponentsFilters[i]);
                var paths = guids.Select(x => AssetDatabase.GUIDToAssetPath(x));

                foreach (var path in paths)
                {
                    var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);

                    if (allowedDefaultComponents.Contains(asset.name))
                    {
                        defaultComponents.Add(asset);
                    }
                }
            }

            return defaultComponents;
        }

        static bool IsMissingInput(SerializedProperty name, SerializedProperty saveKey, SerializedProperty comp)
        {
            return string.IsNullOrWhiteSpace(name.stringValue) ||
                   string.IsNullOrWhiteSpace(saveKey.stringValue) ||
                   comp.objectReferenceValue == null;
        }

        /// <summary>
        /// Uses reflection to collect all properties from the type and its ancestors,
        /// then skips drawing any serialized property that matches those fields.
        /// <para>Only fields declared in subclasses are drawn here.</para>
        /// </summary>
        public static void AddDerivedClassesProperties(VisualElement root, SerializedObject serializedObject, Type type)
        {
            var propertiesToSkip = type
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Select(f => f.Name)
                .ToHashSet();

            // Skip the script reference
            propertiesToSkip.Add("m_Script");

            var iterator = serializedObject.GetIterator();

            if (iterator.NextVisible(true))
            {
                while (iterator.NextVisible(false))
                {
                    if (propertiesToSkip.Contains(iterator.name))
                        continue;

                    PropertyField derivedField = new PropertyField(iterator.Copy());
                    root.Add(derivedField);
                }
            }
        }

        public static VisualElement GetSpecialGroupElement()
        {
            VisualElement group = new VisualElement();

            group.style.marginTop = 10;
            group.style.marginBottom = 10;
            group.style.paddingRight = 10;
            group.style.paddingLeft = 10;
            group.style.paddingTop = 10;
            group.style.paddingBottom = 10;
            group.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);

            group.style.borderBottomLeftRadius = 10;
            group.style.borderBottomRightRadius = 10;
            group.style.borderTopLeftRadius = 10;
            group.style.borderTopRightRadius = 10;

            return group;
        }
    }
}
