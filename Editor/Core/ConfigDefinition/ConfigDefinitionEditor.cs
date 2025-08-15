using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    [CustomEditor(typeof(ConfigDefinition))]
    public class ConfigDefinitionEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            var configDefinition = target as ConfigDefinition;

            ParameterEditor.AddScriptableObjectField(root, configDefinition);

            SerializedProperty iterator = serializedObject.GetIterator();

            iterator.NextVisible(true); // Steps into the children.

            while (iterator.NextVisible(false))
            {
                var propertyField = new PropertyField(iterator);
                propertyField.Bind(serializedObject);
                root.Add(propertyField);
            }

            return root;
        }
    }
}
