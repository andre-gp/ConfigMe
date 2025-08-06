using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM.UIElements
{
    public class Separator : VisualElement
    {
        public Separator() : this(Color.gray)
        {

        }

        public Separator(Color color)
        {
            style.backgroundColor = color;

            Height = 1;

            Margin = 6f;
        }

        public float Height { set { style.height = value; } }

        public float Margin { set { style.marginTop = value; style.marginBottom = value; } }
    }

}
