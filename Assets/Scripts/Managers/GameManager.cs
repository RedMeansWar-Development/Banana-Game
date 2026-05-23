using BananaGame.BananaTime;
using UnityEngine;

namespace BananaGame.Managers;

 public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
 
        [Header("Eras")]
        public EraDefinition[] allEras;
 
        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
 
        public EraDefinition GetEraByName(string eraName)
        {
            if (allEras == null) return null;
            foreach (var era in allEras)
                if (era.eraName == eraName) return era;
            return null;
        }
 
        public void PauseGame()  => Time.timeScale = 0f;
        public void ResumeGame() => Time.timeScale = 1f;
 
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }