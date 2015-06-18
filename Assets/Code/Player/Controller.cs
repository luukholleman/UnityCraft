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
        public float JumpSpeed = 20f;
        public float RotateSpeed = 10f;
        public float LookSensivity = 15f;

        public float MinimumX = -360F;
        public float MaximumX = 360F;

        public float MinimumY = -360F;
        public float MaximumY = 360F;

        private float _rotationX;
        private float _rotationY;
        private Quaternion _originalRotation;

        private Rigidbody _rigidbody;

        private float _rot = 0f;

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
            if (Time.timeScale == 0)
                return;

            UpdateView();

            if (Input.GetKeyDown(KeyCode.F))
            {
                _stableThrusters = !_stableThrusters;
                _rigidbody.useGravity = !_rigidbody.useGravity;
            }
            
            if(_stableThrusters)
                UpdateStableVelocity();
            else
                UpdateFreeVelocity();
        }

        void OnGUI()
        {
            UnityEngine.GUI.Label(new Rect(10, 10, 200, 20), "[F] Stable thrusters: " + _stableThrusters);
            UnityEngine.GUI.Label(new Rect(10, 50, 200, 20), "Speed: " + _rigidbody.velocity.magnitude);
        }

        private void UpdateStableVelocity()
        {
            _rigidbody.AddForce(-_rigidbody.velocity);

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

            _rigidbody.AddRelativeForce(velocity);

            if (Input.GetKey(KeyCode.Space))
                _rigidbody.AddForce(new Vector3(0, JumpSpeed, 0));

        }

        void UpdateView()
        {
            if (Input.GetKey(KeyCode.Q))
            {
                _rot += RotateSpeed;
            }
            if (Input.GetKey(KeyCode.E))
            {
                _rot -= RotateSpeed;
            }

            // Read the mouse input axis
            _rotationX += Input.GetAxis("Mouse X") * LookSensivity;
            _rotationY += Input.GetAxis("Mouse Y") * LookSensivity;
            
            //_rotationX = ClampAngle(_rotationX, minimumX, maximumX);
            //_rotationY = ClampAngle(_rotationY, minimumY, maximumY);

            Quaternion xQuaternion = Quaternion.AngleAxis(_rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(_rotationY, -Vector3.right);
            Quaternion zQuaternion = Quaternion.AngleAxis(_rot, Vector3.forward);

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
