using ConfigMe.EditorCM.UIElements;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    [CustomEditor(typeof(BaseRangeParameter), true)]
    public class RangeParameterEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            BaseRangeParameter baseRangeParameter = target as BaseRangeParameter;

            VisualElement root = new VisualElement();

            /* --- SCRIPTABLE OBJ --- */
            ParameterEditor.AddScriptableObjectField(root, baseRangeParameter);

            root.Add(new Separator());

            /* --- PARAMETER FIELDS --- */
            ParameterEditor.AddParameterFields(root, serializedObject, new string[] { ConfigMeEditor.COMPONENT_FLOAT_SLIDER, ConfigMeEditor.COMPONENT_INT_SLIDER });

            /* --- DEFAULT VALUE --- */
            VisualElement defaultValueGroup = ParameterEditor.GetSpecialGroupElement();
            root.Add(defaultValueGroup);

            Label defaultValueTitle = new Label("Default Value");
            defaultValueTitle.style.marginLeft = 2;
            defaultValueGroup.Add(defaultValueTitle);

            PropertyField defaultValue = new PropertyField(serializedObject.FindProperty("defaultValue"));
            defaultValue.label = "";
            defaultValue.style.width = 100;
            defaultValueGroup.Add(defaultValue);

            /* --- RANGE --- */

            VisualElement range = new VisualElement();
            range.style.marginLeft = 3;
            range.style.flexDirection = FlexDirection.Row;

            root.Add(range);

            Label rangeLabel = new Label("Range");
            rangeLabel.style.alignSelf = Align.Center;
            range.Add(rangeLabel);

            PropertyField lowValue = new PropertyField(serializedObject.FindProperty("lowValue"));
            lowValue.label = "";
            lowValue.style.width = 80;
            range.Add(lowValue);

            Label separator = new Label("-");
            separator.style.marginLeft = 10;
            separator.style.marginRight = 8;
            separator.style.alignSelf = Align.Center;

            range.Add(separator);

            PropertyField highValue = new PropertyField(serializedObject.FindProperty("highValue"));
            highValue.label = "";
            highValue.style.width = 80;
            range.Add(highValue);


            root.Add(new Separator());

            /* --- DRAW DERIVED CLASSES PROPERTIES --- */
            ParameterEditor.AddDerivedClassesProperties(root, serializedObject, typeof(RangeParameter<>));




            return root;
        }
    }
}
