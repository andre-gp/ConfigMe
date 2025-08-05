using Newtonsoft.Json.Linq;
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

            int defaultIndex = ParseToInt(currentValue);

            ChoicesParameterData optionsParameterData
                = new ChoicesParameterData(parameterName, new List<string>() { labelWhenFalse, labelWhenTrue }, defaultIndex);

            FillDropdown(root, optionsParameterData);

            FillStepper(root, optionsParameterData);

            FillSliderInt(root);
        }


        public override void SetWithoutNotify(JObject jObj)
        {
            currentValue = jObj[saveKey].ToObject<bool>();
            setComponentsWithoutNotify?.Invoke(currentValue);
        }
        
        public override object GetCurrentValue()
        {
            return currentValue;
        }

        public override void ApplyValue(JObject jObj)
        {
            ApplyValue(jObj[saveKey].ToObject<bool>());
        }
        
        void FillBaseField(VisualElement root)
        {
            var toggle = root.Q<BaseField<bool>>();

            if (toggle == null)
                return;

            toggle.label = this.parameterName;
            toggle.value = currentValue;

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
            dropdown.index = data.currentValue;

            dropdown.RegisterValueChangedCallback(evt =>
            {
                ValueChanged(ParseToBool(dropdown.index));                
            });

            setComponentsWithoutNotify += evt => { dropdown.SetValueWithoutNotify(dropdown.choices[ParseToInt(evt)]); };
        }

        void FillStepper(VisualElement root, ChoicesParameterData data)
        {
            var stepper = root.Q<Stepper>();

            if (stepper == null)
                return;

            stepper.label = data.label;
            stepper.Choices = data.choices;
            stepper.value = data.currentValue;

            stepper.RegisterValueChangedCallback(evt =>
            {
                ValueChanged(ParseToBool(stepper.value));
            });

            setComponentsWithoutNotify += newValue => { stepper.SetValueWithoutNotify(ParseToInt(newValue)); };
        }


        void FillSliderInt(VisualElement root)
        {
            var sliderInt = root.Q<SliderInt>();

            if (sliderInt == null)
                return;

            sliderInt.lowValue = 0;
            sliderInt.highValue = 1;
            sliderInt.value = ParseToInt(currentValue);

            sliderInt.RegisterValueChangedCallback(evt => { ValueChanged(ParseToBool(evt.newValue) ); });

            setComponentsWithoutNotify += newValue =>
            {
                sliderInt.SetValueWithoutNotify(ParseToInt(newValue));
                SetLabel(sliderInt, newValue);
            };

            SetLabel(sliderInt, defaultValue);

            sliderInt.showInputField = false;
        }

        void SetLabel(SliderInt slider, bool value)
        {
            string state = value ? labelWhenTrue : labelWhenFalse;
            slider.label = $"{parameterName} ({state})";
        }

        private int ParseToInt(bool value)
        {
            return value ? 1 : 0;
        }

        private bool ParseToBool(int value)
        {
            return value == 1;
        }
    }

}
