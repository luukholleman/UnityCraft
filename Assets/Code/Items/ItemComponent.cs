using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Items
{
    class ItemComponent : MonoBehaviour
    {
        public Item Item;

        public Vector3 Rotation = new Vector3(10, -10, 10);

        public float Scale;

        public float PulseScale;

        private Vector3 _scale;

        public float PulseFrequency;

        private bool _pulsing;

        void Start()
        {
            GetComponent<MeshFilter>().mesh = Item.GetMesh();

            transform.rotation = Quaternion.Euler(new Vector3(10, -10, 10));

            _scale = new Vector3(Scale, Scale, Scale);

            transform.localScale = _scale;
        }

        void Update()
        {
            if (_pulsing)
            {
                float scale = Scale + ((float)(PulseScale * (1 + Math.Sin(2 * Math.PI * PulseFrequency * Time.timeSinceLevelLoad))) - PulseScale / 2);

                Vector3 scaleVector = new Vector3(scale, scale, scale);
                transform.localScale = scaleVector;
            }
        }

        public void StartPulsing()
        {
            _pulsing = true;
        }

        public void StopPulsing()
        {
            transform.localScale = _scale;

            _pulsing = false;
        }
    }
}
