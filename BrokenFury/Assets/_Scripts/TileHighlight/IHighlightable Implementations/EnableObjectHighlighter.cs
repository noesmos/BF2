using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{
    public class EnableObjectHighlighter : MonoBehaviour, IHighlightable
    {
        private MeshRenderer mr;

        private void Awake()
        {
            mr = GetComponent<MeshRenderer>();
            mr.enabled = false;
        }

        public void Highlight(string highlightType)
        {
            mr.enabled = true;
        }

        public void UnHighlight()
        {
            mr.enabled = false;
        }


    }
}


