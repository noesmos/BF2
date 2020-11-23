using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{
    public interface IHighlightable
    {
        void Highlight(string highlightType);
        void UnHighlight();
    }
}


