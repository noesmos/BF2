using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{
    public class Barracks : Building
    {
        public GameObject soldier;
        public override void CollectStorage()
        {            
            if (Tile.MilitaryUnit != null)
                return;
            Debug.Log("Spawing Soldiers.");
            GameObject newUnit = Instantiate(soldier, Tile.transform);
            newUnit.GetComponentInChildren<MeshRenderer>().material.color = Owner.Color;
            EmptyStorage();
            
            Unit unit = newUnit.GetComponent<Unit>();
            unit.Tile = Tile;
            unit.Owner = Owner;
            Owner.Units.Add(unit);
            Tile.MilitaryUnit = unit;

            GameManager.instance.BattleSystem.Player();
        }
    }
}
