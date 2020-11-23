using System.Collections;
using UnityEngine;

namespace BrokenFury.Test
{
    internal class Strike : State
    {
        public Strike(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            Debug.Log("Strike State Active.");

            BattleSystem.UnitInterface.SetActive(false);

            if (!BattleSystem.SelectedUnit.CanStrike())
            {
                Debug.Log("Unit can't strike this turn.");
                BattleSystem.SetState(new PlayerTurn(BattleSystem));
            }

            yield return new WaitForSeconds(0f);
        }

        public override IEnumerator Tile()
        {
            
            //if (BattleSystem.SelectedUnit.Tile.CanStrikeTile(BattleSystem.SelectedTile, BattleSystem.SelectedUnit.GetRange()))
            if(BattleSystem.SelectedUnit.tilesWithinRange.Contains(BattleSystem.SelectedTile))
            {
                HighlightManager.UnHighlightTileList(BattleSystem.SelectedUnit.tilesWithinRange);

                Tile targetTile = BattleSystem.SelectedTile;
                targetTile.strikeParticles.Play();
                if (BattleSystem.SelectedUnit.GetSplash())
                    targetTile.SplashDamage(BattleSystem.SelectedUnit.GetDamage());
                else
                    targetTile.DistributeDamage(BattleSystem.SelectedUnit.GetDamage());
                BattleSystem.SelectedUnit.OnStrike();
            }
            BattleSystem.Player();
            yield return new WaitForSeconds(0f);
        }
    }
}