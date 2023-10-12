using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3D
{
    public class LevelGenerator : MonoBehaviour
    {
        const float TILE_LENGTH = 10f; //tile base lenght
        const int MIN_TILE_BEFORE_GATE = 10; //tile amount between gates

        [Header("Events")]
        [SerializeField] private IntEventChannelSO levelLoadEventChannel;
        [SerializeField] private VoidEventChannelSO levelRestartEventChannel;

        [Header("Levels Data")]
        [SerializeField] private LevelData[] levels;

        [Header("Prefab References")]
        [SerializeField] private CollectableGroup[] collectableGroups;
        [SerializeField] private GameObject emptyTilePf;
        [SerializeField] private GameObject tileFinishPf;
        [SerializeField] private Gate gatePf;

        [Header("Platform Settings")]
        [SerializeField] private float tileStartingPositionZ = -10f;
        [SerializeField] private float collectableXRange = 3f;

        [Header("Random Level Generation Settings")]
        [SerializeField] int baseDifficulty = 10; // Collectable objeler için minimum sayý
        [SerializeField] int maxDifficulty = 50; // Collectable için max
        [SerializeField] int difficultyIncrease = 2; // Her seviyede collectable obje sayýsý x artýyor.

        private List<CollectableGroup> collectableGroupList = new();
        private List<Gate> gateList = new();
        private List<GameObject> tileList = new();
        private float zPointer;

        #region Events
        private void OnEnable()
        {
            levelLoadEventChannel.OnEventRaised += OnLevelLoad;
            levelRestartEventChannel.OnEventRaised += OnLevelRestart;
        }

        private void OnDisable()
        {
            levelLoadEventChannel.OnEventRaised -= OnLevelLoad;
            levelRestartEventChannel.OnEventRaised -= OnLevelRestart;
        }
        private void OnLevelRestart()
        {
            foreach (Gate gate in gateList)
            {
                gate.Restart();
            }
            foreach (CollectableGroup group in collectableGroupList)
            {
                group.Restart();
            }
        }


        private void OnLevelLoad(int level)
        {
            Clear();
            zPointer = tileStartingPositionZ;
            SpawnEmptyTiles(2);
            if (level >= levels.Length)
            {
                //random level!
                RandomLevelGeneration(level);
            }
            else
            {
                LevelData levelData = levels[level];
                GenerateLevelFromData(levelData);
            }
            SpawnEmptyTiles(2);
            SpawnTile(tileFinishPf);
        }
        #endregion
        #region Level Generation Methods
        private void GenerateLevelFromData(LevelData levelData)
        {
            for (int i = 0; i < levelData.Stages.Length; i++)
            {
                int requireAmount = levelData.Stages[i].GateRequireAmount;
                int collectableGroupLength = levelData.Stages[i].collectableGroups.Length;

                for (int j = 0; j < collectableGroupLength; j++)
                {
                    SpawnCollectableGroup(levelData.Stages[i].collectableGroups[j]);
                    SpawnTile(emptyTilePf);
                }
                SpawnTile(emptyTilePf);
                SpawnGate(requireAmount, i);
            }
        }
        private int GetRandomDifficulty(int level)
        {
            int difficulty = baseDifficulty + (level * difficultyIncrease);
            if (difficulty > maxDifficulty)
            {
                difficulty = maxDifficulty;
            }
            return Random.Range(baseDifficulty, difficulty);
        }

        private void RandomLevelGeneration(int level)
        {
            for (int i = 0; i < 3; i++)
            {
                int requireAmount = GetRandomDifficulty(level);
                int spawned = 0;
                for (int j = 0; j < MIN_TILE_BEFORE_GATE; j++)
                {
                    if (zPointer > 10f && (spawned < requireAmount || Random.value > .5f)) //TODO: %50 ihtimal sanki iyileþtirilebilir
                    {
                        spawned += SpawnRandomCollectableGroup();
                    }
                    SpawnTile(emptyTilePf);
                }
                SpawnGate(requireAmount, i);
            }
        }
        #endregion
        #region Spawner Methods
        private void SpawnEmptyTiles(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                SpawnTile(emptyTilePf);
            }
        }
        private int SpawnRandomCollectableGroup()
        {
            return SpawnCollectableGroup(collectableGroups[RandomCollectableGroupIndex()]).Amount;
        }
        private CollectableGroup SpawnCollectableGroup(CollectableGroup pf)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-collectableXRange, collectableXRange), 0, zPointer + TILE_LENGTH * 0.5f);
            CollectableGroup collectableGroup = LeanPool.Spawn(pf, spawnPos, Quaternion.identity);
            collectableGroupList.Add(collectableGroup);
            return collectableGroup;
        }


        private void SpawnTile(GameObject pf)
        {
            GameObject tile = LeanPool.Spawn(pf, new Vector3(0, 0, zPointer), Quaternion.identity);
            tileList.Add(tile);
            zPointer += TILE_LENGTH;
        }

        private void SpawnGate(int requireAmount, int gateIndex)
        {
            Gate gate = LeanPool.Spawn(gatePf, new Vector3(0, 0, zPointer), Quaternion.identity);
            gate.RequireAmount = requireAmount;
            gate.GateIndex = gateIndex;
            gateList.Add(gate);
            zPointer += TILE_LENGTH;
        }
        #endregion
        #region Helper Methos
        private void Clear()
        {
            LeanPool.DespawnAll();
            tileList.Clear();
            gateList.Clear();
            collectableGroupList.Clear();
        }
        private int RandomCollectableGroupIndex()
        {
            return Random.Range(0, collectableGroups.Length);
        }
        #endregion
    }
}
