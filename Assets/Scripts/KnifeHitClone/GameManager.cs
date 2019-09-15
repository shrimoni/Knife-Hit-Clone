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
        GAME_OVER = 5,
        EXIT_GAME = 6
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance; // Singleton

        public GameState gameState;

        [Header("Knife Variables")]
        public List<GameObject> inGameKnives;
        [SerializeField] private GameObject knifePrefab;
        public Transform knifeSpawnPoint;

        [Header("Wooden Log")]
        public GameObject inGameWoodLog;
        [SerializeField] private GameObject woodenLogPrefab;
        public Transform woodenLogSpawnPoint;

        [Header("Menu")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject gamePlayMenu;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private GameObject gameOverMenu;
        [SerializeField] private GameObject gameWinningMenu;

        [Header("GameOverMenu Variables")]
        public Text gameOverScore;
        public Text stage;

        [Header("Score Variables")]
        public Text scoreText;
        private int score;

        [Header("Stages")]
        public List<Stage> stages = new List<Stage>();
        public GameObject knifeIndicatorPrefab;
        public Transform knifeIndicatorParent;
        public List<GameObject> knifeIndicators = new List<GameObject>();
        public Sprite knifeIndicatorBlue;
        public Sprite knifeIndicatorBlack;
        public Stage currentStage;
        public Level currentLevel;
        public Text stageText;
        public Text levelText;

        [Header("Audio Variables")]
        private AudioSource gameAudio;
        public AudioSource knifeDrop;
        public AudioSource woodenLogSound;
        public Toggle playSound;

        private void Awake()
        {
            // Making sure the instance is not null
            if (Instance == null)
            {
                Instance = this;
            }
            gameAudio = GetComponent<AudioSource>();
        }

        private void Start()
        {
            inGameKnives = new List<GameObject>();
            gameState = GameState.MAIN_MENU;
        }

        // When user hits the play button
        public void PlayGame()
        {
            gameAudio.volume = gameAudio.volume / 2f;
            ResetGame();
            // Load first stage
            if(stages.Count > 0)
            {
                LoadStage(0); // Loading the 1st stage when starting the game
                score = 0;
                UpdateScore(score);
                gameState = GameState.GAME_PLAYING;
                mainMenu.SetActive(false);
                gamePlayMenu.SetActive(true);
            }
        }

        // Loads the stage
        public void LoadStage(int stageIndex)
        {
            currentStage = stages[stageIndex];
            // Whenever we load a stage we load its first level
            StartCoroutine(LoadLevel(currentStage, 0));
        }

        // Loads the level
        // Given stage and level index
        public IEnumerator LoadLevel(Stage stage, int levelIndex, float waitTime = 0f)
        {
            yield return new WaitForSeconds(waitTime);
            Debug.Log("Loading Level: " + levelIndex);
            if (levelIndex < stage.levels.Count)
            {
                ResetGame();
                var woodenLog = stage.levels[levelIndex].woodenLog;
                var knife = stage.levels[levelIndex].knife;
                currentLevel = currentStage.levels[levelIndex];
                currentLevel.SpawnKnife(knifeSpawnPoint);
                currentLevel.SpawnWoodenLog(woodenLogSpawnPoint);
                if(levelIndex < stage.levels.Count - 1)
                    levelText.text = "Level " + (levelIndex + 1).ToString();
                else if (levelIndex == stage.levels.Count - 1)
                    levelText.text = "Boss Level";
                CreateKnifeIndicators(currentLevel.numOfKnives);
            }
            else
            {
                StartCoroutine(GameWon());
            }
        }

        // Creates the knife indicators on the left of the gameplay menu
        public void CreateKnifeIndicators(int noOfKnives)
        {
            if(knifeIndicatorPrefab == null)
            {
                Debug.Log("assing knife indicator prefab in the inspector");
                return;
            }
            for (int i = 0; i < noOfKnives; i++)
            {
                var knifeindicator = Instantiate(knifeIndicatorPrefab, knifeIndicatorParent);
                knifeIndicators.Add(knifeindicator);
            }
        }

        // Updates the knife indicator when user throws an knife
        public void UpdateKnifeIndicator(int index)
        {
            knifeIndicators[index].GetComponent<Image>().sprite = knifeIndicatorBlack;
        }

        // If the user wants to restart the game
        public void RestartGame()
        {
            gameAudio.volume = gameAudio.volume * 2f;
            gameOverMenu.SetActive(false);
            gameWinningMenu.SetActive(false);
            PlayGame();
        }

        // When user hits the pause button
        public void PauseGame()
        {
            gameState = GameState.GAME_PAUSED;
        }

        // If the game is over
        public IEnumerator GameOver()
        {
            gameState = GameState.GAME_OVER;  
            yield return new WaitForSeconds(1f);
            gamePlayMenu.SetActive(false);
            gameOverMenu.SetActive(true);
            stageText.text = "Stage " + (stages.IndexOf(currentStage) + 1).ToString();
            ResetGame();
        }

        // If user won the game
        public IEnumerator GameWon()
        {
            Debug.Log("game won by the player");
            gameState = GameState.GAME_OVER;
            yield return new WaitForSeconds(0.5f);
            gamePlayMenu.SetActive(false);
            gameWinningMenu.SetActive(true);
            ResetGame();
        }

        // Resets the game
        public void ResetGame()
        {
            foreach(var knife in inGameKnives)
            {
                Destroy(knife);
            }

            foreach(var knifeIndicator in knifeIndicators)
            {
                Destroy(knifeIndicator);
            }

            Destroy(inGameWoodLog);

            inGameWoodLog = null;
            inGameKnives.Clear();
            knifeIndicators.Clear();
        }

        // Spawns the knife at the given spawn point
        public void SpawnKnife(GameObject levelKnife)
        {
            if(levelKnife == null)
            {
                Debug.Log("assign knife prefab in inspector");
                return;
            }

            var knife = Instantiate(levelKnife, knifeSpawnPoint.position, Quaternion.identity);
            inGameKnives.Add(knife);
        }

        // Spawns the wooden log at the given spawn point
        public void SpawnWoodenLog(GameObject levelWoodenLog)
        {
            if (levelWoodenLog == null)
            {
                Debug.Log("assign wooden log prefab in inspector");
                return;
            }

            levelWoodenLog = Instantiate(woodenLogPrefab, woodenLogSpawnPoint.position, Quaternion.identity);
        }

        // Adds the score
        public void UpdateScore(int val)
        {
            if(scoreText == null)
            {
                Debug.Log("assign score text in the inspector");
                return;
            }
            score = score + val;
            scoreText.text = score.ToString();
            gameOverScore.text = scoreText.text;
        }

        // Sets the current game state
        public void SetGameState(int state)
        {
            gameState = (GameState)state;
        }

        public void ToggleGameAudio()
        {
            if(!playSound.isOn)
            {
                gameAudio.mute = true;
                knifeDrop.mute = true;
                woodenLogSound.mute = true;
            }else
            {
                gameAudio.mute = false;
                knifeDrop.mute = false;
                woodenLogSound.mute = false;
            }
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
        gameState = GameState.EXIT_GAME;
#endif
        }
    }
}
