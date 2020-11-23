using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{
    public class HighlightManager : MonoBehaviour
    {
        //different highlights to make (with different colors/shapes/etc.)
        //they will probably have different priorities (the hover highlight can't override the select highlight)
        //- hover
        //- within range
        //- route
        //- selected

        [SerializeField] private bool highlighted;

        private IHighlightable highligther;

        private void Awake()
        {
            highligther = GetComponentInChildren<IHighlightable>();
        }

        public void HighlightTileAsWithinRange()
        {
            highligther.Highlight("WithinRange");
        }
        public void UnHighlight()
        {
            highligther.UnHighlight();
        }

        public static void HighlightTileList(List<Tile>tileList)
        {
            foreach (Tile t in tileList)
            {
                t.HighlightManager.HighlightTileAsWithinRange();
            }
        }

        public static void UnHighlightTileList(List<Tile> tileList)
        {
            foreach (Tile t in tileList)
            {
                t.HighlightManager.UnHighlight();
            }
        }

    }
}


