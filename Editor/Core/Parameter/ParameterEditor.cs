using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    public class ParameterEditor
    {
        const string MISSING_FIELDS_MESSAGE = "Fill in all required fields.";
        public static void AddParameterFields(VisualElement root, SerializedObject serializedObject)
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
            PropertyField component = new PropertyField(componentProperty);
            root.Add(component);

            

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
            component.RegisterValueChangeCallback(evt => { UpdateWarningVisibility(); });
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
