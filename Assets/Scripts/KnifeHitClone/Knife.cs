using UnityEngine;

namespace KinfeHitClone
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Knife : MonoBehaviour
    {
        [SerializeField] private float velocity;
        private Rigidbody2D rigibody2D;
        private CapsuleCollider2D capsuleCollider2D;
        [SerializeField] private bool canMoveInSpace; // Determines whether the object can move or not

        private GameManager gameManager;

        private void Start()
        {
            gameManager = GameManager.Instance;
            rigibody2D = GetComponent<Rigidbody2D>();
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            canMoveInSpace = true;
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButtonDown(0) && canMoveInSpace
                && gameManager.gameState == GameState.GAME_PLAYING)
            {
                // the object is going to move upwards in 10 units per second
                rigibody2D.velocity = new Vector2(0, velocity);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "WoodenLog")
            {
                gameManager.woodenLogSound.Play();
                canMoveInSpace = false;

                rigibody2D.velocity = Vector2.zero;
                transform.position = new Vector2(0f, -1f);

                transform.SetParent(collision.transform, true);

                capsuleCollider2D.isTrigger = true;
                gameManager.UpdateScore(10);

                var currentLevel = gameManager.currentLevel;

                if (currentLevel.numOfKnives == gameManager.inGameKnives.Count)
                {
                    var stage = gameManager.currentStage;
                    var level = gameManager.currentLevel;
                    StartCoroutine(gameManager.LoadLevel(stage, stage.levels.IndexOf(currentLevel) + 1, 0.1f));
                    return;
                }
                gameManager.currentLevel.SpawnKnife(gameManager.knifeSpawnPoint);

            }
            else if (collision.tag == "Knife")
            {
                if (canMoveInSpace)
                {
                    gameManager.knifeDrop.Play();
                    Debug.Log("Hit Knife");
                    rigibody2D.velocity = new Vector2(0, -velocity);
                    GetComponent<Animator>().enabled = true;
                    StartCoroutine(gameManager.GameOver());
                }
            }
        }
    }
}
