using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    public static class CustomList
    {
        public static readonly string ussClassName = "custom-list";

        public static readonly string ussButtonAddClassName = ussClassName + "__button-add";
        public static readonly string ussScrollViewClassName = ussClassName + "__scroll-view";

        public static readonly string ussButtonRemoveClassName = ussClassName + "__button-remove";
        public static readonly string ussLabelClassName = ussClassName + "__label";

        public static VisualElement GenerateCustomList()
        {
            VisualElement customListRoot = new VisualElement();
            customListRoot.styleSheets.Add(ConfigMeEditor.CustomListStyleSheet);
            customListRoot.AddToClassList(ussClassName);

            Button addButton = new Button() { text = "+" };
            addButton.AddToClassList(ussButtonAddClassName);
            customListRoot.Add(addButton);

            ScrollView scrollView = new ScrollView();
            scrollView.AddToClassList(ussScrollViewClassName);
            customListRoot.Add(scrollView);


            return customListRoot;
        }
    }
}
