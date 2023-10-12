using System.Collections.Generic;
using UnityEngine;

namespace Picker3D
{
    [RequireComponent(typeof(Rigidbody))]
    public class Picker : MonoBehaviour
    {
        [Header("Game Events")]
        [SerializeField] private VoidEventChannelSO gameStartEventChannel;
        [SerializeField] private VoidEventChannelSO gameOverEventChannel;
        [SerializeField] private VoidEventChannelSO gameReadyEventChannel;


        [Header("Input")]
        [SerializeField] private InputData inputData;


        [Header("Settings")]
        [SerializeField] private float clampX = 2.5f;
        [SerializeField] private float moveSpeed = 2.5f;
        [SerializeField] private float horizontalSpeed = 2.5f;

        private Rigidbody rb;
        private bool isStopped;

        private List<Collectable> collectedList;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            collectedList = new List<Collectable>();
        }

        private void OnEnable()
        {
            isStopped = true;
            gameStartEventChannel.OnEventRaised += Release;
            gameOverEventChannel.OnEventRaised += Stop;
            gameReadyEventChannel.OnEventRaised += Restart;
        }

        private void OnDisable()
        {
            gameStartEventChannel.OnEventRaised -= Release;
            gameOverEventChannel.OnEventRaised -= Stop;
            gameReadyEventChannel.OnEventRaised -= Restart;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Collectable collectable))
            {
                collectable.Collect();
                collectedList.Add(collectable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Collectable collectable))
            {
                collectable.Release();
                collectedList.Remove(collectable);
            }
        }

        private void FixedUpdate()
        {
            if (isStopped)
            {
                return;
            }
            Vector3 pos = rb.position;
            pos.z += moveSpeed * Time.deltaTime;
            pos.x = Mathf.Clamp(pos.x + inputData.Horizontal * Time.deltaTime * horizontalSpeed, -clampX, clampX);
            rb.MovePosition(pos);
        }


        public void Release()
        {
            isStopped = false;
        }
        public void Stop()
        {
            isStopped = true;
        }
        private void Restart()
        {
            rb.position = Vector3.zero;
            Stop();
        }

        public void PushCollectables()
        {
            foreach (Collectable collectable in collectedList)
            {
                collectable.Push();
            }
        }

    }
}
