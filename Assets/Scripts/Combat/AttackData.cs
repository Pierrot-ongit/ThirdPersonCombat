using Sirenix.OdinInspector;
using UnityEngine;

namespace ThirdPersonCombat.Combat
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Attack Data", order = 0)]
    public class AttackData : ScriptableObject
    {
        [BoxGroup("Animation")]
        [LabelWidth(100)]
        public string AnimationName = "Attack";

        [BoxGroup("Animation")] public float TransistionDuration = 0.1f;
        
        
        [VerticalGroup("Stats")]
        [LabelWidth(100)]
        [Range(1,50)]
        [GUIColor(0.8f,0.4f,0.4f)]
        public int Damage = 1;
        
        [VerticalGroup("Stats")]
        [LabelWidth(100)]
        [Range(0,60)]
        [GUIColor(1f,1f,0f)]
        public int Cooldown = 0;
        
        [VerticalGroup("Stats")]
        [LabelWidth(100)]
        [Range(1,60)]
        [GUIColor(1f,1f,0f)]
        public float Range = 1.5f;
        
        [VerticalGroup("Stats")]
        [LabelWidth(100)]
        [Range(0.1f,10)]
        [GUIColor(0.5f,1f,0.5f)]
        public float ForceTime = 0.35f;
        
        [VerticalGroup("Stats")]
        [LabelWidth(100)]
        [Range(1,50)]
        [GUIColor(0.5f,1f,0.5f)]
        public int Force = 1;

        [VerticalGroup("Stats")]
        [LabelWidth(100)]
        [Range(1,50)]
        [GUIColor(1f,1f,0f)]
        public float Knockback = 1;
        
        public Projectile ProjectilePrefab;
        
    }
}