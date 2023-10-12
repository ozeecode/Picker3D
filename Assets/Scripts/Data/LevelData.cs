using System;
using UnityEngine;

namespace Picker3D
{
    [CreateAssetMenu(menuName = "Picker3D/Level Data")]
    public class LevelData : ScriptableObject
    {
        public Stage[] Stages;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Stages.Length > 3)
            {
                Debug.LogWarning("Stage size must be 3!");

            }
            for (int i = 0; i < Stages.Length; i++)
            {
                int total = 0;
                foreach (CollectableGroup group in Stages[i].collectableGroups)
                {
                    total += group.Amount;
                }
                Stages[i].GateRequireAmount = total - Stages[i].WasteAmount;
            }
        }
#endif
    }

    [Serializable]
    public struct Stage
    {
        public int GateRequireAmount;
        public int WasteAmount;
        public CollectableGroup[] collectableGroups;
    }

}