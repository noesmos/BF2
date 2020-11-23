using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{
    public static class TileSearch 
    {
        //still need to take the movement cost of each tile into account
        public static List<Tile> GetTilesWithinMovement(Unit unit) //get all tiles within a the tile-radius "range" around a given Unit
        {
            return Pathfinding.FindAllReachableTiles(unit);
        }

        public static List<Tile> GetTilesWithinRadius(Unit unit, int radius) //get all tiles within a the tile-radius "range" around a given Unit
        {
            List<Tile> tilesWithinRadius = new List<Tile>();
            tilesWithinRadius.Add(unit.Tile); // we start this search from the tile the unit stand on.

            List<Tile> tilesToAdd = new List<Tile>(); // for storing tiles while we iterate over tilesWithinRange

            for (int i = 0; i < radius; i++)
            {
                tilesToAdd.Clear(); //make sure the list is empty before adding to it
                foreach (Tile t in tilesWithinRadius)
                {
                    tilesToAdd.AddRange(t.Neighbours);
                }
                tilesWithinRadius.AddRange(tilesToAdd); // add neighbours to list
                tilesWithinRadius = tilesWithinRadius.Distinct().ToList(); //remove duplicates

            }
            tilesWithinRadius.Remove(unit.Tile); // exclude the tile you stand on
            return tilesWithinRadius;
        }

        public static List<Tile> GetTilesWithinRange(Unit unit, int range) // for unit that shoot in straigt direction
        {
            List<Tile> tilesWithinRange = new List<Tile>();

            tilesWithinRange.AddRange(GetTilesinDirection(unit.Tile, range, "NW")); //check in all six directions
            tilesWithinRange.AddRange(GetTilesinDirection(unit.Tile, range, "NE"));
            tilesWithinRange.AddRange(GetTilesinDirection(unit.Tile, range, "E"));
            tilesWithinRange.AddRange(GetTilesinDirection(unit.Tile, range, "SE"));
            tilesWithinRange.AddRange(GetTilesinDirection(unit.Tile, range, "SW"));
            tilesWithinRange.AddRange(GetTilesinDirection(unit.Tile, range, "W"));

            return tilesWithinRange;
        }

        public static List<Tile> GetTilesinDirection(Tile tile, int range, string direction)
        {
            List<Tile> tiles = new List<Tile>();
            Tile newTile = tile.GetNeighbourInDirection(direction);
            if (newTile == null) //if no tiles was found, stop
                return tiles;

            tiles.Add(newTile);
            range--;
            if (range <= 0) // if there is no range left, stop
                return tiles;

            tiles.AddRange(GetTilesinDirection(newTile, range, direction)); //else continue to recurse in the same direction 
            return tiles;
         }


    }
}


