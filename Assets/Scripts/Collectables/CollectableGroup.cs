using UnityEngine;

namespace Picker3D
{
    public class CollectableGroup : MonoBehaviour
    {
        public int Amount => collectables.Length;

        [SerializeField] private Collectable[] collectables;
        [SerializeField] private Vector3[] positions;

#if UNITY_EDITOR
        private void OnValidate()
        {
            collectables = GetComponentsInChildren<Collectable>();
            positions = new Vector3[collectables.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = collectables[i].transform.localPosition;
                collectables[i].Group = this;
            }
        }
#endif
        private void OnEnable()
        {
            Restart();
        }

        public void Restart()
        {
            for (int i = 0; i < collectables.Length; i++)
            {
                collectables[i].Restart(positions[i]);
            }
        }

    }
}
