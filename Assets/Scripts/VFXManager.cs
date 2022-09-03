using System;
using Sirenix.OdinInspector;
using ThirdPersonCombat.Combat;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ThirdPersonCombat
{
    public class VFXManager : MonoBehaviour
    {
        [field:SerializeField] public ParticleSystem HeavyAttack { get; private set; }
        
        public enum VFXType
        {
            Attack,
            HeavyAttack,
            Screaming,
            Pain,
            Death,
        }

        private void Start()
        {

        }


        void PlayParticleSystem(VFXType type)
        {
            switch (type)
            {
                case VFXType.HeavyAttack: 
                    HeavyAttack.Play();
                    break;
            }
        }

        public void PlayHeavyAttackVFX()
        {
            PlayParticleSystem(VFXType.HeavyAttack);
        }

    }
}