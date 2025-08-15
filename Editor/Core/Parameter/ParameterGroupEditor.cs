using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    [CustomPropertyDrawer(typeof(ParameterGroup))]
    public class ParameterGroupEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            ParameterGroup categoryDefinition = property.boxedValue as ParameterGroup;

            VisualElement root = new VisualElement();
            root.style.marginTop = 5;
            root.style.marginBottom = 10;
            root.style.SetBorderColor(Color.gray);
            root.style.SetBorderWidth(1f);
            root.style.SetBorderRadius(10f);
            root.style.SetPadding(10, 10, 20, 20);

            TextField textField = new TextField();
            textField.style.paddingBottom = 10;
            textField.textEdition.placeholder = "Category Name";
            textField.BindProperty(property.FindPropertyRelative("categoryName"));
            root.Add(textField);

            PropertyField parameters = new PropertyField(property.FindPropertyRelative("parameters"));
            root.Add(parameters);

            return root;
        }
    }
}
