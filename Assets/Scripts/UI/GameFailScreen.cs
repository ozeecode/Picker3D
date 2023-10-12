using UnityEngine;

namespace Picker3D
{
    public class GameFailScreen : CanvasBase
    {
        [SerializeField] private VoidEventChannelSO gameFailedEventChannel;
        [SerializeField] private float showDelay = 2f;
        private void OnEnable()
        {
            gameFailedEventChannel.OnEventRaised += OnGameFail;
        }
        private void OnDisable()
        {
            gameFailedEventChannel.OnEventRaised -= OnGameFail;
        }

        private void OnGameFail()
        {
            Invoke(nameof(Show), showDelay);
        }
    }
}
