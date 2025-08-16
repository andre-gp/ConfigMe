using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ConfigMe.EditorCM
{
    public class ParameterTracker : AssetPostprocessor
    {
        [InitializeOnLoad]
        public static class ParametersTrackerInitializer
        {
            static ParametersTrackerInitializer()
            {
                ForceRefreshParameters();
            }
        }

        private static List<Parameter> parameters = new List<Parameter>();
        public static IReadOnlyList<Parameter> Parameters => parameters;

        public static event Action<List<Parameter>> OnChangeParameters = null;

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            for (int i = 0; i < importedAssets.Length; i++)
            {
                var parameter = GetParameter(importedAssets[i]);

                if (parameter != null)
                {
                    parameters.Add(parameter);

                    OnChangeParameters?.Invoke(parameters);
                }
            }

            if (deletedAssets.Length > 0)
            {
                int removedCount = parameters.RemoveAll(def => def == null);

                if (removedCount > 0)
                {
                    OnChangeParameters?.Invoke(parameters);
                }
            }
        }

        private static void ForceRefreshParameters()
        {
            var parametersGUIDs = AssetDatabase.FindAssets("t:ConfigMe.Parameter");

            parameters = parametersGUIDs.Select(guid => AssetDatabase.LoadAssetAtPath<Parameter>(AssetDatabase.GUIDToAssetPath(guid))).ToList();

            OnChangeParameters?.Invoke(parameters);
        }

        private static Parameter GetParameter(string path)
        {
            if (!path.EndsWith(".asset"))
                return null;

            return AssetDatabase.LoadAssetAtPath<Parameter>(path);
        }
    }
}
