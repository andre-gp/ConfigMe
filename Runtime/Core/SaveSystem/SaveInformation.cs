using System.IO;
using UnityEngine;

namespace ConfigMe
{
    [CreateAssetMenu(fileName = "Save Information", menuName = "ConfigMe/Save/Save Information")]
    public class SaveInformation : ScriptableObject
    {
        [SerializeField] protected string savePath;

        public virtual string GetSavePath()
        {
            return Path.Combine(Application.persistentDataPath, savePath); 
        }
    }
}
