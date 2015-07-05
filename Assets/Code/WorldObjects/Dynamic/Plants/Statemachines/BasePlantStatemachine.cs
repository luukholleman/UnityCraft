using Assets.Code.Items.Blocks;
using Assets.Code.WorldObjects.Dynamic.Statemachines;
using Assets.Code.WorldObjects.Static;
using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic.Plants.Statemachines
{
    abstract class BasePlantStatemachine : BaseStatemachine
    {
        private float _nextGrowTime;

        private float _currentGrowTime;

        protected PlowedEarth Earth;

        protected BasePlant Plant;

        protected abstract int[] GrowTimes { get; set; }

        protected virtual int MaxGrowthLevel { get; set; }

        public override void Start()
        {
            _nextGrowTime = GrowTimes[0];
            _currentGrowTime = 0;

            Earth = DynamicObjectComponent.Chunk.GetObject(new Position(DynamicObjectComponent.Position) { y = DynamicObjectComponent.Position.y - 1 }) as PlowedEarth;
        }

        public override void Update()
        {
            if (Earth.Watered)
            {
                _currentGrowTime += Time.deltaTime;

                if (_currentGrowTime >= _nextGrowTime)
                {
                    Plant.GrowLevel = Mathf.Clamp(++Plant.GrowLevel, 0, MaxGrowthLevel);

                    DynamicObjectComponent.Chunk.DoRebuild();

                    _currentGrowTime = 0;

                    _nextGrowTime = GrowTimes[Plant.GrowLevel];
                }
            }

            WorldObject block = Chunk.GetObject(new Position(DynamicObjectComponent.Position) {y = DynamicObjectComponent.Position.y - 1});

            if (block == null || block is Air)
            {
                Destroy();
            }
        }

        public override void Destroy()
        {
            Object.Destroy(GameObject);
        }

        public override void Action()
        {
            Destroy();
        }
    }
}
