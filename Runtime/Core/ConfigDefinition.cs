using System.Collections.Generic;
using UnityEngine;

namespace ConfigMe
{
    public class ConfigDefinition : ScriptableObject
    {
        [SerializeField] string saveName = "settings.txt";

        [SerializeField] List<Parameter> parameters = new List<Parameter>();
        public IReadOnlyList<Parameter> Parameters => parameters;
    }
}
