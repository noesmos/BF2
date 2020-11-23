﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace BrokenFury.Test
{
    public class MapGenerator : MonoBehaviour
    {
        readonly Vector2[,] oddKernel = new Vector2[3, 2] { { new Vector2(-1, 0), new Vector2(-1, 1) }, { new Vector2(0, -1), new Vector2(0, 1) }, { new Vector2(1, 0), new Vector2(1, 1) } };
        readonly Vector2[,] evenKernel = new Vector2[3, 2] { { new Vector2(-1, -1), new Vector2(-1, 0) }, { new Vector2(0, -1), new Vector2(0, 1) }, { new Vector2(1, -1), new Vector2(1, 0) } };
        public static GameObject[,] worldLocal;

        public float[,] FillMap(int width, int length)
        {
            float[,] map = new float[width, length];

            List<string[]> dataGrid = GetData("MapData");
            
            if (dataGrid.Count-1<width || dataGrid.Count<length ||dataGrid[0].Length<width ||dataGrid[0].Length<length)
            {
                Debug.LogError("Map Size Out of Bounds");
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    map[x, y] = float.Parse(dataGrid[x][y]);
                    //map[x, y] = UnityEngine.Random.Range(0, maxElevation); // Optimize Later
                }
            }
            return map;
        }

        public GameObject[,] DrawWorld(float[,] map, GameObject pfTile)
        {
            int width = map.GetLength(0);
            int length = map.GetLength(1);
            // Create game objects for eahc Tile.
            GameObject[,] world = new GameObject[width, length];

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    Vector3 pos = new Vector3(x * 1.56f, map[x, z] / 4, z * 1.8f + ((x % 2 == 0) ? 0f : 0.9f)); // Optimize Later
                                                                                                    // Assign values to each Tile.
                    world[x, z] = Instantiate(pfTile, pos, Quaternion.identity) as GameObject;
                    Vector2 gridID = new Vector2(x, z);
                    HexCoordinates hexCoord = HexCoordinates.FromOffsetCoordinate(x,z);
                    GameObject go = world[x, z];
                    go.name = hexCoord.ToString();// gridID.ToString();
                    go.transform.Rotate(0.0f, 30.0f, 0.0f, Space.Self);

                    Tile grt = world[x, z].GetComponent<Tile>();
                    grt.Elevation = map[x, z];// Mathf.Sqrt( Mathf.Pow(map[x, z], 2)/2);
                    grt.Position = pos;
                    grt.GridID = gridID;
                    grt.HexCoord = hexCoord;

                    GameObject child = go.transform.GetChild(0).gameObject;
                    SpriteRenderer coreSprite = child.GetComponent<SpriteRenderer>();
                    coreSprite.color = new Color((0 + grt.Elevation/4), (1 - grt.Elevation/4), 0); // optimize later
                }
            }

            worldLocal = world;
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {

                    GameObject go = world[x, z];
                    Tile grt = go.GetComponent<Tile>();
                    grt.Neighbours = GetNeighbours(grt.GridID);
                }

            }
            return world;
        }

        List<string[]> GetData(string file)
        {
            TextAsset mapData = Resources.Load<TextAsset>(file);
            string[] data = mapData.text.Split(new char[] { '\n' });
            List<string[]> grid = new List<string[]>();
            int i = 0;
            foreach (string s in data)
            {
                string[] tmp = s.Split(new char[] { ',' });
                grid.Add(tmp);
                i++;
            }
            return grid;
        }

        List<Tile> GetNeighbours(Vector2 id)
        {
            // Remake with recursion
            List<Tile> neighbours = new List<Tile>();
            int width = worldLocal.GetLength(0);
            int length = worldLocal.GetLength(1);
            Vector2 l;
            if (id.x % 2 == 0) // Even
            {
                foreach (Vector2 k in evenKernel)
                {
                    l = id + k;
                    if (l.x < 0 || l.x > width - 1 || l.y < 0 || l.y > length - 1)
                        continue;
                    neighbours.Add(worldLocal[(int)l.x, (int)l.y].GetComponent<Tile>());
                }
            }
            else //Odd
            {
                foreach (Vector2 k in oddKernel)
                {
                    l = id + k;
                    if (l.x < 0 || l.x > width - 1 || l.y < 0 || l.y > length - 1)
                        continue;
                    neighbours.Add(worldLocal[(int)l.x, (int)l.y].GetComponent<Tile>());
                }
            }
            return neighbours;
        }

    }
}
