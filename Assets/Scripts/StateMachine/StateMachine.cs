using System;
using UnityEngine;

namespace ThirdPersonCombat.StateMachine
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected State currentState;

        private void Update()
        {
            currentState?.Tick(Time.deltaTime);
        }

        public void SwitchState(State newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }
    }
}