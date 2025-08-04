using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.NavigationMoveEvent;

namespace ConfigMe
{
    [UxmlElement]
    public partial class Stepper : BaseField<int>
    {
        public static readonly new string ussClassName = "stepper";

        public static readonly string ussLabelClassName = ussClassName + "__label";

        public static readonly string ussInputClassName = ussClassName + "__input";

        public static readonly string ussChoiceLabelClassName = ussClassName + "__choice-label";

        public static readonly string ussPositionIndicatorGroup = ussClassName + "__position-indicator-group";
        public static readonly string ussPositionIndicator = ussClassName + "__position-indicator";
        public static readonly string ussPositionIndicatorActive = ussClassName + "__position-indicator--active";

        private List<String> choices;

        [Header("Stepper")]

        [UxmlAttribute]
        public List<string> Choices
        {
            get => this.choices;

            set
            {
                this.choices = value;

                for (int i = positionIndicators.Count; i < choices.Count; i++)
                {
                    PositionIndicator newPosIndicator = CreatePositionIndicator();

                    newPosIndicator.Index = i;
                    positionIndicatorGroup.Add(newPosIndicator);
                    positionIndicators.Add(newPosIndicator);
                }

                for (int i = 0; i < positionIndicators.Count; i++)
                {
                    positionIndicators[i].style.display = i < Choices.Count ? DisplayStyle.Flex : DisplayStyle.None;
                }


                SetValueWithoutNotify(this.value);
            }
        }

        VisualElement visualInputRoot;

        Label choiceLabel;

        VisualElement positionIndicatorGroup;

        List<PositionIndicator> positionIndicators;

        public Stepper() : this(null) { }

        public Stepper(string label) : base(label, new VisualElement())
        {
            positionIndicators = new List<PositionIndicator>();

            AddToClassList(ussClassName);

            labelElement.AddToClassList(ussLabelClassName);

            visualInputRoot = this.Q<VisualElement>(className: inputUssClassName);
            visualInputRoot.AddToClassList(ussInputClassName);


            choiceLabel = new Label();
            choiceLabel.AddToClassList(ussChoiceLabelClassName);
            visualInputRoot.Add(choiceLabel);

            positionIndicatorGroup = new VisualElement();
            positionIndicatorGroup.AddToClassList(ussPositionIndicatorGroup);
            visualInputRoot.Add(positionIndicatorGroup);

            //TODO: Show arrow on mouse enter/leave
            //RegisterCallback<MouseEnterEvent>(evt => );

            //RegisterCallback<MouseLeaveEvent>(evt => );

            RegisterCallback<NavigationMoveEvent>(OnMove);
        }

        public void OnMove(NavigationMoveEvent evt)
        {
            if (evt.direction is Direction.Up or Direction.Down)
            {
                return;
            }

            int modifier = evt.direction is Direction.Right ? 1 : -1;

            value = Mathf.Clamp(value + modifier, 0, Choices.Count - 1);

            focusController.IgnoreEvent(evt);
        }

        public override void SetValueWithoutNotify(int newValue)
        {
            base.SetValueWithoutNotify(newValue);

            if (Choices == null || Choices.Count <= 0)
            {
                return;
            }

            RefreshPositionIndicator(newValue);

            choiceLabel.text = Choices[Mathf.Clamp(newValue, 0, Choices.Count - 1)];
        }

        private void RefreshPositionIndicator(int newValue)
        {
            for (int i = 0; i < positionIndicators.Count; i++)
            {
                positionIndicators[i].EnableInClassList(ussPositionIndicatorActive, newValue == i);
            }
        }

        private PositionIndicator CreatePositionIndicator()
        {
            PositionIndicator posIndicator = new PositionIndicator();
            posIndicator.AddToClassList(ussPositionIndicator);
            posIndicator.RegisterCallback<ClickEvent, PositionIndicator>(OnClickStepper, posIndicator);

            return posIndicator;
        }

        private void OnClickStepper(ClickEvent evt, PositionIndicator posIndicator)
        {
            this.value = posIndicator.Index;
        }
    }
}
