using System;
using UnityEngine;

namespace DWGames
{
    public class DWRTSCamera : MonoBehaviour
    {
        private float speed;
        private float zoomSpeed;
        private float rotateSpeed = 0.1f;

        public float maxHeight = 80f;
        public float minHeight = 0.5f;

        private Vector2 p1;
        private Vector2 p2;

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 2f;
                zoomSpeed = 6.0f;
            }
            else
            {
                speed = 1f;
                zoomSpeed = 3.0f;
            }

            var rawMouseWheel = Input.GetAxisRaw("Mouse ScrollWheel");

            var hsp = transform.position.y * speed * Input.GetAxis("Horizontal") * Time.deltaTime;
            var vsp = transform.position.y * speed * Input.GetAxis("Vertical")  * Time.deltaTime;
            var scrollSp = Mathf.Log(transform.position.y) * -zoomSpeed * Input.GetAxis("Mouse ScrollWheel");
       //     Debug.Log("Mouse delta: " + Input.GetAxis("Mouse ScrollWheel"));
        //    Debug.Log("Mouse Wheel raw: " + rawMouseWheel);


            if ((transform.position.y >= maxHeight) && (scrollSp > 0))
            {
                scrollSp = 0;
            }
            else if ((transform.position.y <= minHeight) && scrollSp < 0)
            {
                scrollSp = 0;
            }

            if ((transform.position.y + scrollSp) > maxHeight)
            {
                scrollSp = maxHeight - transform.position.y;
            }
            else if ((transform.position.y + scrollSp) < minHeight)
            {
                scrollSp = minHeight - transform.position.y;
            }

            //  scrollSp *= Time.deltaTime;

            var verticalMove = new Vector3(0, scrollSp, 0);
            var lateralMove = hsp * transform.right;
            var forwardMove = transform.forward;


            forwardMove.y = 0;
            forwardMove.Normalize();
            forwardMove *= vsp;

            var move = verticalMove + lateralMove + forwardMove;
            transform.position += move;
            getCameraRotation();
        }

        private void FixedUpdate()
        {
           
        }

        void getCameraRotation()
        {
            if (Input.GetMouseButtonDown(1))
            {
                p1 = Input.mousePosition;
            }

            if (Input.GetMouseButton(1))
            {
                p2 = Input.mousePosition;
                var dx = (p2 - p1).x * rotateSpeed;
                var dy = (p2 - p1).y * rotateSpeed;
                transform.rotation *= Quaternion.Euler(new Vector3(0, dx, 0));
                transform.GetChild(0).transform.rotation *= Quaternion.Euler(-dy, 0, 0);
                p1 = p2;
            }
        }
    }
}