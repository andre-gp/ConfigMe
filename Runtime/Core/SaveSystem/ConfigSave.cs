using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine;

namespace ConfigMe
{
    public class ConfigSave : MonoBehaviour
    {
        [SerializeField] SaveInformation saveInformation;

        public void LoadObject(FileLoadContext fileLoadContext)
        {
            if (File.Exists(saveInformation.GetSavePath()))
            {
                fileLoadContext.LoadedObject = JObject.Parse(File.ReadAllText(saveInformation.GetSavePath()));
                fileLoadContext.LoadResult = LoadResult.Success;
            }
            else
            {
                fileLoadContext.LoadedObject = null;
                fileLoadContext.LoadResult = LoadResult.FileNotFound;
                ConfigMeLogger.LogInfo("Save file not found. Using default settings.");
            }
        }

        public void SaveFile(JObject objToSave)
        {
            File.WriteAllText(saveInformation.GetSavePath(), objToSave.ToString(Formatting.Indented));

            ConfigMeLogger.LogInfo($"Saved settings on disk!\n{objToSave.ToString(Formatting.Indented)}");
        }

    }
}
