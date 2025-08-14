using UnityEngine;

namespace ConfigMe
{
    public class ConfigDefinition : ScriptableObject
    {
        [SerializeField] string definitionName;

        [SerializeField] Parameter[] parameters;
    }
}
