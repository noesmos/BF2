using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{
    public class Path : MonoBehaviour
    {
        public List<Tile> tiles { get; set; }

        public float movementCost { get; } //implement later

        public int length { get { return tiles.Count-1; } } 

        public Path(List<Tile> tiles)
        {
            this.tiles = tiles;
            this.movementCost = tiles[length].gCost;
        }

        
    }
}
