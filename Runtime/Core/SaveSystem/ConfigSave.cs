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
                objToPopulate = JObject.Parse(File.ReadAllText(saveInformation.GetSavePath()));                
            }            
        }

        public void SaveFile(JObject objToSave)
        {
            File.WriteAllText(saveInformation.GetSavePath(), objToSave.ToString(Formatting.Indented));

            Debug.Log($"[ConfigMe] Saved settings on disk!\n{objToSave.ToString(Formatting.Indented)}");
        }

    }
}
