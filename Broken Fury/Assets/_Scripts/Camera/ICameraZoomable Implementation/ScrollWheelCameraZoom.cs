using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{

    public class ScrollWheelCameraZoom : MonoBehaviour, ICameraZoomable
    {
        [SerializeField] private float minDistance = 2;
        [SerializeField] private float maxDistance = 10;

        [RangeAttribute(1, 100)]
        public int cameraZoomSpeed = 50;

        private int cameraZoomSteps = 10;

        // I use the world XZ plane here to calculate some zoom distance as the game world tiles are placed along the world XZ-plane 
        private Vector3 planeNormal = Vector3.up; // the normal vector for the XZ-plane
        private Vector3 planePoint = Vector3.zero; // point on the world XZ-plane

        private float currentDistance;

        private Transform camTrans;
        private Vector3 TargetCamPosition;

        private bool isZooming;

        // Start is called before the first frame update
        void Start()
        {
            camTrans = Camera.main.transform;
        }

        public void Zoom()
        {
            TargetCamPosition = camTrans.position;

            currentDistance = GetDistanceToPlane(planeNormal, planePoint, camTrans.position); //updating the current distance to the XZ-plane

            //Zoom in
            if (Input.GetAxis("Mouse ScrollWheel") > 0f // if user is zoom in with the scroll Wheel
                && currentDistance > minDistance) // if the camera is allowed to come closer
            {
                TargetCamPosition += camTrans.forward * cameraZoomSpeed * Time.deltaTime;
                if (GetDistanceToPlane(planeNormal, planePoint, TargetCamPosition) < minDistance)
                {
                    SetTargetPositionToSetDistance(minDistance); //set to minimum distance
                }
            }
            // Zoom out
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f // if user is zoom in with the scroll Wheel
                && currentDistance < maxDistance) // if the camera is allow to go further away
            {
                TargetCamPosition += -camTrans.forward * cameraZoomSpeed * Time.deltaTime;
                if (GetDistanceToPlane(planeNormal, planePoint, TargetCamPosition) < minDistance)
                {
                    SetTargetPositionToSetDistance(minDistance); //set to maximum distance
                }
            }


            if (TargetCamPosition != camTrans.position) //if the camera is not a its target position - start Zoom Coroutine
            {
                isZooming = true;
                StartCoroutine(SmoothZoom());
            }
            else
            {
                isZooming = false;
            }

        }

        public bool IsZooming()
        {
            return isZooming;
        }

        private float GetDistanceToPlane(Vector3 planeNormal, Vector3 planePoint, Vector3 point)
        {
            return Vector3.Dot(planeNormal, (point - planePoint));
        }

        private IEnumerator SmoothZoom()
        {
            while (TargetCamPosition != camTrans.position)
            {
                camTrans.position = Vector3.Lerp(camTrans.position, TargetCamPosition, cameraZoomSteps * Time.deltaTime);
                yield return 0;
            }
            yield break;
        }


        public void SetTargetPositionToSetDistance(float targetDistance)
        {
            //needs to be implemented to ensure that the camera does not cross the min- and maxDistances
        }
    }
}
