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
        public Camera Cam;

        public float FlySpeed = 10f;
        public float MoveAccelerationSpeed = 10f;
        public float MaxMoveSpeed = 10f;
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

        private float _distToGround;

        void Start()
        {
            if (GetComponent<Rigidbody>())
                GetComponent<Rigidbody>().freezeRotation = true;

            _originalRotation = transform.localRotation;

            _rigidbody = GetComponent<Rigidbody>();

            _distToGround = GetComponent<Collider>().bounds.extents.y;
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
        }

        void FixedUpdate()
        {
            if (_stableThrusters)
                UpdateJetpackVelocity();
            else
                UpdateWalkVelocity();
        }

        void OnGUI()
        {
            UnityEngine.GUI.Label(new Rect(10, 10, 200, 20), "[F] Stable thrusters: " + _stableThrusters);
            UnityEngine.GUI.Label(new Rect(10, 50, 200, 20), "Speed: " + _rigidbody.velocity.magnitude);
        }

        private void UpdateJetpackVelocity()
        {
            _rigidbody.AddForce(-_rigidbody.velocity);

            Vector3 velocity = new Vector3();

            velocity.x = Input.GetAxis("Horizontal") * FlySpeed;
            velocity.z = Input.GetAxis("Vertical") * FlySpeed;

            if (Input.GetKey(KeyCode.Space))
                velocity.y = FlySpeed;

            velocity *= Time.deltaTime;

            _rigidbody.AddRelativeForce(-velocity.normalized * FlySpeed * (FlySpeed / 3));
        }

        private void UpdateWalkVelocity()
        {
            Vector3 velocity = new Vector3();

            velocity.x = Input.GetAxis("Horizontal") * MoveAccelerationSpeed;
            velocity.z = Input.GetAxis("Vertical") * MoveAccelerationSpeed;

            velocity = velocity.normalized*MaxMoveSpeed*_rigidbody.drag;

            if (Input.GetKey(KeyCode.Space) && IsGrounded())
                velocity.y = -JumpSpeed;

            _rigidbody.AddRelativeForce(-velocity, ForceMode.Acceleration);
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

            Quaternion xQuaternion = Quaternion.AngleAxis(_rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(_rotationY, -Vector3.right);

            Cam.transform.localRotation = _originalRotation * yQuaternion;
            transform.rotation = _originalRotation * xQuaternion;
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }

        bool IsGrounded()
        {
            return Physics.Raycast(transform.position, -Vector3.up, _distToGround + 0.1f);
        }

    }
}
