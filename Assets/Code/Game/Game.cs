using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.Items.Blocks;
using Assets.Code.Items.Tools;
using Assets.Code.Items.Usables;
using Assets.Code.Messenger;
using Assets.Code.Player;
using UnityEngine;

namespace Assets.Code.Game
{
    class Game : MonoBehaviour
    {
        public GameObject Player;

        void Start()
        {
            Player.SetActive(false);

            StartCoroutine("SetPlayerActive");
        }

        IEnumerator SetPlayerActive()
        {
            for (;;)
            {
                if (Physics.Raycast(Player.transform.position, Vector3.down, 100, LayerMask.GetMask("Chunks")))
                {
                    Player.SetActive(true);

                    Postman.Broadcast<Item>("picked up item", new Seeder());
                    Postman.Broadcast<Item>("picked up item", new Seeder());
                    Postman.Broadcast<Item>("picked up item", new Seeder());
                    Postman.Broadcast<Item>("picked up item", new Seeder());
                    Postman.Broadcast<Item>("picked up item", new Seeder());
                    Postman.Broadcast<Item>("picked up item", new Seeder());

                    break;
                }

                yield return null;
            }
        }

        void Update()
        {

        }
    }
}
