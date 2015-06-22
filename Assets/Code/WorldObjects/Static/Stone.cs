using Assets.Code.Items;

namespace Assets.Code.WorldObjects.Static
{
    class Stone : StaticObject
    {
        public override Item GetItem()
        {
            return new Items.Blocks.Stone();
        }
    }
}
