using UnityEngine.UIElements;

namespace ConfigMe
{
    public class PositionIndicator : VisualElement
    {
        private int index;

        public int Index
        {
            get => index;
            set
            {
                index = value;
                this.name = $"Position Indicator {index}";
            }
        }
    }
}
