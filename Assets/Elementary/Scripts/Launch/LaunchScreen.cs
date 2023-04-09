using UnityEngine;
using UnityEngine.SceneManagement;

namespace Elementary.Scripts.Launch
{
    public class LaunchScreen : MonoBehaviour
    {
        [SerializeField] private float waitTime = 3;
        [SerializeField] private int redirectSceneIndex = 1;
        
        private void StartRedirectProgress()
        {
            SceneManager.LoadScene(redirectSceneIndex, LoadSceneMode.Single);
        }

        private void Start() => Invoke(nameof(StartRedirectProgress), waitTime);
    }
}