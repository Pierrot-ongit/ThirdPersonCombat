using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonCombat.Combat
{
    public class AttackTokenManager : MonoBehaviour
    {
        // Token pool
        // AI request
         // If token not available, do something else
         // if token available, acquire it
         // Use the token, then release it
         // Cooldown on the token itself before it can be reacquired.
         // Different tokens for differents attacks types (light / heavy)
         // Enemy close to the player can steal the token.
         public Dictionary<int, Token> TokenLightAttack { get; private set; }
         [field:SerializeField] public int TokensNumber { get; private set; }
         [field:SerializeField] public float tokenCooldown { get; private set; }
         [field:SerializeField] public float managerCooldown { get; private set; }

         private float managerCooldownRemainingTime;
         
         public class Token
         {
             public float cooldown;
             public bool taken;
         }
         private void Start()
         {
             TokenLightAttack = new Dictionary<int, Token>();
             for (int i = 0; i < TokensNumber + 1; i++)
             {
                 Token token = new Token();
                 token.cooldown = 0;
                 token.taken = false;
                 TokenLightAttack[i] = token;
             }
         }

         private void Update()
         {
             for (int i = 0; i < TokensNumber + 1; i++)
             {
                 if (TokenLightAttack[i].cooldown > 0)
                 {
                     TokenLightAttack[i].cooldown = Mathf.Max(TokenLightAttack[i].cooldown - Time.deltaTime, 0);
                 }
             }

             if (managerCooldownRemainingTime > 0)
             {
                 managerCooldownRemainingTime = Mathf.Max(managerCooldownRemainingTime - Time.deltaTime, 0);
             }
         }


         public int GetTokenAvailable()
         {
             for (int i = 0; i < TokensNumber + 1; i++)
             {
                 if (TokenLightAttack[i].taken == false && TokenLightAttack[i].cooldown <= 0)
                 {
                     return i;
                 }
             }

             return -1;
         }

         public void SetTokenTaken(int i)
         {
             TokenLightAttack[i].taken = true;
             managerCooldownRemainingTime = managerCooldown;
         }

         public void ReleaseToken(int i)
         {
             TokenLightAttack[i].taken = false;
             TokenLightAttack[i].cooldown = tokenCooldown;
         }
    }
}