using System;
using UnityEngine;

namespace Elementary.Scripts.LevelManagement
{
    public class LevelHapticController : MonoBehaviour
    {
        private void OnLevelComplete(Level level)
        {
        }

        private void OnLevelFail(Level level)
        {
        }

        private void OnLevelStageComplete(Level level)
        {
        }


        private void Start()
        {
            LevelManager.OnLevelComplete += OnLevelComplete;
            LevelManager.OnLevelFail += OnLevelFail;
            LevelManager.OnLevelStageComplete += OnLevelStageComplete;
        }


        private void OnDestroy()
        {
            LevelManager.OnLevelComplete -= OnLevelComplete;
            LevelManager.OnLevelFail -= OnLevelFail;
            LevelManager.OnLevelStageComplete -= OnLevelStageComplete;
        }
    }
}