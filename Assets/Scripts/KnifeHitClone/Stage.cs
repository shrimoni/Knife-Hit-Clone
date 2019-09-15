using System.Collections.Generic;
using UnityEngine;

namespace KinfeHitClone
{
    [CreateAssetMenu(fileName = "New Stage", menuName = "Stage")]
    public class Stage : ScriptableObject
    {
        public int stageNo;
        public List<Level> levels = new List<Level>();
    }
}
