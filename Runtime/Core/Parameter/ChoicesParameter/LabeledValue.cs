using UnityEngine;

namespace ConfigMe
{
    [System.Serializable]
    public class LabeledValue<T>
    {
        public string label;
        public T value;
    }
}
