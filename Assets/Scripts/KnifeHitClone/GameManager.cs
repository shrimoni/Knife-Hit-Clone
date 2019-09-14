using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KinfeHitClone
{
    public enum GameState
    {
        NONE = 0,
        LOADING = 1,
        MAIN_MENU = 2,
        GAME_PLAYING = 3,
        GAME_PAUSED = 4,
        EXIT_GAME = 5
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance; // Singleton

        public GameState gameState;

        [Header("Knife Variables")]
        [SerializeField]private GameObject knifePrefab;
        [SerializeField]private Transform knifeSpawnPoint;

        [Header("Score Variables")]
        public Text scoreText;
        private int score;

        private void Awake()
        {
            // Making sure the instance is not null
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            gameState = GameState.LOADING;
            score = 0;

            AddScore(score);
            SpawnKnife();
        }

        public void SpawnKnife()
        {
            if(knifePrefab == null)
            {
                Debug.Log("assign knife prefab in inspector");
                return;
            }

            var knife = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);
        }

        // Adds the score
        public void AddScore(int val)
        {
            if(scoreText == null)
            {
                Debug.Log("assign score text in the inspector");
                return;
            }
            score += val;
            scoreText.text = score.ToString();
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
