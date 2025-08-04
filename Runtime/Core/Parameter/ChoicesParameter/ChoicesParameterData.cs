using System.Collections.Generic;
using UnityEngine;

namespace ConfigMe
{
    public class ChoicesParameterData
    {
        public string label;
        public List<string> choices;
        public int defaultValue;

        public ChoicesParameterData(string label, List<string> choices, int defaultValue)
        {
            this.label = label;
            this.choices = choices;
            this.defaultValue = defaultValue;
        }
    }
}
