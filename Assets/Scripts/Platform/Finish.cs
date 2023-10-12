using UnityEngine;
namespace Picker3D
{
    public class Finish : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO gameWinEventChannel;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PickerCollider"))
            {
                gameWinEventChannel.RaiseEvent();
            }
        }
    }
}
