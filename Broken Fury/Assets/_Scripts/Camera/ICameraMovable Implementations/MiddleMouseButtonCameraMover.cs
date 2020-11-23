using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{
    // This class listens for user input on the middle mouse button and as long as the button is led down
    // the main camera is moved along the XZ-plane corresponding to how the mouse is moved on the screen
    public class MiddleMouseButtonCameraMover : MonoBehaviour, ICameraMovable
    {
        
        [RangeAttribute(1, 100)]
        [SerializeField] private float cameraMoveSpeed = 50;

        private Vector3 mouseCurrentPosition = Vector3.zero;
        private Vector3 mousePreviousPosition = Vector3.zero;

        private Transform camTrans;

        private bool isMoving;

        // Start is called before the first frame update
        void Start()
        {
            camTrans = Camera.main.transform;
        }

        public void Move()
        {
            if (Input.GetMouseButtonDown(2))
            {
                mousePreviousPosition = GetMousePosition();
                isMoving = true;
            }
            else if(Input.GetMouseButton(2))
            {
                mouseCurrentPosition = GetMousePosition();         
                MoveCamera(mouseCurrentPosition - mousePreviousPosition);

                mousePreviousPosition = mouseCurrentPosition;
            }
            else if (Input.GetMouseButtonUp(2))
            { 
                isMoving = false;
            }
        }

        public bool IsMoving()
        {
            return isMoving;
        }

        private Vector3 GetMousePosition()
        {
            return new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height, Input.mousePosition.z);
        }

        private void MoveCamera(Vector3 mouseDifference)
        {
            if (mouseDifference == Vector3.zero) //if the mouse has not been moved between frames
                return;

            //Change mouseDifference from screenspace to worldspace
            Vector3 moveVector = new Vector3(-mouseDifference.y, 0, mouseDifference.x).normalized; // this is hardcoded.
            camTrans.Translate(moveVector * cameraMoveSpeed * Time.deltaTime, Space.World);

        }
    }
}
