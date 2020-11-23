using System.Collections;
using UnityEngine;

namespace BrokenFury.Test
{
    internal class SelectedUnit : State
    {
        public SelectedUnit(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            Debug.Log("SelectedUnit State Active.");

            // Show Move and Strike buttons

            BattleSystem.UnitInterface.SetActive(true);
            BattleSystem.UnitInterface.transform.localPosition = BattleSystem.PanelPosition(BattleSystem.SelectedUnit.gameObject, 1f);

            yield return new WaitForSeconds(0f);
        }

        public override IEnumerator Strike(Tile tile)
        {
            yield return new WaitForSeconds(0f);

            BattleSystem.SetState(new Strike(BattleSystem));
        }

        public override IEnumerator Move(Tile tile)
        {
            yield return new WaitForSeconds(0f);

            BattleSystem.SetState(new Move(BattleSystem));
        }

        public override IEnumerator Fortify(Tile tile)
        {
            yield return new WaitForSeconds(0f);

            BattleSystem.SetState(new Fortify(BattleSystem));
        }
    }
}