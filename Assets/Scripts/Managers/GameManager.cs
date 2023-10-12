using UnityEngine;
namespace Picker3D
{
    ///TODO: InputManager touch ile kontrol yapılaca
    ///LevelGenerator random level methodu yazılacak
    ///Mobil test!
    ///Optimizasyon
    public class GameManager : MonoBehaviour
    {
        const string SAVE_FILE = "Picker3DSave";

        [SerializeField] private VoidEventChannelSO gameReadyEventChannel;
        [SerializeField] private VoidEventChannelSO gameStartEventChannel;
        [SerializeField] private VoidEventChannelSO gameWinEventChannel;
        [SerializeField] private VoidEventChannelSO gameFailEventChannel;
        [SerializeField] private IntEventChannelSO levelLoadEventChannel;
        [SerializeField] private VoidEventChannelSO levelNextEventChannel;
        [SerializeField] private VoidEventChannelSO levelRestartEventChannel;
        [SerializeField] private InputData input;

        private GameData gameData;

        #region MonoBehavior Methods
        private void Awake()
        {
#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            LoadGameData();
        }

        private void OnDestroy()
        {
            SaveGameData();
        }

        private void Start()
        {
            levelLoadEventChannel.RaiseEvent(gameData.Level);
            //gameReadyEventChannel.RaiseEvent();
        }
        private void OnEnable()
        {
            input.TouchStart += OnFirstTouch;
            gameWinEventChannel.OnEventRaised += OnGameWin;
            levelNextEventChannel.OnEventRaised += OnNextLevel;
            levelRestartEventChannel.OnEventRaised += OnLevelRestart;
        }
        private void OnDisable()
        {
            input.TouchStart -= OnFirstTouch;
            gameWinEventChannel.OnEventRaised -= OnGameWin;
            levelNextEventChannel.OnEventRaised -= OnNextLevel;
            levelRestartEventChannel.OnEventRaised -= OnLevelRestart;
        }
        #endregion
        #region Events
        private void OnLevelRestart()
        {
            input.TouchStart += OnFirstTouch;
            gameReadyEventChannel.RaiseEvent();
        }

        private void OnNextLevel()
        {
            input.TouchStart += OnFirstTouch;
            levelLoadEventChannel.RaiseEvent(gameData.Level);
            gameReadyEventChannel.RaiseEvent();
        }

        private void OnFirstTouch()
        {
            input.TouchStart -= OnFirstTouch;
            gameStartEventChannel.RaiseEvent();
        }

        private void OnGameWin()
        {
            gameData.IncreaseLevel();
            SaveGameData();
        }
        #endregion
        #region Save&Load
        private void LoadGameData()
        {
            gameData = SaveIO.LoadData<GameData>(SAVE_FILE);
            if (gameData is null)
            {
                gameData = new GameData();
            }
        }
        private void SaveGameData()
        {
#if UNITY_EDITOR
            if (doNotSave)
            {
                doNotSave = false;
                return;
            }
#endif
            SaveIO.SaveData(gameData, SAVE_FILE);
        }

#if UNITY_EDITOR
        bool doNotSave;
        [ContextMenu(nameof(DeleteSave))]
        private void DeleteSave()
        {
            doNotSave = true;

            SaveIO.DeleteDataFile(SAVE_FILE);
            if (Application.isPlaying)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }
#endif
        #endregion
    }

}
