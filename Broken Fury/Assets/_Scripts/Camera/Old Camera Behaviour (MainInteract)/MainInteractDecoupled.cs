using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrokenFury.Test
{


    public class MainInteractDecoupled : MonoBehaviour, ICameraMovable, ICameraZoomable
    {
        int minHorizontal;
        int minVertical;
        int maxHorizontal;
        int maxVertical;
        [RangeAttribute(1, 100)]
        public int cameraMoveSpeed = 15;
        [RangeAttribute(1, 500)]
        public int cameraZoomSpeed = 500;
        [RangeAttribute(.01f, .2f)]
        public float cameraMoveThreshold = 0.018f;
        public GameObject pfFlag; // not used

        float mouseNormalX;
        float mouseNormalY;
        Vector3 moveAmount;
        Vector3 zoomAmount;

        Transform camTrans;

        private bool isMoving;
        private bool isZooming;
        private void Start()
        {
            camTrans = gameObject.transform;
            GetRange();   
        }

        public void Move()
        {
            ReadMovementData();
            moveAmount = new Vector3(0, 0, 0);

            //Moving
            if (mouseNormalX < cameraMoveThreshold)
            {
                moveAmount.z += cameraMoveSpeed * (1 - mouseNormalX / cameraMoveThreshold) * Time.deltaTime;
            }
            else if (mouseNormalX > (1 - cameraMoveThreshold))
            {
                moveAmount.z -= cameraMoveSpeed * ((mouseNormalX - (1 - cameraMoveThreshold)) / cameraMoveThreshold) * Time.deltaTime;
            }
            if (mouseNormalY < cameraMoveThreshold)
            {
                moveAmount.x -= cameraMoveSpeed * (1 - mouseNormalY / cameraMoveThreshold) * Time.deltaTime;
            }
            else if (mouseNormalY > (1 - cameraMoveThreshold))
            {
                moveAmount.x += cameraMoveSpeed * ((mouseNormalY - (1 - cameraMoveThreshold)) / cameraMoveThreshold) * Time.deltaTime;
            }
            if (moveAmount != Vector3.zero)
                isMoving = true;
            else
                isMoving = false;

            camTrans.transform.position += moveAmount;
            SmoothTranslation(camTrans);
        }

        public void Zoom()
        {
            zoomAmount = new Vector3(0, 0, 0);
            //Zooming
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // Zoom In
            {
                zoomAmount.x += cameraZoomSpeed * Time.deltaTime * Mathf.Cos(30);
                zoomAmount.y -= cameraZoomSpeed * Time.deltaTime * Mathf.Cos(30);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // Zoom Out
            {
                zoomAmount.x -= cameraZoomSpeed * Time.deltaTime * Mathf.Cos(30);
                zoomAmount.y += cameraZoomSpeed * Time.deltaTime * Mathf.Cos(30);
            }

            if (zoomAmount != Vector3.zero)
                isZooming = true;
            else
                isZooming = false;

            camTrans.position += zoomAmount;
            SmoothTranslation(camTrans);
           
        }

        public bool IsMoving()
        {
            return isMoving;
        }
        public bool IsZooming()
        {
            return isZooming;
        }

        private void GetRange()
        {
            // add way to calculate that
            minHorizontal = -10;
            minVertical = -10;
            maxHorizontal = 23;
            maxVertical = 10;
        }

        private void ReadMovementData()
        {
            // Add zoom in/out limits
            mouseNormalX = Mathf.Clamp(Input.mousePosition.x / Screen.width, 0, 1);
            mouseNormalY = Mathf.Clamp(Input.mousePosition.y / Screen.height, 0, 1);
        }

        private void SmoothTranslation(Transform camTrans)
        {
            camTrans.transform.position = new Vector3(Mathf.Clamp(transform.position.x, minVertical, maxVertical),
               Mathf.Clamp(transform.position.y, 1, 10),
               Mathf.Clamp(transform.position.z, minHorizontal, maxHorizontal));
        }

        
    }
}
