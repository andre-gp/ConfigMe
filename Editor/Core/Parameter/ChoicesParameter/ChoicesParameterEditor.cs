using ConfigMe.EditorCM.UIElements;
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

            /* --- SCRIPTABLE OBJ --- */
            ParameterEditor.AddScriptableObjectField(root, optionsParameter);

            root.Add(new Separator());

            /* --- PARAMETER FIELDS --- */
            ParameterEditor.AddParameterFields(root, serializedObject, new string[] { ConfigMeEditor.COMPONENT_STEPPER, ConfigMeEditor.COMPONENT_DROPDOWN });

            //root.Add(new Separator());

            /* --- DEFAULT OPTION GROUP --- */
            VisualElement defaultOptionGroup = ParameterEditor.GetSpecialGroupElement();
            root.Add(defaultOptionGroup);

            Label defaultOptionTitle = new Label("Default Option");
            defaultOptionTitle.style.marginLeft = 2;
            //defaultOptionTitle.style.color = new Color(0.1f, 0.1f, 0.1f);
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

            root.Add(new Separator());

            /* --- DRAW DERIVED CLASSES PROPERTIES --- */
            ParameterEditor.AddDerivedClassesProperties(root, serializedObject, typeof(ChoicesParameter<>));



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
