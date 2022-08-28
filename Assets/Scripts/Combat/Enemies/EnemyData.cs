using UnityEngine;
using Sirenix.OdinInspector;

namespace ThirdPersonCombat.Combat.Enemy
{

    [CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy Data")]
    [InlineEditor]
    public class EnemyData : ScriptableObject
    {
        [BoxGroup("Basic Info")]
        [LabelWidth(100)]
        public string enemyName;
        [BoxGroup("Basic Info")]
        [LabelWidth(100)]
        [TextArea]
        public string description;

        [HorizontalGroup("Game Data", 75)]
        [PreviewField(75)]
        [HideLabel]
        public GameObject enemyModel;

        [VerticalGroup("Game Data/Stats")]
        [LabelWidth(100)]
        [Range(20,100)]
        [GUIColor(0.5f,1f,0.5f)]
        public int health = 20;
        [VerticalGroup("Game Data/Stats")]
        [LabelWidth(100)]
        [Range(0.5f,5f)]
        [GUIColor(0.3f,0.5f,1f)]
        public float speed = 2f;
        [VerticalGroup("Game Data/Stats")]
        [LabelWidth(100)]
        [Range(5,30)]
        [GUIColor(1f,1f,0f)]
        public float detectRange = 10f;
        [VerticalGroup("Game Data/Stats")]
        [LabelWidth(100)]
        [Range(1,20)]
        [GUIColor(0.8f,0.4f,0.4f)]
        public int damage = 1;
    }
}