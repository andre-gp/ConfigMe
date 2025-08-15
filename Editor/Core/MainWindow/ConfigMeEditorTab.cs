using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    public abstract class ConfigMeEditorTab : IDisposable
    {
        public abstract void InitTab(VisualElement root);

        public abstract void Dispose();
    }
}
