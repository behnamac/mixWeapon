using UnityEngine;

namespace Elementary.Scripts.Log
{
    public static class Log
    {
        #if UNITY_EDITOR

        public static void Write(string message, object obj) => Debug.Log($"JOB {obj} - {message}");
        public static void WriteError(string message, object obj) => Debug.LogError($"JOB {obj} - {message}");
        public static void WriteWarning(string message, object obj) => Debug.LogWarning($"JOB {obj} - {message}");

        #endif
    }
}