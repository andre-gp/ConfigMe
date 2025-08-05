using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe
{
    public class ConfigUI : MonoBehaviour
    {
        [SerializeField] VisualTreeAsset tabButton;
        [SerializeField] VisualTreeAsset parameterListTemplate;
        [SerializeField] ParameterGroup[] selections;

        UIDocument document;



        private void Start()
        {
            document = GetComponent<UIDocument>();

            var categoriesGroup = document.rootVisualElement.Q<VisualElement>("tabs__button-group");
            var parametersGroup = document.rootVisualElement.Q<VisualElement>("parameters__group");

            for (int i = 0; i < selections.Length; i++)
            {
                ParameterGroup category = selections[i];

                VisualElement tabButtonRoot = this.tabButton.Instantiate();
                categoriesGroup.Add(tabButtonRoot);

                Button tabButton = tabButtonRoot.Q<Button>();
                tabButton.text = category.categoryName;
                category.TabButton = tabButton;
                tabButton.clickable.clicked += () =>
                {
                    OnClickTabButton(category);
                };

                VisualElement parameterList = this.parameterListTemplate.Instantiate();
                parametersGroup.Add(parameterList);
                category.ParameterList = parameterList;

                ScrollView scrollView = parameterList.Q<ScrollView>();

                foreach (var parameter in category.parameters)
                {
                    VisualElement element = parameter.InstantiateElement();
                    //element.AddToClassList(".text__size--small");
                    //element.AddToClassList(".text__font--default");
                    scrollView.Add(element);
                }
            }

            var btn = document.rootVisualElement.Q<Button>("ApplyChangesButton");

            if(btn != null)
            {
                btn.clickable.clicked += () =>
                {
                    GetComponent<ConfigManager>().ApplyAndSaveCurrentSettings();
                };
            }

            OnClickTabButton(selections[0]);
        }


        private void OnClickTabButton(ParameterGroup selectedCategory)
        {
            foreach (var category in selections)
            {
                bool activate = category == selectedCategory;

                category.ParameterList.style.display = activate ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }
}
