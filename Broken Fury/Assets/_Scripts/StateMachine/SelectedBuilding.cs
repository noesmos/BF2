using System.Collections;
using UnityEngine;

namespace BrokenFury.Test
{
    internal class SelectedBuilding : State
    {
        public SelectedBuilding(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            Debug.Log("SelectedBilding State Active.");

            BattleSystem.BuildingInterface.SetActive(true);
            BattleSystem.BuildingInterface.transform.localPosition = BattleSystem.PanelPosition(BattleSystem.SelectedBuilding, 1f);
            if (BattleSystem.SelectedBuilding.GetComponent<Factory>())
            {
                Debug.Log("Factory.");
                BattleSystem.SelectedBuilding.GetComponent<Factory>().factoryInterface = BattleSystem.FactoryProduce;
            }
            
            yield return new WaitForSeconds(0f);
        }

        public override IEnumerator Tile()
        {
            yield return new WaitForSeconds(0f);            
        }

        public override IEnumerator Building()
        {
            BattleSystem.SetState(new SelectedBuilding(BattleSystem));

            yield return new WaitForSeconds(0f);           
        }
    }
}