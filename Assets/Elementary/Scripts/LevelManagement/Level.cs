using UnityEngine;

namespace Elementary.Scripts.LevelManagement
{
    public class Level : MonoBehaviour
    {
        #region PUBLIC FIELDS

        public int LevelIndex => _levelIndex;
        public int LevelNumber => _levelNumber;
        public int LevelStageNumber => _levelStageNumber;

        #endregion

        #region FIELDS

        [Header("INFO")]
        [SerializeField] private int _levelIndex;

        [SerializeField] private int _levelNumber;
        [SerializeField] private int _levelStageNumber;

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PUBLIC METHODS

        public void SetLevelData(int index, int number)
        {
            _levelIndex = index;
            _levelNumber = number;
        }

        public void SetLevelStageNumber(int stageNumber) => _levelStageNumber = stageNumber;

        #endregion
    }
}