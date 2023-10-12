using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Picker3D
{
    public class GamePanel : CanvasBase
    {
        [Header("Events")]
        [SerializeField] private VoidEventChannelSO gameStartEventChannel;
        [SerializeField] private VoidEventChannelSO gameWinEventChannel;
        [SerializeField] private VoidEventChannelSO gameFailEventChannel;
        [SerializeField] private GateEventChannelSO gateEventChannel;
        [SerializeField] private IntEventChannelSO levelLoadEventChannel;

        [Header("References")]
        [SerializeField] private Image[] gateImages;
        [SerializeField] private TMP_Text currentLevelText;
        [SerializeField] private TMP_Text nextLevelText;
        private void OnEnable()
        {
            gameStartEventChannel.OnEventRaised += OnGameStart;
            gameWinEventChannel.OnEventRaised += OnGameOver;
            gameFailEventChannel.OnEventRaised += OnGameOver;
            gateEventChannel.OnEventRaised += OnGatePass;
            levelLoadEventChannel.OnEventRaised += OnLevelLoad;
        }

        private void OnDisable()
        {
            gameStartEventChannel.OnEventRaised -= OnGameStart;
            gameWinEventChannel.OnEventRaised -= OnGameOver;
            gameFailEventChannel.OnEventRaised += OnGameOver;
            gateEventChannel.OnEventRaised -= OnGatePass;
            levelLoadEventChannel.OnEventRaised -= OnLevelLoad;
        }

        private void OnLevelLoad(int level)
        {
            currentLevelText.SetText((level + 1).ToString());
            nextLevelText.SetText((level + 2).ToString());
        }

        private void OnGatePass(Gate gate, int gateIndex)
        {
            gateImages[gateIndex].color = Color.green;
        }

        private void OnGameOver()
        {
            Hide();
        }

        private void OnGameStart()
        {
            foreach (var gateImage in gateImages)
            {
                gateImage.color = Color.white;
            }
            Show();
        }
    }
}
