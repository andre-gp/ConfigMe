using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace ConfigMe
{
    public abstract class ChoicesParameter<T> : BaseChoicesParameter
    {
        [SerializeField] List<LabeledValue<T>> labeledValues = new List<LabeledValue<T>>();

        Action<T> setComponentsWithoutNotify;

        public override List<string> GetChoices { get => labeledValues.Select(x => x.label).ToList(); }

        public abstract void ApplyValue(T value);

        public override void InitParameter()
        {
            base.InitParameter();

            setComponentsWithoutNotify = null;

            this.currentIndex = defaultOption;
        }

        public override void DisposeParameter()
        {
            
        }

        public override void ApplyValue(JObject jObj)
        {            
            ApplyValue(jObj[saveKey].ToObject<T>());
        }

        public override void SetWithoutNotify(JObject jObj)
        {
            T value = jObj[saveKey].ToObject<T>();

            currentIndex = labeledValues.FindIndex(x => x.value.Equals(value));

            if (currentIndex < 0)
            {
                ConfigMeLogger.LogError($"Couldn't find the option <b>[{value}]</b> in the <b>[{name}]</b> choices", this);
            }

            setComponentsWithoutNotify?.Invoke(value);
        }

        protected override void InitElement(VisualElement root)
        {
            ChoicesParameterData data = new ChoicesParameterData(parameterName, GetChoices, this.currentIndex);

            FillPopupField(root, data);

            FillStepper(root, data);
        }

        public override object GetCurrentValue()
        {
            return labeledValues[currentIndex].value;
        }

        public void FillPopupField(VisualElement root, ChoicesParameterData data)
        {
            var dropdown = root.Q<PopupField<string>>();

            if (dropdown == null)
                return;

            dropdown.label = data.label;
            dropdown.choices = data.choices;
            dropdown.index = data.currentValue;

            dropdown.RegisterValueChangedCallback(evt =>
            {
                ValueChanged(labeledValues[dropdown.index].value);                
            });


            setComponentsWithoutNotify += val =>
            {
                int index = GetValueIndex(val);

                dropdown.SetValueWithoutNotify(dropdown.choices[index]);
            };
        }



        public void FillStepper(VisualElement root, ChoicesParameterData data)
        {
            var stepper = root.Q<Stepper>();

            if (stepper == null)
                return;

            stepper.label = data.label;
            stepper.Choices = data.choices;
            stepper.value = data.currentValue;

            stepper.RegisterValueChangedCallback(evt =>
            {
                ValueChanged(labeledValues[stepper.value].value);
            });

            setComponentsWithoutNotify += val =>
            {
                stepper.SetValueWithoutNotify(GetValueIndex(val));
            };
        }
        private int GetValueIndex(T val)
        {
            return labeledValues.FindIndex(x => x.value.Equals(val));
        }

    }
}
