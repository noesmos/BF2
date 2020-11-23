using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{
    public class PathfindingDebugging : MonoBehaviour
    {
        public Tile from, to;
        Path path;

        List<Tile> allTiles;
        List<Tile> notPathTiles;

        private void Start()
        {
            allTiles = new List<Tile>();
            notPathTiles = new List<Tile>();
            foreach (GameObject go in MapGenerator.worldLocal)
            {
                allTiles.Add(go.GetComponent<Tile>());
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (from != null && to != null)
            {
                path = Pathfinding.FindPath( from, to);

                foreach (Tile t in allTiles)
                {
                    if (!path.tiles.Contains(t))
                    {
                        notPathTiles.Add(t);
                    }
                }

                HighlightManager.UnHighlightTileList(notPathTiles);
                HighlightManager.HighlightTileList(path.tiles);
                notPathTiles.Clear();
            }

        }
    }
}

