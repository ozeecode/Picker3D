using UnityEngine;

namespace Picker3D
{
    public class GameWinScreen : CanvasBase
    {
        [SerializeField] private VoidEventChannelSO gameWinEventChannel;
        [SerializeField] private float showDelay = 2f;
        private void OnEnable()
        {
            gameWinEventChannel.OnEventRaised += OnGameWin;
        }
        private void OnDisable()
        {
            gameWinEventChannel.OnEventRaised -= OnGameWin;
        }

        private void OnGameWin()
        {
            Invoke(nameof(Show), showDelay);
        }
    }
}
