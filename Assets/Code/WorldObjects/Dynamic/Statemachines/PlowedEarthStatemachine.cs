using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic.Statemachines
{
    class PlowedEarthStatemachine : BaseStatemachine
    {
        private float _nextDryTime = float.PositiveInfinity;

        public override void Start()
        {

        }

        public override void Update()
        {
            if (Time.time >= _nextDryTime)
            {
                ((PlowedEarth) DynamicObject).Watered = false;

                Chunk.DoRebuild();

                _nextDryTime = float.PositiveInfinity;
            }
        }

        public override void Interact()
        {

        }

        public override void Action()
        {
            DropItem();

            Destroy();
        }

        public override void Destroy()
        {

        }

        public void SetWatered(bool watered)
        {
            if (watered)
                _nextDryTime = Time.time + Random.value * 3 + 5;
        }
    }
}
