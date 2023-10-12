using UnityEngine;

namespace Picker3D
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float pushForce = 3f;
        [HideInInspector] public CollectableGroup Group;

#if UNITY_EDITOR
        private void OnValidate()
        {
            rb = GetComponent<Rigidbody>();
        }
#endif
        private void Awake()
        {

            if (Group is null)
            {
                Debug.LogError("Group null");
            }
        }
        public void Collect()
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
        public void Release()
        {
            rb.constraints = RigidbodyConstraints.None;
        }
        public void DelayedDestroy()
        {
            Invoke(nameof(Hide), 3f);
        }
        private void Hide()
        {
            gameObject.SetActive(false);
        }
        public void Restart(Vector3 position)
        {
            Release();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.localPosition = position;
            gameObject.SetActive(true);
        }
        public void Push()
        {
            rb.AddForce(Vector3.forward * pushForce, ForceMode.VelocityChange);
        }
    }
}
