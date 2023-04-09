using Elementary.Scripts.Data.Management;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace Elementary.Scripts.LevelManagement
{
    public class LevelManager : MonoBehaviour
    {
        #region EVENTS

        /// <summary>
        ///     Level Load Complete
        /// </summary>
        public static UnityAction<Level> OnLevelLoadComplete;

        /// <summary>
        ///     Level Start
        /// </summary>
        public static UnityAction<Level> OnLevelStart;

        /// <summary>
        ///     Level Stage Complete
        /// </summary>
        public static UnityAction<Level> OnLevelStageComplete;

        /// <summary>
        ///     Level Complete
        /// </summary>
        public static UnityAction<Level> OnLevelComplete;

        /// <summary>
        ///     Level Fail
        /// </summary>
        public static UnityAction<Level> OnLevelFail;

        #endregion

        #region PUBLIC FIELDS / PROPS

        public static LevelManager Instance { get; private set; }

        #endregion

        #region SERIALIZE FIELDS

        // Level source
        [SerializeField] private LevelSource levelSource;

        // Level spawn point and container
        [SerializeField] private Transform levelContainerTransform;

        // If all levels are completed random start index for next level 
        [SerializeField] private int loopLevelsStartIndex = 1;

        // Set loop level usage random or not. 
        [SerializeField] private bool loopLevelGetRandom = true;

        #endregion

        #region PRIVATE FIELDS

        private GameObject _activeLevel;

        private const string LevelIndexKey = "level-index";
        private const string LevelNumberKey = "level-number";

        private bool _onEnd;
        #endregion

        #region PRIVATE METHODS

        private GameObject GetLevel()
        {
            int currentLevelIndex = DataManager.Get<int>(LevelIndexKey);
            int currentLevelNumber = DataManager.Get<int>(LevelNumberKey);

            // Level number cannot be start 0
            if (currentLevelNumber == 0) currentLevelNumber = 1;

            // If all level is completed, set random level index;
            if (currentLevelIndex >= levelSource.levelData.Length)
            {
                if (loopLevelGetRandom)
                {
                    currentLevelIndex = Random.Range(loopLevelsStartIndex, levelSource.levelData.Length - 1);
                    DataManager.Save(LevelIndexKey, currentLevelIndex);
                }
                else 
                {
                    currentLevelIndex = levelSource.levelData.Length - 1;
                }
            }

            GameObject level = levelSource.levelData[currentLevelIndex];
            Level levelData = level.GetComponent<Level>();
            levelData.SetLevelData(currentLevelIndex, currentLevelNumber);

            return level;
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        ///  Loads the next level
        /// </summary>
        public void LevelLoad()
        {
            _activeLevel = Instantiate(GetLevel(), levelContainerTransform.transform, false);
            OnLevelLoadComplete?.Invoke(_activeLevel.GetComponent<Level>());
        }

        /// <summary>
        ///  Called to start a level
        /// </summary>
        public void LevelStart()
        {
            OnLevelStart?.Invoke(_activeLevel.GetComponent<Level>());
        }

        /// <summary>
        ///     If a level consists of different stages, this method is called
        ///     when each stage is completed and the stage information is stored.
        /// </summary>
        /// <param name="stageNumber"></param>
        public void LevelStageComplete(int stageNumber = 1)
        {
            Level levelData = _activeLevel.GetComponent<Level>();
            levelData.SetLevelStageNumber(stageNumber);
            OnLevelStageComplete?.Invoke(levelData);
        }

        /// <summary>
        /// This method is called when the active level is completed
        /// </summary>
        public void LevelComplete()
        {
            if (_onEnd) return;
            Level levelData = _activeLevel.GetComponent<Level>();
            int currentLevelIndex = levelData.LevelIndex + 1;
            int currentLevelNumber = levelData.LevelNumber + 1;

            DataManager.Save(LevelIndexKey, currentLevelIndex);
            DataManager.Save(LevelNumberKey, currentLevelNumber);

            OnLevelComplete?.Invoke(_activeLevel.GetComponent<Level>());
            _onEnd = true;
        }

        /// <summary>
        ///  This method is called when the active level is failed
        /// </summary>
        public void LevelFail()
        {
            if (_onEnd) return;
            OnLevelFail?.Invoke(_activeLevel.GetComponent<Level>());
            _onEnd = true;
        }

        public void ResetLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        #endregion

        #region UNITY EVENT METHODS

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private void Start() => LevelLoad();

        #endregion
    }
}