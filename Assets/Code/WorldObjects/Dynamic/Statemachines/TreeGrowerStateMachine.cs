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
            _start = Time.realtimeSinceStartup;
        }

        public override void Update()
        {
            if (Time.realtimeSinceStartup < _start + 5)
            {
                CreateTree(new Position(GO.transform.position));
            }
        }

        private void CreateTree(Position position)
        {
            int treeHeight = (int)Math.Round(Random.value*5 + 3);

            for (int i = 0; i < treeHeight; i++)
            {
                Position treeblockPosition = new Position(position.x, position.y + i, position.z);

                DynamicObjectComponent.Chunk.SetObject(treeblockPosition, new Wood(), true);
            }

            Position treeTop = new Position(position);

            treeTop.y += treeHeight;

            int leafRadius = (int)Math.Round(Random.value * treeHeight / 2f);

            for (int x = treeTop.x - leafRadius; x < treeTop.x + leafRadius; x++)
            {
                for (int y = treeTop.y - leafRadius; y < treeTop.y + leafRadius; y++)
                {
                    for (int z = treeTop.z - leafRadius; z < treeTop.z + leafRadius; z++)
                    {
                        Position leafPosition = new Position(x, y, z);

                        if (Vector3.Distance(treeTop.ToVector3(), leafPosition.ToVector3()) < leafRadius - (int)Math.Round(Random.value))
                        {
                            DynamicObjectComponent.Chunk.SetObject(leafPosition, new Leaves(), true);
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

        }

        public override void OnGUI()
        {

        }
    }
}
