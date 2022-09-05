using System;
using UnityEngine;

namespace ThirdPersonCombat
{
    public class ShowHideUI : MonoBehaviour
    {
        [field:SerializeField] public InputReader InputReader { get; private set; }
        [SerializeField] GameObject uiContainer = null;
        public event Action onToggleContainer;

        // Start is called before the first frame update
        void Start()
        {
            uiContainer.SetActive(false);
            InputReader.MenuEvent += Toggle;
        }

        private void OnDisable()
        {
            InputReader.MenuEvent -= Toggle;
        }

        public void Toggle()
        {
            uiContainer.SetActive(!uiContainer.activeSelf);
            onToggleContainer?.Invoke();
        }
    }
}