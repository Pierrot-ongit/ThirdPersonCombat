using Sirenix.OdinInspector;
using UnityEngine;

namespace ThirdPersonCombat.Combat
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Player Attack Data", order = 0)]
    public class PlayerAttackData : AttackData
    {
        
        [field: SerializeField] public int ComboStateIndex { get; private set; } = -1;
        [field: SerializeField] public float ComboAttackTime { get; private set; }
        
    }
}