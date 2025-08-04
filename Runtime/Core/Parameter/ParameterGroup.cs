using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe
{
    [System.Serializable]
    public class ParameterGroup
    {
        public string categoryName;

        public Parameter[] parameters;

        Button tabButton;
        public Button TabButton { get => this.tabButton; set => this.tabButton = value; }

        VisualElement parameterList;
        public VisualElement ParameterList { get => this.parameterList; set => this.parameterList = value; }
    }

}
