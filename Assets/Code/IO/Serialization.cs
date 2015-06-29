using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects;

namespace Assets.Code.IO
{
    public static class Serialization
    {
        public static string SaveFolderName = "SaveGames";

        public static string SaveLocation(string worldName)
        {
            string saveLocation = SaveFolderName + "/" + worldName + "/";

            if (!Directory.Exists(saveLocation))
            {
                Directory.CreateDirectory(saveLocation);
            }

            return saveLocation;
        }

        public static string FileName(Position chunkLocation)
        {
            string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";

            return fileName;
        }

        public static void SaveChunk(Chunk chunk)
        {
            Save save = new Save(chunk);
            if (save.blocks.Count == 0)
                return;

            string saveFile = SaveLocation(chunk.World.WorldName);
            saveFile += FileName(chunk.Position);

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, save);
            stream.Close();
        }

        public static bool Load(Chunk chunk)
        {
            string saveFile = SaveLocation(chunk.World.WorldName);
            saveFile += FileName(chunk.Position);

            if (!File.Exists(saveFile))
                return false;

            IFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveFile, FileMode.Open);

            Save save = (Save) formatter.Deserialize(stream);

            foreach (KeyValuePair<Position, WorldObject> block in save.blocks)
            {
                //chunk.ChunkData.GetStaticObjects[block.Key] = block.Value;
            }

            stream.Close();
            return true;
        }
    }
}
