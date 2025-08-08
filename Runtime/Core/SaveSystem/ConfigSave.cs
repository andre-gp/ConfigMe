using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine;

namespace ConfigMe
{
    public class ConfigSave : MonoBehaviour
    {
        [SerializeField] SaveInformation saveInformation;

        public void LoadObject(JObject objToPopulate)
        {
            if (File.Exists(saveInformation.GetSavePath()))
            {
                JObject newObj = JObject.Parse(File.ReadAllText(saveInformation.GetSavePath()));

                // Manually copy values to preserve the original reference of objToPopulate.
                foreach (var item in newObj)
                {
                    objToPopulate[item.Key] = item.Value;
                }
            }
            else
            {
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
