using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe
{
    public class ConfigUI : MonoBehaviour
    {
        [SerializeField] VisualTreeAsset tabButton;
        [SerializeField] VisualTreeAsset parameterListTemplate;

        public List<ConfigDefinition> definitionsToShow;

        ConfigManager manager;

        UIDocument document;

        List<ParameterGroup> groups;

        private void Start()
        {
            manager = FindFirstObjectByType<ConfigManager>();

            document = GetComponent<UIDocument>();

            var categoriesGroup = document.rootVisualElement.Q<VisualElement>("tabs__button-group");
            var parametersGroup = document.rootVisualElement.Q<VisualElement>("parameters__group");

            groups = new List<ParameterGroup>();

            foreach (var definition in manager.Definitions)
            {
                ParameterGroup category = new ParameterGroup();
                groups.Add(category);
                category.categoryName = definition.name;
                category.parameters = new List<Parameter>(definition.Parameters).ToArray();

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
                    VisualElement element = parameter.InstantiateVisualElement();
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
                    ConfigManager.ApplyAndSaveCurrentSettings();
                };
            }

            OnClickTabButton(groups[0]);
        }


        private void OnClickTabButton(ParameterGroup selectedCategory)
        {
            foreach (var category in groups)
            {
                bool activate = category == selectedCategory;

                category.ParameterList.style.display = activate ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }
}
