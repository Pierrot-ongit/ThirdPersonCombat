using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThirdPersonCombat
{
    public class PauseMenuUI : MonoBehaviour
    {
        private CharacterController playerController;
        private void Awake()
        {
            playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            if (playerController == null) return;
            Time.timeScale = 0;
            playerController.enabled = false;
        }

        private void OnDisable()
        {
            if (playerController == null) return;
            Time.timeScale = 1;
            playerController.enabled = true;
        }

        public void Settings()
        {
            
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}