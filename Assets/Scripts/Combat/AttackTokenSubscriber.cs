using System;
using UnityEngine;

namespace ThirdPersonCombat.Combat
{
    public class AttackTokenSubscriber : MonoBehaviour
    {
        [field: SerializeField] public bool DontListen { get; private set; } = false; // TODO Améliorer pour un vrai system de priorité.
        public AttackTokenManager TokenManager { get; private set; }
        public int idToken { get; private set; } = -1;

        private void Start()
        {
            TokenManager = GameObject.FindWithTag("BattleManager").GetComponent<AttackTokenManager>();
        }

        public bool RequestToken()
        {
            if (DontListen) return true;
            if (this.idToken > -1) return true;
            
            int idToken = TokenManager.GetTokenAvailable();
           if (idToken < 0) return false;
           
           TokenManager.SetTokenTaken(idToken);
           this.idToken = idToken;
           return true;
        }

        public void ReleaseToken()
        {
            if (idToken < 0) return;
            
            TokenManager.ReleaseToken(idToken);
            idToken = -1;
        }

        private void OnDrawGizmos()
        {
            if (idToken > -1)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(transform.position + Vector3.up * 2, 0.2f);
            }
        }
    }
}