using DG.Tweening;
using UnityEngine;


namespace Picker3D
{
    public class StartScreen : CanvasBase
    {
        [Header("Game Events")]
        [SerializeField] private VoidEventChannelSO gameStartEventChannel;
        [SerializeField] private VoidEventChannelSO gameReadyEventChannel;

        [SerializeField] private RectTransform handTransform;

        private Vector2 handStartingPosition;


        protected override void Awake()
        {
            base.Awake();
            handStartingPosition = handTransform.anchoredPosition;
        }
        private void OnEnable()
        {
            gameStartEventChannel.OnEventRaised += OnGameStart;
            gameReadyEventChannel.OnEventRaised += OnGameReady;
        }
        private void OnDisable()
        {
            gameStartEventChannel.OnEventRaised += OnGameStart;
            gameReadyEventChannel.OnEventRaised += OnGameReady;
        }
        private void OnGameReady()
        {
            Show();
        }
        private void OnGameStart()
        {
            Hide();
        }

        public override void Show()
        {
            base.Show();
            handTransform.DOKill();
            handTransform.anchoredPosition = handStartingPosition;
            handTransform.DOAnchorPos3DX(handStartingPosition.x * -1f, 1f).SetLoops(-1, LoopType.Yoyo);

        }
    }
}
