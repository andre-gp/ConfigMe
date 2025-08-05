using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe
{
    public abstract class RangeParameter<T> : Parameter where T : IComparable<T>
    {
        [SerializeField] T lowValue = default;
        [SerializeField] T highValue = default;
        [SerializeField] T defaultValue = default;

        T currentValue;

        Action<T> setComponentsWithoutNotify;

        protected abstract void ApplyValue(T value);

        public override void InitParameter()
        {
            base.InitParameter();

            this.currentValue = defaultValue;
        }

        public override void DisposeParameter()
        {
            
        }

        public override void ApplyValue(JObject jObj)
        {
            ApplyValue(jObj[saveKey].ToObject<T>());
        }

        protected override void InitElement(VisualElement root)
        {
            var baseSlider = root.Q<BaseSlider<T>>();

            baseSlider.label = this.parameterName;
            baseSlider.lowValue = this.lowValue;
            baseSlider.highValue = this.highValue;
            baseSlider.value = this.currentValue;

            baseSlider.RegisterValueChangedCallback(evt => 
            {
                ValueChanged(evt.newValue);
            });

            setComponentsWithoutNotify += val =>
            {
                baseSlider.SetValueWithoutNotify(val);            
            };
        }

        public override object GetCurrentValue()
        {
            return currentValue;
        }

        public override void SetWithoutNotify(object obj)
        {
            currentValue = ((T)obj);
            setComponentsWithoutNotify?.Invoke(currentValue);
        }
    }
}
