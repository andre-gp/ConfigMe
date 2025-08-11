using UnityEngine;

namespace ConfigMe
{
    public class ConfigMeDefinition : ScriptableObject
    {
        [SerializeField] Parameter[] parameters;
    }
}
