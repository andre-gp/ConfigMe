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
            root.style.borderRightColor = Color.gray;
            root.style.borderLeftColor = Color.gray;
            root.style.borderTopColor = Color.gray;
            root.style.borderBottomColor = Color.gray;
            root.style.borderRightWidth = 1;
            root.style.borderLeftWidth = 1;
            root.style.borderTopWidth = 1;
            root.style.borderBottomWidth = 1;
            root.style.borderBottomLeftRadius = 10;
            root.style.borderBottomRightRadius = 10;
            root.style.borderTopLeftRadius = 10;
            root.style.borderTopRightRadius = 10;
            root.style.paddingBottom = 10;
            root.style.paddingTop = 10;
            root.style.paddingLeft = 20;
            root.style.paddingRight = 20;

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
