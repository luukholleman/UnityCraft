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

        void Start()
        {
            GetComponent<MeshFilter>().mesh = Item.GetMesh();

            GetComponent<Rigidbody>().AddTorque(new Vector3(10, 10, 10));
        }
    }
}
