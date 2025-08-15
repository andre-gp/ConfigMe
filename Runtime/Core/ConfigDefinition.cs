using UnityEngine;

namespace ConfigMe
{
    public class ConfigDefinition : ScriptableObject
    {
        [SerializeField] string saveName = "settings.txt";

        [SerializeField] Parameter[] parameters;
    }
}
