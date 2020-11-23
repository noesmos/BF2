using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{
    [CreateAssetMenu]
    public class TerrainFeature : ScriptableObject
    {
        [SerializeField] private string terrainName;
        [SerializeField] private float travelCost;

        public float TravelCost => travelCost;
        public float TerrainName => TerrainName;
    }
}
