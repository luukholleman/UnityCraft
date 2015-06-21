using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;

namespace Assets.Code.Blocks
{
    class Stone : Block
    {
        public override Item GetItem()
        {
            return new Items.Blocks.Stone();
        }
    }
}
