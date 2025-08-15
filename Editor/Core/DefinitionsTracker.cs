using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ConfigMe.EditorCM
{
    public class DefinitionsTracker : AssetPostprocessor
    {
        [InitializeOnLoad]
        public static class DefinitionsTrackerInitializer
        {
            static DefinitionsTrackerInitializer()
            {
                Debug.Log("Initializing definitions tracker");

                ForceRefreshDefinitions();
            }
        }

        private static List<ConfigDefinition> definitions = new List<ConfigDefinition>();
        public static IReadOnlyList<ConfigDefinition> Definitions => definitions;

        public static event Action<List<ConfigDefinition>> OnChangeDefinitions;

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            for (int i = 0; i < importedAssets.Length; i++)
            {
                var definition = GetDefinition(importedAssets[i]);
                
                if(definition != null)
                {
                    definitions.Add(definition);

                    OnChangeDefinitions?.Invoke(definitions);
                }
            }

            if(deletedAssets.Length > 0)
            {
                int removedCount = definitions.RemoveAll(def => def == null);

                if(removedCount > 0)
                {
                    OnChangeDefinitions?.Invoke(definitions);
                }                

            }
        }

        private static void ForceRefreshDefinitions()
        {
            var definitionsGUIDs = AssetDatabase.FindAssets("t:ConfigDefinition");

            definitions = definitionsGUIDs.Select(guid => AssetDatabase.LoadAssetAtPath<ConfigDefinition>(AssetDatabase.GUIDToAssetPath(guid))).ToList();

            OnChangeDefinitions?.Invoke(definitions);
        }

        private static ConfigDefinition GetDefinition(string path)
        {
            if (!path.EndsWith(".asset"))
                return null;

            return AssetDatabase.LoadAssetAtPath<ConfigDefinition>(path);
        }
    }
}
