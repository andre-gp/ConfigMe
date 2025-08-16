using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConfigMe.EditorCM
{    
    public class ConfigManagerTracker : AssetPostprocessor
    {
        public static event Action<ConfigManager> OnChangeConfigManager = null;

        private static ConfigManager configManager = null;

        public static ConfigManager ConfigManager
        {
            get => configManager;

            set
            {
                configManager = value;

                if (configManager != null && AssetDatabase.TryGetGUIDAndLocalFileIdentifier(configManager, out string guid, out long localID))
                {
                    EditorPrefs.SetString(ConfigMeEditor.KEY_CONFIG_MANAGER_GUID, guid);
                }
                else
                {
                    EditorPrefs.DeleteKey(ConfigMeEditor.KEY_CONFIG_MANAGER_GUID);
                }

                OnChangeConfigManager?.Invoke(configManager);
            }
        }


        [InitializeOnLoad]
        public static class ConfigManagerInitializer
        {
            static ConfigManagerInitializer()
            {
                configManager = LoadConfigManager();

                SetManagerDefinitions(DefinitionsTracker.Definitions);

                DefinitionsTracker.OnChangeDefinitions += SetManagerDefinitions;
            }
        }

        

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (!EditorPrefs.HasKey(ConfigMeEditor.KEY_CONFIG_MANAGER_GUID))
                return;

            // Check if the ConfigManager has been deleted.

            for (int i = 0; i < deletedAssets.Length; i++)
            {
                if (deletedAssets[i].EndsWith(".prefab"))
                {
                    string guid = AssetDatabase.AssetPathToGUID(deletedAssets[0]);

                    if(guid.Equals(EditorPrefs.GetString(ConfigMeEditor.KEY_CONFIG_MANAGER_GUID)))
                    {
                        ConfigManager = null;

                        return;
                    }
                }
            }    
        }

        private static ConfigManager LoadConfigManager()
        {
            if (EditorPrefs.HasKey(ConfigMeEditor.KEY_CONFIG_MANAGER_GUID))
            {
                string guid = EditorPrefs.GetString(ConfigMeEditor.KEY_CONFIG_MANAGER_GUID);

                return AssetDatabase.LoadAssetAtPath<ConfigManager>(AssetDatabase.GUIDToAssetPath(guid));
            }

            return null;
        }

        private static void SetManagerDefinitions(IReadOnlyList<ConfigDefinition> definitions)
        {
            if (configManager == null)
                return;

            // Small delay to ensure AssetDatabase operations run after OnPostprocessAssets has finished
            // There was a thread conflict happening before.
            EditorApplication.delayCall += () =>
            {
                SetDefinitionsValue(definitions.ToArray());

                EditorUtility.SetDirty(configManager);
                AssetDatabase.SaveAssetIfDirty(configManager);
            };
        }

        private static void SetDefinitionsValue(ConfigDefinition[] definitions)
        {
            var defProp = configManager.GetType().GetField("definitions", BindingFlags.NonPublic | BindingFlags.Instance);

            defProp.SetValue(configManager, definitions.ToArray());
        }

        public static void CreateNewConfigManager()
        {
            GameObject go = new GameObject("ConfigManager", typeof(ConfigManager));

            Directory.CreateDirectory(ConfigMeEditor.USER_FOLDER);
            string path = AssetDatabase.GenerateUniqueAssetPath(ConfigMeEditor.DEFAULT_CONFIG_MANAGER_PATH);

            var prefab = PrefabUtility.SaveAsPrefabAsset(go, path);
            ConfigManager = prefab.GetComponent<ConfigManager>();
            SetDefinitionsValue(DefinitionsTracker.Definitions.ToArray());
            EditorGUIUtility.PingObject(prefab);

            GameObject.DestroyImmediate(go);
        }
    }
}
