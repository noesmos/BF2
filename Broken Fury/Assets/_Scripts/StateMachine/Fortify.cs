using System.Collections;
using UnityEngine;

namespace BrokenFury.Test
{
    internal class Fortify : State
    {
        public Fortify(BattleSystem battleSystem) : base(battleSystem)
        {
        }

        public override IEnumerator Start()
        {
            Debug.Log("Fortify State Active.");

            BattleSystem.UnitInterface.SetActive(false);
            BattleSystem.SelectedUnit.OnFortify(3);
            BattleSystem.SetState(new PlayerTurn(BattleSystem));
            yield return new WaitForSeconds(0f);
        }
    }
}