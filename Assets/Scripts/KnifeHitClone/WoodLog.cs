using UnityEngine;

namespace KinfeHitClone
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class WoodLog : MonoBehaviour
    {
        public Sprite woodLogImage;
        public Animation woodLogAnimation;
    }
}
