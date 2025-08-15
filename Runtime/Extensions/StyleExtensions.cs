using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe
{
    public static class StyleExtensions
    {
        public static void SetMargin(this IStyle style, float value)
        {
            SetMargin(style, value, value, value, value);
        }

        public static void SetMargin(this IStyle style, float top, float bottom, float left, float right)
        {
            style.marginTop = top;
            style.marginBottom = bottom;
            style.marginLeft = left;
            style.marginRight = right;
        }

        public static void SetPadding(this IStyle style, float value)
        {
            SetPadding(style, value, value, value, value);
        }

        public static void SetPadding(this IStyle style, float top, float bottom, float left, float right)
        {
            style.paddingTop = top;
            style.paddingBottom= bottom;
            style.paddingLeft = left;
            style.paddingRight = right;
        }

        public static void SetBorderWidth(this IStyle style, float value)
        {
            SetBorderWidth(style, value, value, value, value);
        }

        public static void SetBorderWidth(this IStyle style, float top, float bottom, float left, float right)
        {
            style.borderTopWidth = top;
            style.borderBottomWidth = bottom;
            style.borderLeftWidth = left;
            style.borderRightWidth = right;
        }

        public static void SetBorderColor(this IStyle style, Color value)
        {
            SetBorderColor(style, value, value, value, value);
        }

        public static void SetBorderColor(this IStyle style, Color top, Color bottom, Color left, Color right)
        {
            style.borderTopColor = top;
            style.borderBottomColor = bottom;
            style.borderLeftColor = left;
            style.borderRightColor = right;
        }

        public static void SetBorderRadius(this IStyle style, float value)
        {
            SetBorderRadius(style, value, value, value, value);
        }

        public static void SetBorderRadius(this IStyle style, float topLeft, float topRight, float bottomLeft, float bottomRight)
        {
            style.borderTopLeftRadius = topLeft;
            style.borderTopRightRadius = topRight;
            style.borderBottomLeftRadius = bottomLeft;
            style.borderBottomRightRadius = bottomRight;
        }
    }
}
