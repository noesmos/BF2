using System.Collections;
using UnityEngine;

namespace BrokenFury.Test
{
    public class SelectedTile : State
    {
        public SelectedTile(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            Debug.Log("SelectedTile State Active.");
            Player tileOwner = BattleSystem.SelectedTile.GetComponent<Tile>().Owner;
            if (tileOwner == BattleSystem.CurrentPlayer || tileOwner == null)
            {
                BattleSystem.BuildingInterface.SetActive(false);
                BattleSystem.TileInterface.SetTileInterface(BattleSystem.SelectedTile.gameObject);
            }
            else
                BattleSystem.Player();

            yield return new WaitForSeconds(0f);
        }

        public override IEnumerator ConstructBuilding(GameObject building)
        {
            Entity buildScript = building.GetComponent<Entity>();

            Debug.Log("Construct");

            BattleSystem.CurrentPlayer.ChangeOre(buildScript.Cost*(-1));

            BattleSystem.Player();
            yield return new WaitForSeconds(0f);

        }
    }
}