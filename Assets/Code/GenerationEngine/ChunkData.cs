using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.WorldObjects;
using Assets.Code.WorldObjects.Dynamic;
using Assets.Code.WorldObjects.Static;
using UnityEngine;

namespace Assets.Code.GenerationEngine
{
    public class ChunkData
    {
        public Position Position;

        private Dictionary<Position, StaticObject> _staticObjects = new Dictionary<Position, StaticObject>();
        private Dictionary<Position, DynamicObject> _dynamicObjects = new Dictionary<Position, DynamicObject>();
        
        public ChunkData(Position position)
        {
            Position = position;
        }

        public void SetObject(Position position, WorldObject staticObject, bool replaceBlocks = false)
        {
            bool emptyOrAir = !_staticObjects.ContainsKey(position - Position) || _staticObjects[position - Position] is Air;

            if (((emptyOrAir && !_dynamicObjects.ContainsKey(position - Position)) || replaceBlocks) && Helper.InOuterChunk(position - Position))
            {
                if(staticObject is StaticObject)
                    _staticObjects[position - Position] = (StaticObject)staticObject;
                else if (staticObject is DynamicObject)
                    _dynamicObjects[position - Position] = (DynamicObject)staticObject;
                else
                    Debug.LogWarning("Object not of type static or dynamic");
            }
        }

        public bool HasObjectAtPosition(Position position)
        {
            if(_staticObjects.ContainsKey(position))
            {
                if (_staticObjects[position] is Air)
                {
                    return false;
                }
                return true;
            }

            return _dynamicObjects.ContainsKey(position);
        }

        public WorldObject GetObject(Position position)
        {
            if (_staticObjects.ContainsKey(position))
            {
                return _staticObjects[position];
            }

            if (_dynamicObjects.ContainsKey(position))
            {
                return _dynamicObjects[position];
            }

            return null;
        }

        public void RemoveObject(Position position)
        {
            Position pos = new Position(position - Position);

            if (_staticObjects.ContainsKey(pos))
                _staticObjects.Remove(pos);
            if (_dynamicObjects.ContainsKey(pos))
                _dynamicObjects.Remove(pos);
        }

        public void SetBlocksUnmodified()
        {
            //foreach (KeyValuePair<Position, StaticObject> block in _staticObjects)
            //{
            //    block.Value.Changed = false;
            //}

            //foreach (KeyValuePair<Position, DynamicObject> block in _dynamicObjects)
            //{
            //    block.Value.Changed = false;
            //}
        }

        public Dictionary<Position, StaticObject> GetStaticObjects()
        {
            return _staticObjects;
        }

        public Dictionary<Position, DynamicObject> GetDynamicObjects()
        {
            return _dynamicObjects;
        }
    }
}
