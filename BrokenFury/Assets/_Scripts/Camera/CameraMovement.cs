using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{

    public class CameraMovement : MonoBehaviour
    {

        private ICameraMovable cameraMover;
        private ICameraZoomable cameraZoomer;
        GameObject camObject;

        private void Awake()
        {
            cameraMover = GetComponent<ICameraMovable>();
            cameraZoomer = GetComponent<ICameraZoomable>();


            camObject = Camera.main.gameObject;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (cameraMover == null)
                Debug.LogError("Nullreference: ICameraMovable implementation on " + gameObject.name + " is missing");
            else if(!cameraZoomer.IsZooming())           
                cameraMover.Move();

            if (cameraZoomer == null)
                Debug.LogError("Nullreference: ICameraZoomable implementation on " + gameObject.name + " is missing");
            else if(!cameraMover.IsMoving())
                cameraZoomer.Zoom();
        }
    }
}
