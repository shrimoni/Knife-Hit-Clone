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

        private void Start()
        {
            rigibody2D = GetComponent<Rigidbody2D>();
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            canMoveInSpace = true;
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButtonDown(0) && canMoveInSpace)
            {
                // the object is going to move upwards in 10 units per second
                rigibody2D.velocity = new Vector2(0, velocity);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "WoodenLog")
            {
                canMoveInSpace = false;
                Debug.Log("Hit Wooden Log");

                rigibody2D.velocity = Vector2.zero;
                transform.position = Vector2.zero;

                transform.SetParent(collision.transform, true);
                GameManager.Instance.SpawnKnife();

                capsuleCollider2D.isTrigger = true;
                GameManager.Instance.AddScore(10);
            }
            else if (collision.tag == "Knife")
            {
                if (canMoveInSpace)
                {
                    Debug.Log("Hit Knife");
                    rigibody2D.velocity = new Vector2(0, -velocity);
                    GetComponent<Animator>().enabled = true;
                }
            }
        }
    }
}
