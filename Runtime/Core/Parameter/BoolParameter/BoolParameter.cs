using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace ConfigMe
{
    public abstract class BoolParameter : Parameter
    {
        [SerializeField] bool defaultValue;

        [SerializeField] string labelWhenFalse;
        [SerializeField] string labelWhenTrue;

        Action<bool> setComponentsWithoutNotify;

        bool currentValue;

        public abstract void ApplyValue(bool value);

        public override void InitParameter()
        {
            base.InitParameter();

            this.currentValue = defaultValue;
        }

        public override void DisposeParameter()
        {
            
        }

        protected override void InitElement(VisualElement root)
        {
            FillBaseField(root);

            int defaultIndex = defaultValue ? 1 : 0;

            ChoicesParameterData optionsParameterData
                = new ChoicesParameterData(parameterName, new List<string>() { labelWhenFalse, labelWhenTrue }, defaultIndex);

            FillDropdown(root, optionsParameterData);

            FillStepper(root, optionsParameterData);
        }


        public override void SetWithoutNotify(object obj)
        {
            setComponentsWithoutNotify?.Invoke((bool)obj);
        }
        
        public override object GetCurrentValue()
        {
            return currentValue;
        }

        public override void ApplyValue(object obj)
        {
            ApplyValue((bool)obj);
        }
        
        void FillBaseField(VisualElement root)
        {
            var toggle = root.Q<BaseField<bool>>();

            if (toggle == null)
                return;

            toggle.label = this.parameterName;
            toggle.value = defaultValue;

            toggle.RegisterValueChangedCallback(evt => ValueChanged(evt.newValue));

            setComponentsWithoutNotify += evt => { toggle.SetValueWithoutNotify(evt); };
        }

        void FillDropdown(VisualElement root, ChoicesParameterData data)
        {
            var dropdown = root.Q<DropdownField>();

            if (dropdown == null)
                return;

            dropdown.label = data.label;
            dropdown.choices = data.choices;
            dropdown.index = data.defaultValue;

            dropdown.RegisterValueChangedCallback(evt =>
            {
                ValueChanged(dropdown.index == 1);                
            });

            setComponentsWithoutNotify += evt => { dropdown.SetValueWithoutNotify(dropdown.choices[evt == true ? 1 : 0]); };
        }

        void FillStepper(VisualElement root, ChoicesParameterData data)
        {
            var stepper = root.Q<Stepper>();

            if (stepper == null)
                return;

            stepper.label = data.label;
            stepper.Choices = data.choices;
            stepper.value = data.defaultValue;

            stepper.RegisterValueChangedCallback(evt =>
            {
                ValueChanged(stepper.value == 1);
            });

            setComponentsWithoutNotify += evt => { stepper.SetValueWithoutNotify(evt == true ? 1 : 0); };
        }

    }

}
