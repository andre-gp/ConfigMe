using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe
{
    public abstract class Parameter : ScriptableObject
    {
        [Tooltip("The display name of the parameter, shown in the UI.")]
        [SerializeField] protected string parameterName;

        [Tooltip("A unique identifier used to save and retrieve this setting's value from the JSON file.")]
        [SerializeField] protected string saveKey;
        public string SaveKey => this.saveKey;

        [Tooltip("The UXML asset to instantiate.\nThe system will search within it for a matching UI component (e.g., Dropdown, Toggle) to initialize based on the parameter type.")]
        [SerializeField] protected VisualTreeAsset component;


        /// <summary>
        /// Primarly subscribed to by the ConfigManager to detect component changes.
        /// </summary>
        public event Action<Parameter, object> OnValueChanged;
        

        /// <summary>
        /// Override this to initialize parameters that require setup at startup.
        /// </summary>
        public virtual void InitParameter()
        {
            OnValueChanged = null;
            Debug.Log($"Initialized: {parameterName}");
        }

        /// <summary>
        /// Called when the game ends. Use this to clean up or reset any data related to the parameter.
        /// </summary>
        public abstract void DisposeParameter();

        /// <summary>
        /// Tries to find a suitable UI element in the given root and sets it up with this parameter's information.
        /// </summary>
        protected abstract void InitElement(VisualElement root);

        public abstract object GetCurrentValue();
        public abstract void ApplyValue(JObject jObj);
        public abstract void SetWithoutNotify(JObject jObj);

        public VisualElement InstantiateElement()
        {
            VisualElement root = component.Instantiate();
            InitElement(root);
            return root;
        }

        protected void ValueChanged(object newValue)
        {
            OnValueChanged?.Invoke(this, newValue);
        }

    }
}
