using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrokenFury.Test
{
    public class TileInterface : MonoBehaviour
    {
        public Text ownerText;

        public void SetTileInterface(GameObject tile)
        {
            // Show the hex buttons for the selected tile.
            this.gameObject.SetActive(true);
            int index = 0;
            foreach (Transform child in transform)
            {
                if(child.GetComponent<TileButton>()!=null)
                    child.GetComponent<TileButton>().Setup(tile, index);
                ++index;
            }
            SetOwnerText(tile);
        }

        public void HideUI()
        {           
            this.gameObject.SetActive(false);
        }

        void SetOwnerText(GameObject tile)
        {
            Player owner;             
            if (tile.GetComponent<Tile>().Owner != null)
            {
                owner = tile.GetComponent<Tile>().Owner;
                ownerText.text = owner.name;
            }
            else
            {
                ownerText.text = "Not Claimed!";
                return;
            }
            
        }
    }
}