using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine.Events;

namespace ConfigMe
{
    public class ConfigManager : MonoBehaviour
    {
        [SerializeField] UnityEvent<JObject> LoadObject;

        [SerializeField] UnityEvent<JObject> SaveObject;

        [SerializeField] Parameter[] parameters;

        JObject currentSettings;

        JObject appliedSettings;

        private void Awake()
        {
            foreach (var parameter in parameters)
            {
                parameter.InitParameter();

                parameter.OnValueChanged += OnParameterChanged;
            }

            LoadSettings();
        }

        private void OnParameterChanged(Parameter changedParameter, object value)
        {
            changedParameter.ApplyValue(value);

            // Checks for other parameters with the same key and updates them silently with the new value.
            foreach (var parameter in parameters)
            {
                if(parameter.SaveKey == changedParameter.SaveKey)
                {
                    parameter.SetWithoutNotify(value);
                }
            }

            currentSettings[changedParameter.SaveKey] = value.ToString();
        }

        private void LoadSettings()
        {
            currentSettings = null;

            LoadObject?.Invoke(currentSettings);

            if (currentSettings == null)
            {
                Debug.Log("[ConfigMe] Save file not found. Creating default settings.");

                currentSettings = GenerateSettings();

                SaveOnDisk(currentSettings);
            }

            ApplySettings(currentSettings);
        }

        private void SaveOnDisk(JObject settings)
        {
            SaveObject?.Invoke(settings);
        }

        private void Start()
        {
            
        }

        public JObject GenerateSettings()
        {
            JObject settings = new JObject();

            foreach (var parameter in parameters)
            {
                settings[parameter.SaveKey] = parameter.GetCurrentValue().ToString();
            }

            Debug.Log(settings.ToString());

            return settings;
        }

        public void ApplySettings(JObject json)
        {
            foreach (var parameter in parameters)
            {
                parameter.ApplyValue(json);
            }

            appliedSettings = new JObject(json);
        }

        private void OnDestroy()
        {
            foreach (var parameter in parameters)
            {
                parameter.DisposeParameter();
            }
        }
    }
}
