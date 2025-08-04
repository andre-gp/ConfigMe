using System.Collections.Generic;
using UnityEngine;

namespace ConfigMe
{
    public abstract class BaseChoicesParameter : Parameter
    {
        [SerializeField] protected int defaultOption = 0;

        protected int currentIndex;

        public abstract List<string> GetChoices { get; }
    }
}
