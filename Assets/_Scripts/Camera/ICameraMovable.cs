using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{
    public interface ICameraMovable
    {
        void Move();
        bool IsMoving();
    }
}
