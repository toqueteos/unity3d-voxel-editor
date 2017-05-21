using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Voxels
{
    public class MapEditor : MonoBehaviour
    {
        protected static MapEditor instance;

        public static MapEditor Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<MapEditor>();

                    if (instance == null)
                    {
                        Debug.LogErrorFormat("An instance of {0} is needed in the scene, but there is none.", typeof(MapEditor));
                    }
                }

                return instance;
            }
        }

        public World world;

        [HideInInspector]
        public WorldData worldData;

        static bool burst = false;

        void Start()
        {
            Assert.raiseExceptions = true;

            world = GetComponent<World>();
            worldData = new WorldData();

            world.data = worldData;

            CreateStartingChunk();
        }

        private void CreateStartingChunk()
        {
            var chunk = world.CreateChunk(0, 0, 0);
            world.SetBlock(0, 0, 0, BlockId.Stone);
            chunk.Render();
        }

        public void SetWorldName(string name)
        {
            worldData.name = name;
        }

        public void SaveWorld()
        {
            WorldSerializer.Save(worldData);
        }

        public void LoadWorld(string name)
        {
            worldData = WorldSerializer.Load(name);
        }

        public void ResetWorld(Text text)
        {
            worldData = new WorldData();
            text.text = worldData.name;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                burst = !burst;
            }

            if (Press(KeyCode.Escape)) Debug.Break();
        }

        public static bool Press(KeyCode keyCode)
        {
            return burst ? Input.GetKey(keyCode) : Input.GetKeyDown(keyCode);
        }
    }
}
