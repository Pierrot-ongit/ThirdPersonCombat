using System;
using Sirenix.OdinInspector;
using ThirdPersonCombat.Combat;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ThirdPersonCombat
{
    [RequireComponent(typeof(AudioSource))]
    public class SFXManager : MonoBehaviour
    {
        [MinMaxSlider(0, 1)] [BoxGroup("config")]
        public Vector2 volume = new Vector2(0.5f, 0.5f);
        [MinMaxSlider(0, 3)]
        [HorizontalGroup("config/pitch")]
        public Vector2 pitch = new Vector2(1, 1);
        [field:SerializeField] public Health Health { get; private set; }
        [field:SerializeField] public AudioClip[] Attack { get; private set; }
        [field:SerializeField] public AudioClip[] HeavyAttack { get; private set; }
        [field:SerializeField] public AudioClip[] Screaming { get; private set; }
        [field:SerializeField] public AudioClip[] Pain { get; private set; }
        [field:SerializeField] public AudioClip[] Death { get; private set; }

        private AudioSource _audioSource;
        
        public enum SoundType
        {
            Attack,
            HeavyAttack,
            Screaming,
            Pain,
            Death,
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            if (Health != null)
            {
                Health.OnTakeDamage += PlayPainSound;
                Health.OnDie += PlayDeathSound;
            }
        }

        private void OnDisable()
        {
            if (Health != null)
            {
                Health.OnTakeDamage -= PlayPainSound;
            }
        }


        public void PlaySound(SoundType soundType)
        {
            AudioClip chosenSound = null;
            switch (soundType)
            {
                case SoundType.Attack:
                    if (Attack.Length == 0) return;
                    chosenSound =  Attack[Random.Range(0, Attack.Length)];
                    break;
                
                case SoundType.HeavyAttack: 
                    if (HeavyAttack.Length == 0) return;
                    chosenSound =  HeavyAttack[Random.Range(0, HeavyAttack.Length)];
                    Debug.Log(chosenSound);
                    break;
                case SoundType.Screaming:
                    if (Screaming.Length == 0) return;
                    chosenSound =  Screaming[Random.Range(0, Screaming.Length)];
                    break; 
                case SoundType.Pain:
                    if (Pain.Length == 0) return;
                    chosenSound =  Pain[Random.Range(0, Pain.Length)];
                    break;
                case SoundType.Death:
                    if (Death.Length == 0) return;
                    chosenSound =  Death[Random.Range(0, Death.Length)];
                    break;
            }

            if (chosenSound == null)
            {
                return;
            }
            
            _audioSource.volume = Random.Range(volume.x, volume.y);
            _audioSource.pitch = Random.Range(pitch.x, pitch.y);
            _audioSource.clip = chosenSound;
            _audioSource.Play();
        }

        public void PlayPainSound()
        {
            PlaySound(SoundType.Pain);
        }
        
        public void PlayDeathSound()
        {
            PlaySound(SoundType.Death);
        }
    }
}