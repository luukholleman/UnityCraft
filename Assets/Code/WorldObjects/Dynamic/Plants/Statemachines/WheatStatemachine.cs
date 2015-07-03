using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic.Plants.Statemachines
{
    class WheatStatemachine : BasePlantStatemachine
    {
        private int[] _growTimes = { 5, 5, 5, 10, 10, 10, 20 };

        protected override int[] GrowTimes
        {
            get { return _growTimes; }
            set { _growTimes = value; }
        }

        public override void Start()
        {
            base.Start();

            MaxGrowthLevel = 7;

            Plant = DynamicObject as Wheat;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Interact()
        {

        }

        public override void Action()
        {
            DropItem();
        }

        public override void Destroy()
        {

        }
    }
}
