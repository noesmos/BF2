using System.Collections;
using UnityEngine;

namespace BrokenFury.Test
{
    internal class Move : State
    {
        public Move(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            Debug.Log("Move State Active.");

            BattleSystem.UnitInterface.SetActive(false);
            if (!BattleSystem.SelectedUnit.CanMove())
            {
                Debug.Log("Unit can't move this turn.");
                BattleSystem.SetState(new PlayerTurn(BattleSystem));
            }

            yield return new WaitForSeconds(0f);
        }

        public override IEnumerator Tile()
        {
            if (BattleSystem.SelectedUnit.tilesWithinMovement.Contains(BattleSystem.SelectedTile))
            {
                HighlightManager.UnHighlightTileList(BattleSystem.SelectedUnit.tilesWithinMovement);
                if (BattleSystem.SelectedTile.CanMoveTo())                    
                        BattleSystem.SelectedUnit.OnMove(BattleSystem.SelectedTile);
            }

            yield return new WaitForSeconds(0f);
        }
    }
}