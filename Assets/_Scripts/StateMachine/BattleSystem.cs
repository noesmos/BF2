using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BrokenFury.Test
{
    public class BattleSystem : StateMachine
    {
        [SerializeField] private HUD hud;
        [SerializeField] private List<Player> players; 
        [SerializeField] private TileInterface tileInterface;
        [SerializeField] private GameObject buildingInterface;
        [SerializeField] private GameObject unitInterface;
        [SerializeField] private GameObject buildingPanel;
        [SerializeField] private GameObject factoryProduce;
        [SerializeField] private GameObject button;

        public GameObject BuildingInterface => buildingInterface;
        public GameObject UnitInterface => unitInterface;
        public GameObject BuildingPanel => buildingPanel;
        public GameObject FactoryProduce => factoryProduce;
        public GameObject Button => button;
        public HUD Interface => hud;
        public List<Player> Players => players;
        public TileInterface TileInterface => tileInterface;
        public Player CurrentPlayer { get; private set; }
        public int CurrentTurn { get; private set; }
        public Tile SelectedTile { get; private set; }
        public GameObject SelectedBuilding { get; private set; }
        public Unit SelectedUnit { get; private set; }
        public bool NewTurn { get; set; }


    private void Start()
        {
            GameManager.instance.BattleSystem = this;
            foreach (Player p in Players)
            {
                p.NewGame();
            }

            CurrentPlayer = Players[0];
            SetState(new Begin(this));
        }

        public void OnNextButton()
        {
            NewTurn = true;
            ClearPanel();

            int index = Players.IndexOf(CurrentPlayer)+1;
            if (index >= Players.Count)
            {
                index = 0;
                CurrentTurn++;
            }
            CurrentPlayer = Players[index];
            
            Interface.SetCurrentPlayer(CurrentPlayer);
            Interface.UpdateTurnText(CurrentTurn);

            Player();
        }

        public void Player()
        {
            StartCoroutine(State.Player());
        }

        public void OnConstructBuilding(GameObject build)
        {
            StartCoroutine(State.ConstructBuilding(build));
        }

        public void OnSelectTile(Tile tile)
        {
            SelectedTile = tile;
            StartCoroutine(State.Tile());
        }

        public void OnSelectBuilding(GameObject build)
        {
            SelectedBuilding = build;
            StartCoroutine(State.Building());
        }

        public void OnSelectUnit(Unit unit)
        {
            SelectedUnit = unit;
            StartCoroutine(State.Unit());
        }

        public void OnUnitMove()
        {
            HighlightManager.HighlightTileList(SelectedUnit.tilesWithinMovement);
            StartCoroutine(State.Move(SelectedTile));
        }

        public void OnUnitStrike()
        {
            HighlightManager.HighlightTileList(SelectedUnit.tilesWithinRange);
            StartCoroutine(State.Strike(SelectedTile));
        }

        public void OnUnitFortify()
        {
            StartCoroutine(State.Fortify(SelectedTile));
        }

        public void AddWorker()
        {
            Building b = SelectedBuilding.GetComponent<Building>();
            
            if(CurrentPlayer.Population>0 && b.Workers < b.WorkerCapacity)
            {
                b.ChangeWorker(1);
            }
            Interface.UpdatePopText(CurrentPlayer);
        }

        public void ChangeProduce()
        {
            if (SelectedBuilding.GetComponent<Factory>())
                SelectedBuilding.GetComponent<Factory>().NextProduce();
        }

        public void RemoveWorker()
        {
            Building b = SelectedBuilding.GetComponent<Building>();
            
            if (b.Workers > 0)
            {
                b.ChangeWorker(-1);
            }
            Interface.UpdatePopText(CurrentPlayer);
        }

        public void CollectResources()
        {
            Building b = SelectedBuilding.GetComponent<Building>();

            if(b.Storage > 0)
            {
                b.CollectStorage();
            }

            UpdateResources();
        }

        public void CollectAll()
        {
            foreach (Tile t in CurrentPlayer.Tiles)
            {
                Building build = t.GetComponentInChildren<Building>();
                build.CollectStorage();
            }
            UpdateResources();
        }

        private void UpdateResources()
        {
            Interface.UpdateFoodText(CurrentPlayer);
            Interface.UpdateOreText(CurrentPlayer);
            Interface.UpdateFuelText(CurrentPlayer);
            Interface.UpdatePopText(CurrentPlayer);
        }

        public GameObject AddToBuildingPanel(Building build)
        {            
            GameObject newButton = Instantiate(Button, Vector3.zero, Quaternion.identity, BuildingPanel.transform) as GameObject;
            newButton.GetComponentInChildren<Text>().text = build.GetData();
            void a() => OnSelectBuilding(build.gameObject);
            newButton.GetComponent<Button>().onClick.AddListener(a);

            return newButton;
        }

        public Vector2 PanelPosition(GameObject target, float offset)
        {
            float offsetPosY = target.transform.position.y - offset;

            // Final position of marker above GO in world space
            Vector3 offsetPos = new Vector3(target.transform.position.x, offsetPosY, target.transform.position.z);

            // Calculate *screen* position (note, not a canvas/recttransform position)
            Vector2 canvasPos;
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);

            // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
            RectTransformUtility.ScreenPointToLocalPointInRectangle(BuildingInterface.GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>(), screenPoint, null, out canvasPos);

            // Set
            return canvasPos;
        }

        void ClearPanel()
        {
            // Clear the panel, remove all listeners, destroy all buttons.
            foreach (Transform child in BuildingPanel.transform)
            {
                child.GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(child.gameObject);
            }
        }

    }
}
