using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{

    public class Unit : Entity
    {
        [SerializeField] private int damage;
        [SerializeField] private float maxMovement;
        [SerializeField] private int maxStrikes;
        [SerializeField] private int range;
        [SerializeField] private bool splash;
        [SerializeField] protected int moveSpeed = 5;

        public float MaxMovement => maxMovement;
        public int MaxStrikes => maxStrikes;
        public bool Splash => splash;

        public float MovementLeft { get; protected set; }
        public float StrikesLeft { get; protected set; }

        public List<Tile> tilesWithinMovement { get { return Pathfinding.FindAllReachableTiles(this); }}
        public List<Tile> tilesWithinRange { get { return TileSearch.GetTilesWithinRadius(this, range); } }

        public override int GetDamage()
        {
            return damage;
        }

        public override int GetRange()
        {
            return range;
        }

        public virtual bool CanMove()
        {
            return false;
        }

        public virtual bool CanStrike()
        {
            return false;
        }

        public virtual void OnMove(Tile target)
        {

        }

        public virtual IEnumerator Move(Path path)
        {
            yield break;
        }

        public virtual void OnStrike()
        {

        }
    }
}
