using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BrokenFury.Test
{
    public class EntityBar : MonoBehaviour
    {
        [SerializeField] private Transform health;
        [SerializeField] private Transform move;
        [SerializeField] private Transform strike;

        void Update()
        {
            transform.LookAt(Camera.main.transform, Vector3.up);
        }

        public void SetHealthSize(float healthNormalized)
        {
            health.localScale = new Vector3(Mathf.Max(0, healthNormalized), 1f, 1f);
        }

        public void Rest(float moveNormalized, float strikeNormalized)
        {
            move.localScale = new Vector3(Mathf.Max(0, moveNormalized), 1f, 1f);
            strike.localScale = new Vector3(Mathf.Max(0, strikeNormalized), 1f, 1f);
        }

        public void SetMoveSize(float moveNormalized)
        {
            move.localScale = new Vector3(Mathf.Max(0, moveNormalized), 1f, 1f);
        }

        public void SetStrikeSize(float strikeNormalized)
        {
            strike.localScale = new Vector3(Mathf.Max(0, strikeNormalized), 1f, 1f);
        }

    }
}
