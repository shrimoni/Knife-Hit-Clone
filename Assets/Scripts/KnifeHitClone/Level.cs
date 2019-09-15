using UnityEngine;
using System.Collections.Generic;

namespace KinfeHitClone
{
    public enum LevelType
    {
        NORMAL = 0,
        BOSS = 1
    }

    [CreateAssetMenu(fileName = "New Level", menuName = "Level")]
    public class Level : ScriptableObject
    {
        public LevelType levelType;
        public int numOfKnives;
        public GameObject woodenLog;
        public GameObject knife;

        public Sprite woodLogTexture;
        public RuntimeAnimatorController woodLogAnimator;

        // Spawns the knife at the given spawn point
        public void SpawnKnife(Transform spawnPoint)
        {
            if (knife == null)
            {
                Debug.Log("assign knife prefab in inspector");
                return;
            }

            var kni = Instantiate(knife, spawnPoint.position, Quaternion.identity);
            if(GameManager.Instance.inGameKnives.Count >= 1)
                GameManager.Instance.UpdateKnifeIndicator(GameManager.Instance.inGameKnives.Count - 1);
            GameManager.Instance.inGameKnives.Add(kni);
        }

        // Spawns the wooden log at the given spawn point
        public void SpawnWoodenLog(Transform spawnPoint)
        {
            if (woodenLog == null)
            {
                Debug.Log("assign wooden log prefab in inspector");
                return;
            }

            var log = Instantiate(woodenLog, spawnPoint.position, Quaternion.identity);
            log.GetComponent<SpriteRenderer>().sprite = woodLogTexture;
            log.GetComponent<Animator>().runtimeAnimatorController = woodLogAnimator;
            GameManager.Instance.inGameWoodLog = log;
        }
    }
}
