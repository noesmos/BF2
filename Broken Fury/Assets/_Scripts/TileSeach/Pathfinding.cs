using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{
    //this algorithm is based on A star pathfinding which uses 3 distances to determine the best route
    //g cost: the travelled distance from the start tile to a given tile in the grid
    //h cost: the distance between a given tile and the target tile
    //f cost: the sum of g and h

    //it uses an "open" set of tiles to find the best candidate tile to continue the search based on the f cost
    //Any tile that have been chosen as the best candidate will be removed from the "open" set and added the "closed" as a way of saying the tile has been evaluated 
    public class Pathfinding : MonoBehaviour
    {
        public static Path FindPath(Tile start, Tile target)
        {
            List<Tile> openSet = new List<Tile>();
            List<Tile> closedSet = new List<Tile>();
            openSet.Add(start); // we start the search from the start tile

            while(openSet.Count > 0) // continue the search while there are still "open" tiles
            {
                Tile currentTile = openSet[0];
      
                for (int i = 1; i < openSet.Count; i++) //for loop for finding the best candidate. this is skipped in the first iteration of the search
                {
                    if (openSet[i].fCost < currentTile.fCost || // swap if the openSet[i] has lower fcost than currentTile
                        openSet[i].fCost == currentTile.fCost && openSet[i].hCost <currentTile.hCost) // or if the fcosts are the same, use the hcost
                    {
                        currentTile = openSet[i];
                    }
                }

                openSet.Remove(currentTile); //update the lists based on the best candidate, the currentTile
                closedSet.Add(currentTile);

                if (currentTile == target) // if you have reach the target tile
                {
                    Path path = new Path(RetracePath(start, target));
                    return path;
                }


                foreach (Tile neighbour in currentTile.Neighbours) 
                {
                    if (!Traversable(currentTile, neighbour) || //checking if it is allowed continue the search through the current neighbour
                        closedSet.Contains(neighbour))
                        continue;

                    float gCostToNeighbour = currentTile.gCost + GetTravelCost(currentTile, neighbour); 
                    if (gCostToNeighbour < neighbour.gCost || // if we have found a new better route to the current neighbour
                        !openSet.Contains(neighbour)) // of the neighbour is not in the openSet
                    {
                        neighbour.gCost = gCostToNeighbour; // set gCost
                        neighbour.hCost = GetTileDistance(neighbour, target); //set hCost
                        neighbour.parent = currentTile;

                        if (!openSet.Contains(neighbour)) // add neightbour to the openSet
                            openSet.Add(neighbour);  
                    }
                }
            }
            Debug.Log("code should not reach here");
            return null;
        }

        public static List<Tile> FindAllReachableTiles(Unit unit) // optimize later as it shares much code with the function FindPath()
        {
            List<Tile> openSet = new List<Tile>();
            List<Tile> closedSet = new List<Tile>();
            openSet.Add(unit.Tile); // we start the search from the Unit
            unit.Tile.gCost = 0;

            while (openSet.Count > 0)
            {
                Tile currentTile = openSet[0];

                for (int i = 1; i < openSet.Count; i++) //for loop for finding the best candidate. this is skipped in the first iteration of the search
                {
                    if (openSet[i].gCost < currentTile.gCost)
                        currentTile = openSet[i];
                }

                openSet.Remove(currentTile);
                closedSet.Add(currentTile);

                foreach (Tile neighbour in currentTile.Neighbours)
                {
                    if (!Traversable(currentTile, neighbour) || //checking if it is allowed continue the search through the current neighbour
                    closedSet.Contains(neighbour))
                        continue;

                    float gCostToNeighbour = currentTile.gCost + GetTravelCost(currentTile, neighbour);
                    if (gCostToNeighbour < neighbour.gCost || // if we have found a new better route to the current neighbour
                        !openSet.Contains(neighbour)) // of the neighbour is not in the openSet
                    {
                        neighbour.gCost = gCostToNeighbour;

                        if (!openSet.Contains(neighbour) && currentTile.gCost < unit.MovementLeft) // if the unit has movementLeft enough to reach the neighbour, add it to openSet
                            openSet.Add(neighbour);
                    }
                }
            }

            closedSet.Remove(unit.Tile);
            return closedSet;
        }

        private static bool Traversable(Tile from, Tile to)
        {
            float evelationDifference = Mathf.Abs(to.Elevation - from.Elevation);
            if (to.MilitaryUnit == null && evelationDifference <=1)
            {
                return true; //implement later
            }
            return false;
        }

        private static int GetTileDistance(Tile from, Tile to)
        {
            return HexCoordinates.ManhattanDistance(from.HexCoord, to.HexCoord); // add considerations to terrain features and elevation later
        }

        public static float GetTravelCost(Tile from, Tile to)
        {
            float elevationDifference = to.Elevation - from.Elevation;
            if (elevationDifference >0) //if the terrain moves up
            {
                return 1f + to.terrainFeature.TravelCost;
            }
            return to.terrainFeature.TravelCost;
        }

        private static List<Tile> RetracePath(Tile start, Tile target)
        {
            List<Tile> path = new List<Tile>();
            Tile currentTile = target;

            while (currentTile != start)
            {
                path.Add(currentTile);
                currentTile = currentTile.parent;
            }
            path.Add(start);

            path.Reverse();

            return path;
        }
    }

}

