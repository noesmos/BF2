using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{
    public class Fortification : Entity
    {
        [SerializeField] private int damage;
        public int Damage => damage;

        private void Start()
        {
            Tile = this.GetComponentInParent<Tile>();
            transform.LookAt(Tile.transform);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Unit") && collision.collider.gameObject.GetComponent<Entity>().Owner != Owner)
            {
                collision.gameObject.GetComponent<Entity>().TakeDamage(Damage);
            }
        }

        private void OnDestroy()
        {
            try
            {
                Owner.Tiles.Remove(Tile);
                Tile.SetTileOwner(null);
            }
            catch
            {

            }
        }
    }
}
