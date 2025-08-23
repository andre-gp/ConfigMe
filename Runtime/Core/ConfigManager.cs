using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine.Events;
using System.Collections.Generic;

namespace ConfigMe
{
    public class ConfigManager : MonoBehaviour
    {
        public static ConfigManager s_instance;

        [SerializeField] UnityEvent<FileLoadContext> LoadObject;

        [SerializeField] UnityEvent<JObject> SaveObject;

        [SerializeField] ConfigDefinition[] definitions;
        public ConfigDefinition[] Definitions => definitions;

        [SerializeField] List<Parameter> parameters;

        JObject currentSettings;

        JObject appliedSettings;

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
            }
            else
            {
                Destroy(this.gameObject);
                ConfigMeLogger.LogError($"Multiple {nameof(ConfigManager)} instances detected. Only one instance is allowed. Removing the extra one.");
            }

            foreach (var def in definitions)
            {
                parameters.AddRange(def.Parameters);
            }

            foreach (var parameter in parameters)
            {
                parameter.InitParameter();

                parameter.OnValueChanged += OnParameterChanged;
            }

            LoadSettings();
        }

        private void Start()
        {
            ApplySettings(currentSettings);

            appliedSettings = new JObject(currentSettings);
        }

        private void OnParameterChanged(Parameter changedParameter, object value)
        {
            currentSettings[changedParameter.SaveKey] = value.ToString();

            //if(autoUpdatePreview)
            changedParameter.ApplyValue(currentSettings);

            // Checks for other parameters with the same key and updates them silently with the new value.
            foreach (var parameter in parameters)
            {
                if (parameter.SaveKey == changedParameter.SaveKey)
                {
                    parameter.SetWithoutNotify(currentSettings);
                }
            }

        }

        private void LoadSettings()
        {
            FileLoadContext loadContext = new FileLoadContext();
            LoadObject?.Invoke(loadContext);

            currentSettings = loadContext.LoadedObject;

            if (currentSettings == null)
            {
                currentSettings = GenerateSettings();

                SaveOnDisk(currentSettings);
            }

            SetAllWithoutNotify(currentSettings);
        }

        private void SaveOnDisk(JObject settings)
        {
            SaveObject?.Invoke(settings);
        }

        private void SetAllWithoutNotify(JObject jObj)
        {
            foreach (var parameter in parameters)
            {
                parameter.SetWithoutNotify(jObj);
            }
        }

        public JObject GenerateSettings()
        {
            JObject settings = new JObject();

            foreach (var parameter in parameters)
            {
                settings[parameter.SaveKey] = parameter.GetCurrentValue().ToString();
            }

            return settings;
        }

        public static void ApplyAndSaveCurrentSettings()
        {
            s_instance.InstanceApplyAndSaveCurrentSettings();
        }

        private void InstanceApplyAndSaveCurrentSettings()
        {
            ApplySettings(currentSettings);

            SaveOnDisk(currentSettings);

            appliedSettings = new JObject(currentSettings);
        }

        public void RevertCurrentSettings()
        {
            currentSettings = new JObject(appliedSettings);

            ApplySettings(currentSettings);
        }

        public void ApplySettings(JObject json)
        {
            HashSet<string> appliedKeys = new HashSet<string>();

            foreach (var parameter in parameters)
            {
                if (!appliedKeys.Contains(parameter.SaveKey))
                {
                    parameter.ApplyValue(json);

                    appliedKeys.Add(parameter.SaveKey);
                }
            }
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
