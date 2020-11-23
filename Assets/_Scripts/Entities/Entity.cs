using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{
    public class Entity : MonoBehaviour, IEquatable<Entity>, IComparable<Entity>
    {
        [SerializeField] private int health;
        [SerializeField] private int armour;
        [SerializeField] private int cost;
        [SerializeField] private int damagePriority;
        [SerializeField] private ParticleSystem pSystem;
        [SerializeField] private GameObject entityBar;
        public float maxHealth;
        public int Health => health;
        public int Armour => armour;    
        public int Cost => cost;
        public int DamagePriority => damagePriority;
        public GameObject EntityBar => entityBar;
        public bool Fortified { get; set; }
        public ParticleSystem PSystem => pSystem;
        
        public Player Owner;
        public Tile Tile { get; set; }

        public void SetMaxHealth()
        {
            maxHealth = health;
        }

        public int SortByDamagePriority(Entity e1, Entity e2)
        {
            return e1.DamagePriority.CompareTo(e2.DamagePriority);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Entity objAsEntity = obj as Entity;
            if (objAsEntity == null) return false;
            else return Equals(objAsEntity);
        }

        public int CompareTo(Entity compareEntity)
        {
            // A null value means that this object is greater.
            if (compareEntity == null)
                return 1;

            else
                return this.DamagePriority.CompareTo(compareEntity.DamagePriority);
        }

        public override int GetHashCode()
        {
            return DamagePriority;
        }
        public bool Equals(Entity other)
        {
            if (other == null) return false;
            return (this.DamagePriority.Equals(other.DamagePriority));
        }

        public void TakeDamage(int damage)
        {
            if (Health < 0)
                return;
            if (Fortified)
                damage = (int)(damage * .8f);
            health -= damage; // Add armour to the equation
            if(EntityBar!=null)
                EntityBar.GetComponent<EntityBar>().SetHealthSize(health/maxHealth);
            Debug.Log(String.Format("{0} took {1} points of damage, {2} health remaining.", name, damage, Health));
            if (Destroyed())
            {
                //gameObject.SetActive(false);
                if(PSystem!=null)
                    PSystem.Play();
                Destroy(this.gameObject, 1f);                
            }
        }

        public virtual int GetDamage()
        {
            return 0;
        }

        public virtual int GetRange()
        {
            return 0;
        }

        public virtual bool GetSplash()
        {
            return false;
        }

        public virtual void OnFortify(int value)
        {
            Fortified = true;
            damagePriority = value;
        }

        public virtual void Rest()
        {

        }

        public bool Destroyed()
        {
            if (Health <= 0)
                return true;
            return false;
        }
    }
}