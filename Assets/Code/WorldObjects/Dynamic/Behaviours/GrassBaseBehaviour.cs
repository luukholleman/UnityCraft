using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic.Behaviours
{
    class GrassBaseBehaviour : BaseBehaviour
    {
        private float time;

        public override void Start()
        {
            time = Time.timeSinceLevelLoad;
        }

        public override void Update()
        {
            if (Time.timeSinceLevelLoad > time + 10)
            {
                DynamicObjectComponent.DynamicObject = new Flower();

                DynamicObjectComponent.BuildMesh();
            }
        }

        public override void Interact()
        {

        }

        public override void Action()
        {
            GameObject.Destroy(DynamicObjectComponent.gameObject);
        }

        public override void OnGUI()
        {
            
        }
    }
}
