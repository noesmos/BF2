using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrokenFury.Test
{
    public class Factory : Building
    {
        public List<GameObject> availableUnits;
        public GameObject factoryInterface;
        GameObject toProduce;
        int index = -1;

        public override void CollectStorage()
        {
            if (toProduce==null && Owner.ConsumeFuel(CollectionFuelCost))
            {
                Owner.ChangeOre(Storage);
                EmptyStorage();
            }else if (toProduce!=null) {
                if (Tile.MilitaryUnit != null)
                    return;
                else
                {
                    Debug.Log("Spawing Soldiers.");
                    GameObject newUnit = Instantiate(toProduce, Tile.gameObject.transform);
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

        public void NextProduce()
        {
            index++;
            if (index > availableUnits.Count-1)
            {
                index = -1;
                toProduce = null;
                factoryInterface.GetComponentInChildren<Text>().text = "Ore";
                SetProduceBase(10, 100, 1);
            }
            else
            {
                toProduce = availableUnits[index];
                factoryInterface.GetComponentInChildren<Text>().text = toProduce.name;
                SetProduceBase(1, 1, 3);
            }
            UpdateButtonData();
        }
    }
}
