using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BrokenFury.Test
{
    public class Tile : MonoBehaviour
    {
        public Vector2 GridID { get; set; } // Tile ID in the world grid.
        public HexCoordinates HexCoord { get; set; } //3D representation of hexagon coordinate
        public List<Tile> Neighbours { get; set; } // up to 6 tiles that share an edge with this one.
        public Vector3 Position { get; set; } // Position of this tile in world space.
        public float Elevation { get; set; } // The elevation of this tile.
        public Player Owner { get; set; } // Current owner of this tile.
        public List<GameObject> Lots { get; set; } // 7 possible locations to place buildings.
        public Unit MilitaryUnit { get; set; }
        public HighlightManager HighlightManager { get; set; }

        public ParticleSystem strikeParticles;
        public TerrainFeature terrainFeature;

        //pathfinding
        public float hCost { get; set; }
        public float gCost { get; set; }
        public float fCost { get { return gCost + hCost; } }
        public Tile parent { get; set; }

        private static Dictionary<string, Vector3> neighbourDictionary = new Dictionary<string, Vector3> // dictionary for when you want to get a specific neighbour tiles
        {
            {"NW", new Vector3(1,-1,0)}, //the northwest neighbour tile
            {"NE", new Vector3(1,0,-1)}, //the northeast neighbour tile
            {"E", new Vector3(0,1,-1)}, //the east neighbour tile
            {"SE", new Vector3(-1,1,0)}, //the southeast neighbour tile
            {"SW", new Vector3(-1,0,1)}, //the southwest neighbour tile
            {"W", new Vector3(0,-1,1)}, //the west neighbour tile
        };



        private void Awake()
        {
            HighlightManager = GetComponent<HighlightManager>();
        }

        private void Start()
        {
            Lots = new List<GameObject>();
            foreach(Transform child in transform)
            {
                if(child.GetComponentInChildren<LotManager>())
                    Lots.Add(child.GetChild(0).gameObject);
            }
            transform.position = new Vector3(transform.position.x, Elevation / 4, transform.position.z);
        }

        public void SetTileOwner(Player player)
        {
            if(Owner != null && Owner != player) // If tile belongs to Enemy
            {
                Owner.Tiles.Remove(this); // Remove tile from Enemy
            }

            if (Owner != null && Owner == player)
            {
                return;
            }

            Owner = player; // Capture the tile
            Owner.Tiles.Add(this);
            if (GetComponentInChildren<Building>()) // If there is a building on the tile
                GetComponentInChildren<Building>().SetBuildingOwner(player); // Capture Building
        }

        public bool CanMoveTo()
        {            
            if (MilitaryUnit == null)
                return true;
            Debug.Log(string.Format("{0}, {1}", MilitaryUnit.Owner.ToString(), GameManager.instance.BattleSystem.CurrentPlayer));
            if (MilitaryUnit.Owner != GameManager.instance.BattleSystem.CurrentPlayer)
                return true;
            return false;
        }

        public bool CanStrikeTile(Tile target, int range)
        {
            if (range == 0)
                return false;
            if (Neighbours.Contains(target))
                return true;
            range--;            
            foreach(Tile t in Neighbours)
            {
                if (t.CanStrikeTile(target, range))
                    return true;
            }
            return false;
        }

        public void DistributeDamage(int damage)
        {
            List<Entity> targets = GetTargets();
            targets.Sort();
            foreach (Entity t in targets)
            {
                int tmpHealth = t.Health;
                t.TakeDamage(damage);
                damage = Mathf.Max(0, damage - tmpHealth);
            }
        }

        public void SplashDamage(int damage)
        {
            List<Entity> targets = GetTargets();

            foreach (Entity t in targets)
            {
                t.TakeDamage(damage);
            }
        }

        List<Entity> GetTargets()
        {
            List<Entity> targets = new List<Entity>();

            foreach (GameObject l in Lots)
            {
                Entity e = l.GetComponentInChildren<Entity>();
                if (e != null && e.DamagePriority > 0)
                {
                    targets.Add(e);
                }
            }
            if (MilitaryUnit != null)
                targets.Add(MilitaryUnit);

            return targets;
        }

        public Tile GetNeighbourInDirection(string direction)
        {
            Vector3 offset = neighbourDictionary[direction];
            HexCoordinates neighbourHex = HexCoord.GetNeighBourHexCoordinates(offset);
            return GetNeighbourTileByHexCoordinate(neighbourHex);
        }

        public Tile GetNeighbourInDirection(Vector3 direction)
        {
            HexCoordinates neighbourHex = HexCoord.GetNeighBourHexCoordinates(direction);
            return GetNeighbourTileByHexCoordinate(neighbourHex);
        }

        private Tile GetNeighbourTileByHexCoordinate(HexCoordinates hex)
        {
            foreach(Tile t in Neighbours)
            {
                if (t.HexCoord == hex)
                {
                    return t;
                }
            }
            return null;
        }

        public void OnMouseDown()
        {            
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            GameManager.instance.BattleSystem.OnSelectTile(this);
        }
    }
}
