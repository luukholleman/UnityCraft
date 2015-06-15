using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Code.Player
{
    class Controller : MonoBehaviour
    {
        public float MoveSpeed = 10f;
        public float RotateSpeed = 10f;
        public float LookSensivity = 15f;
        
        float _rotationX;
        float _rotationY;

        public float minimumX = -360F;
        public float maximumX = 360F;

        public float minimumY = -360F;
        public float maximumY = 360F;

        Quaternion _originalRotation;

        private Rigidbody _rigidbody;

        float rot = 0f;

        private bool _stableThrusters;

        void Start()
        {
            if (GetComponent<Rigidbody>())
                GetComponent<Rigidbody>().freezeRotation = true;
            _originalRotation = transform.localRotation;

            _rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            UpdateView();

            if (Input.GetKeyDown(KeyCode.F))
                _stableThrusters = !_stableThrusters;

            if (Input.GetKeyDown(KeyCode.R))
                _rigidbody.useGravity = !_rigidbody.useGravity;

            if(_stableThrusters)
                UpdateStableVelocity();
            else
                UpdateFreeVelocity();
        }

        void OnGUI()
        {
            UnityEngine.GUI.Label(new Rect(10, 10, 200, 20), "[F] Stable thrusters: " + _stableThrusters);
            UnityEngine.GUI.Label(new Rect(10, 30, 200, 20), "[R] Gravity: " + _rigidbody.useGravity);
            UnityEngine.GUI.Label(new Rect(10, 50, 200, 20), "Speed: " + _rigidbody.velocity.magnitude);
        }

        private void UpdateStableVelocity()
        {
            _rigidbody.AddForce(-_rigidbody.velocity * 30);

            Vector3 velocity = new Vector3();

            velocity.x = Input.GetAxis("Horizontal") * MoveSpeed;
            velocity.z = Input.GetAxis("Vertical") * MoveSpeed;

            if (Input.GetKey(KeyCode.Space))
                velocity.y = MoveSpeed;

            _rigidbody.AddRelativeForce(velocity.normalized * MoveSpeed * (MoveSpeed / 3));
        }

        private void UpdateFreeVelocity()
        {
            Vector3 velocity = new Vector3();

            velocity.x = Input.GetAxis("Horizontal") * MoveSpeed;
            velocity.z = Input.GetAxis("Vertical") * MoveSpeed;

            if (Input.GetKey(KeyCode.Space))
                velocity.y = MoveSpeed;

            _rigidbody.AddRelativeForce(velocity);
        }

        void UpdateView()
        {
            if (Input.GetKey(KeyCode.Q))
            {
                rot += RotateSpeed;
            }
            if (Input.GetKey(KeyCode.E))
            {
                rot -= RotateSpeed;
            }

            // Read the mouse input axis
            _rotationX += Input.GetAxis("Mouse X") * LookSensivity;
            _rotationY += Input.GetAxis("Mouse Y") * LookSensivity;
            
            //_rotationX = ClampAngle(_rotationX, minimumX, maximumX);
            //_rotationY = ClampAngle(_rotationY, minimumY, maximumY);

            Quaternion xQuaternion = Quaternion.AngleAxis(_rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(_rotationY, -Vector3.right);
            Quaternion zQuaternion = Quaternion.AngleAxis(rot, Vector3.forward);

            transform.localRotation = _originalRotation * xQuaternion * yQuaternion;

            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rot);
        }

        public static float ClampAngle (float angle, float min, float max)
         {
             if (angle < -360F)
                 angle += 360F;
             if (angle > 360F)
                 angle -= 360F;
             return Mathf.Clamp (angle, min, max);
         }

    }
}
