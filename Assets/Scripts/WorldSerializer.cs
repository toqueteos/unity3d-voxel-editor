using UnityEngine;
using System.IO;
using System.IO.Compression;

namespace Voxels
{
    public static class WorldSerializer
    {
        public static string Filename(string filename)
        {
            if (!Directory.Exists(MapConstants.SaveFiles))
            {
                Directory.CreateDirectory(MapConstants.SaveFiles);
            }

            return string.Format("{0}/{1}.{2}", MapConstants.SaveFiles, filename, MapConstants.SaveExtension);
        }

        public static void Save(WorldData world)
        {
            var filepath = Filename(world.name);

            using (var stream = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var gzipStream = new GZipStream(stream, CompressionMode.Compress))
            using (var writer = new BinaryWriter(gzipStream))
            {
                var total = world.chunks.Count;
                writer.Write(total);

                foreach (var pair in world.chunks)
                {
                    SaveChunk(writer, pair.Value);
                }
            }
        }

        public static WorldData Load(string filename)
        {
            var filepath = Filename(filename);
            var world = new WorldData();

            using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var gzipStream = new GZipStream(stream, CompressionMode.Decompress))
            using (var reader = new BinaryReader(gzipStream))
            {
                var total = reader.ReadInt32();

                for (int i = 0; i < total; i++)
                {
                    var chunk = LoadChunk(reader);
                    world.chunks.Add(chunk.pos, chunk);
                }
            }

            return world;
        }

        private static void SaveChunk(BinaryWriter writer, ChunkData chunk)
        {
            writer.Write(chunk.pos.x);
            writer.Write(chunk.pos.y);
            writer.Write(chunk.pos.z);

            for (int z = 0; z < MapConstants.ChunkSize; z++)
            {
                for (int y = 0; y < MapConstants.ChunkSize; y++)
                {
                    for (int x = 0; x < MapConstants.ChunkSize; x++)
                    {
                        writer.Write((int)chunk.GetBlock(x, y, z));
                    }
                }
            }
        }

        private static ChunkData LoadChunk(BinaryReader reader)
        {
            var wx = reader.ReadInt32();
            var wy = reader.ReadInt32();
            var wz = reader.ReadInt32();

            var chunk = new ChunkData(wx, wy, wz);
            for (int z = 0; z < MapConstants.ChunkSize; z++)
            {
                for (int y = 0; y < MapConstants.ChunkSize; y++)
                {
                    for (int x = 0; x < MapConstants.ChunkSize; x++)
                    {
                        chunk.SetBlock(x, y, z, BlockData.Get(reader.ReadInt32()));
                    }
                }
            }

            return chunk;
        }
    }
}
