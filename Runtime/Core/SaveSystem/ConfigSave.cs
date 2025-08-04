using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine;

namespace ConfigMe
{
    public class ConfigSave : MonoBehaviour
    {
        [SerializeField] SaveInformation saveInformation;

        private void Start()
        {
            JObject settingsFile = new JObject();

            //settingsFile.Add("myObj", JToken.FromObject(10));

            settingsFile["myObj"] = JToken.FromObject(20);

            string jsonFile = settingsFile.ToString(Formatting.Indented);

            var parsedFile = JObject.Parse(jsonFile);

            Debug.Log((int)parsedFile["myObj"] + 20);

        }

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
        }

    }
}
