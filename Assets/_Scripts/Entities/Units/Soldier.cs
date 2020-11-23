using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BrokenFury.Test
{   
    public class Soldier : Unit
    {

        private void Start()
        {
            SetMaxHealth();
            Rest();
        }

        public override bool CanMove()
        {
            if (MovementLeft > 0 && Owner.Fuel>Cost)
            {                
                return true;
            }
            return false;
        }

        public override bool CanStrike()
        {           
            if (StrikesLeft > 0 && MovementLeft > 0 && Owner.Fuel > 0)
            {                
                return true;
            }
            return false;
        }



        public override void Rest()
        {
            Debug.Log(string.Format("{0} at {1}", name, Tile.name));
            MovementLeft = MaxMovement;
            StrikesLeft = MaxStrikes;

            EntityBar.GetComponent<EntityBar>().Rest(1f, 1f);
        }

        public override void OnMove(Tile target)
        {
            Path path = Pathfinding.FindPath(Tile, target);
            Fortified = false;
            MovementLeft -= path.movementCost;
            Owner.ChangeFuel(Cost);
            EntityBar.GetComponent<EntityBar>().SetMoveSize(MovementLeft / MaxMovement);
            StartCoroutine(Move(target));
        }

        public override void OnStrike()
        {
            MovementLeft--; //change to the travel cost 
            StrikesLeft--;
            Owner.ChangeFuel(Cost);
            EntityBar.GetComponent<EntityBar>().SetStrikeSize(StrikesLeft / MaxStrikes);
            EntityBar.GetComponent<EntityBar>().SetMoveSize(MovementLeft / MaxMovement);
        }

        private void OnCollisionStay(Collision collision)
        {
            // Rework
            if (collision.collider.CompareTag("Unit") || collision.collider.CompareTag("Fortification"))
            {
                collision.gameObject.GetComponent<Entity>().TakeDamage(GetDamage());
            }
        }

        public override IEnumerator Move(Path path)
        {
            GameManager.instance.BattleSystem.Player(); // not sure what this does? - thomas

            transform.position = path.tiles[0].Position; //ensure the unit is on the start position.
            Tile target = path.tiles[path.length];

            //iterate over each tile in the path
            for (int i = 0; i < path.length-1; i++)
            {
                Tile from = path.tiles[i];
                Tile to = path.tiles[i + 1];
                //move from one tile to another
                while(false)
                {

                }
                Tile.MilitaryUnit = null; // Vacate the Tile of origin unit slot
                Tile = to; // Set new parent tile
                Tile.MilitaryUnit = GetComponent<Unit>(); // Occupy the Unit slot

                if (Tile.Owner != null && Tile.Owner != Owner) // if the tile belongs to an enemy
                    Tile.SetTileOwner(Owner); // Capture the tile
            }




            return base.Move(path);
        }

        public  IEnumerator Move(Tile tile)
        {
            Vector3 lookAtVector = new Vector3(tile.transform.position.x - transform.position.x, 0, tile.transform.position.z - transform.position.z);
            transform.rotation = Quaternion.LookRotation(lookAtVector); // Look at the target tile
            //transform.LookAt(tile.transform); 
            GameManager.instance.BattleSystem.Player();

            while (Vector3.Distance(transform.position, tile.transform.position) > 0.01)
            {
                Debug.Log("moving unit");
                transform.position = Vector3.Lerp(transform.position, tile.transform.position, moveSpeed * Time.deltaTime); ;

                yield return new WaitForEndOfFrame();
            }

            Tile.MilitaryUnit = null; // Vacate the Tile of origin unit slot
            Tile = tile; // Set new parent tile
            Tile.MilitaryUnit = GetComponent<Unit>(); // Occupy the Unit slot

            if(Tile.Owner != null && Tile.Owner != Owner) // if the tile belongs to an enemy
                Tile.SetTileOwner(Owner); // Capture the tile
            

            yield return new WaitForSeconds(0f);
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            if (GameManager.instance.BattleSystem.CurrentPlayer != Owner && Owner != null)
                return;

            GameManager.instance.BattleSystem.OnSelectUnit(this);
        }

        private void OnDestroy()
        {
            try
            {
                Owner.Units.Remove(this);
                if (Tile.MilitaryUnit == this)
                {
                    Tile.MilitaryUnit = null;
                }
            }
            catch
            {

            }
        }
    }
}
