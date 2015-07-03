using System;
using Assets.Code.GenerationEngine;
using Assets.Code.WorldObjects.Static.Plants;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code.WorldObjects.Dynamic.Statemachines
{
    class TreeGrowerStateMachine : BaseStatemachine
    {
        private float _start;

        public override void Start()
        {
            _start = Time.time;
        }

        public override void Update()
        {
            if (Time.time > _start + 5)
            {
                DynamicObjectComponent.Chunk.RemoveObject(new Position(DynamicObjectComponent.transform.position));

                CreateTree(new Position(GameObject.transform.position));

                GameObject.Destroy(DynamicObjectComponent.gameObject);
            }
        }

        private void CreateTree(Position position)
        {
            int treeHeight = (int)Math.Round(Random.value*3 + 20);

            for (int i = 0; i < treeHeight; i++)
            {
                Position treeblockPosition = new Position(position.x, position.y + i, position.z);

                DynamicObjectComponent.Chunk.SetObject(treeblockPosition, new Wood());
            }

            Position treeTop = new Position(position);

            treeTop.y += treeHeight;

            int leafRadius = (int)(Math.Round(Random.value * 2) + treeHeight / 2f + 1);

            for (int x = treeTop.x - leafRadius; x < treeTop.x + leafRadius; x++)
            {
                for (int y = treeTop.y - leafRadius; y < treeTop.y + leafRadius; y++)
                {
                    for (int z = treeTop.z - leafRadius; z < treeTop.z + leafRadius; z++)
                    {
                        Position leafPosition = new Position(x, y, z);

                        if (Vector3.Distance(treeTop.ToVector3(), leafPosition.ToVector3()) < leafRadius - (int)Math.Round(Random.value))
                        {
                            DynamicObjectComponent.Chunk.SetObject(leafPosition, new Leaves());
                        }
                    }
                }
            }
        }

        public override void Interact()
        {

        }

        public override void Action()
        {
            Destroy();
        }

        public override void Destroy()
        {
            
        }
    }
}
