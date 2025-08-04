using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace ConfigMe.EditorCM
{
    [CustomEditor(typeof(BaseChoicesParameter), true)]
    public class ChoicesParameterEditor : Editor
    {
        public const string labeledValuesName = "labeledValues";

        BaseChoicesParameter optionsParameter;

        DropdownField defaultOptionField;

        SerializedProperty defaultOptionProperty;

        public override VisualElement CreateInspectorGUI()
        {
            optionsParameter = target as BaseChoicesParameter;

            VisualElement root = new VisualElement();

            /* --- PARAMETER NAME --- */
            PropertyField name = new PropertyField(serializedObject.FindProperty("parameterName"));
            root.Add(name);

            /* --- SAVE KEY --- */
            PropertyField saveKey = new PropertyField(serializedObject.FindProperty("saveKey"));
            root.Add(saveKey);

            /* --- UXML COMPONENT --- */
            PropertyField component = new PropertyField(serializedObject.FindProperty("component"));
            root.Add(component);


            /* --- DEFAULT OPTION GROUP --- */
            VisualElement defaultOptionGroup = new VisualElement();
            root.Add(defaultOptionGroup);

            defaultOptionGroup.style.marginTop = 10;
            defaultOptionGroup.style.marginBottom = 10;
            defaultOptionGroup.style.paddingRight = 10;
            defaultOptionGroup.style.paddingLeft = 10;
            defaultOptionGroup.style.paddingTop = 10;
            defaultOptionGroup.style.paddingBottom = 10;
            defaultOptionGroup.style.backgroundColor = new Color(0.72f, 0.9f, 1f);

            Label defaultOptionTitle = new Label("<b>Default Option</b>");
            defaultOptionTitle.style.marginLeft = 2;
            defaultOptionTitle.style.color = new Color(0.1f, 0.1f, 0.1f);
            defaultOptionGroup.Add(defaultOptionTitle);

            defaultOptionField = new DropdownField();
            defaultOptionField.choices = optionsParameter.GetChoices;
            defaultOptionField.RegisterValueChangedCallback(OnChangeDefaultOptionDropdown);
            defaultOptionGroup.Add(defaultOptionField);

            defaultOptionProperty = serializedObject.FindProperty("defaultOption");

            OnChangeDefaultOption(defaultOptionProperty);

            defaultOptionField.TrackPropertyValue(defaultOptionProperty, OnChangeDefaultOption);


            /* --- LABELED VALUES --- */
            PropertyField labeledValues = new PropertyField(serializedObject.FindProperty(labeledValuesName));
            labeledValues.TrackPropertyValue(serializedObject.FindProperty(labeledValuesName), OnModifyChoices);


            root.Add(labeledValues);

            return root;
        }

        private void OnModifyChoices(SerializedProperty prop)
        {
            defaultOptionField.choices = optionsParameter.GetChoices;
            defaultOptionField.index = defaultOptionProperty.intValue;
        }

        private void OnChangeDefaultOptionDropdown(ChangeEvent<string> evt)
        {
            defaultOptionProperty.intValue = defaultOptionField.index;

            serializedObject.ApplyModifiedProperties();
        }

        private void OnChangeDefaultOption(SerializedProperty property)
        {
            int defaultValue = property.intValue;

            if (defaultOptionField.index == defaultValue)
                return;

            defaultOptionField.index = defaultValue;
        }
    }
}
