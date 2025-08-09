using ConfigMe.EditorCM.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    [CustomEditor(typeof(BoolParameter), true)]
    public class BoolParameterEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            BoolParameter boolParameter = target as BoolParameter;

            VisualElement root = new VisualElement();

            /* --- SCRIPTABLE OBJ --- */
            ParameterEditor.AddScriptableObjectField(root, boolParameter);

            root.Add(new Separator());

            /* --- PARAMETER FIELDS --- */
            ParameterEditor.AddParameterFields(root, serializedObject, new string[] { ConfigMeEditor.COMPONENT_TOGGLE, 
                ConfigMeEditor.COMPONENT_SLIDE_TOGGLE, ConfigMeEditor.COMPONENT_INT_SLIDER,             
                ConfigMeEditor.COMPONENT_STEPPER, ConfigMeEditor.COMPONENT_DROPDOWN});

            return root;
        }
    }
}
