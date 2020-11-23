using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public int mapLength = 0;
        public int mapWidth = 0;
        [RangeAttribute(0, 1f)]
        public float mapElevation = 1;
        public GameObject tilePrefab;
        float[,] map;
        GameObject[,] world;
        public BattleSystem BattleSystem;

        void Awake()
        {
            MakeSingleton();
            MapGenerator mapGenerator = new MapGenerator();
            map = mapGenerator.FillMap(mapWidth, mapLength);
            world = mapGenerator.DrawWorld(map, tilePrefab);
        }


        private void MakeSingleton()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
