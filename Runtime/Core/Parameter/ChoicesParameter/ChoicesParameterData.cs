using System.Collections.Generic;
using UnityEngine;

namespace ConfigMe
{
    public class ChoicesParameterData
    {
        public string label;
        public List<string> choices;
        public int currentValue;

        public ChoicesParameterData(string label, List<string> choices, int currentValue)
        {
            this.label = label;
            this.choices = choices;
            this.currentValue = currentValue;
        }
    }
}
