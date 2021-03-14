using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Yarn.Unity.Example {
    public class PlayerCharacter : MonoBehaviour {
        public Animator animator;
        Rigidbody2D rb;

        public GameObject projectilePrefab; 

        public int enemiesToWin; 
        private int enemiesKilled = 0; 
        private SceneSwitcher sceneSwitcher; 

        public int health = 5; 
        public float timeInvincible = 2.0f; 
        private bool isInvincible; 
        private float invincibleTimer; 

        private bool isPunching;
        private float timePunch = 1.0f; 
        private float punchTimer; 


        public float minXPosition;
        public float maxXPosition;
        public float minYPosition;
        public float maxYPosition;

        private bool left = false;
        private bool prev = false;
        private Vector2 direction;

        public float moveSpeed = 1.0f;

        public float movementFromButtons {get;set;}

        public Image[] hearts; 

        public Text bugsText; 

        void Start ()
        {
            rb = GetComponent<Rigidbody2D>();
            sceneSwitcher = GetComponent<SceneSwitcher>(); 

            isInvincible = false; 
            isPunching = false; 
        }

        /// Update is called once per frame
        void Update () {
            // Detect direction
            CheckDirection();

            // Detect punch
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isInvincible || isPunching)
                {
                    return; 
                }
                GameObject projectileObject = Instantiate(projectilePrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity); 
                Projectile projectile = projectileObject.GetComponent<Projectile>(); 
                projectile.Launch(direction, 100);

                animator.SetTrigger("Punch");
                isPunching = true; 
                punchTimer = timePunch; 
            }


            // Move the player, clamping them to within the boundaries 
            // of the level.
            var xmovement = Input.GetAxis("Horizontal");
            var yMovement = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(xmovement, yMovement, 0.0f);
            rb.velocity = movement * moveSpeed;
            rb.position = new Vector3
            (
                Mathf.Clamp(rb.position.x, minXPosition, maxXPosition),
                Mathf.Clamp(rb.position.y, minYPosition, maxYPosition),
                0.0f
            );

            
        }

        void FixedUpdate()
        {
            if (isInvincible) 
            {
                invincibleTimer -= Time.deltaTime; 
                if (invincibleTimer < 0) 
                {
                    isInvincible = false; 
                }
            }

            if (isPunching)
            {
                punchTimer -= Time.deltaTime; 
                if (punchTimer < 0)
                {
                    isPunching = false; 
                }
            }
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            if(isInvincible) 
            {
                return; 
            }

            isInvincible = true; 
            invincibleTimer = timeInvincible;

            animator.SetTrigger("Damaged");
            health--; 
            if (health > 0)
            {
                hearts[health].enabled = false; 
            }
            else if (health <= 0) {
                hearts[health].enabled = false;  
                sceneSwitcher.setWinState(true); 
            }
            Debug.Log("Player health = " + health); 
        }

        void CheckDirection()
        {
            // Detect move right
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                left = false;
                direction = Vector2.right; 
            }
            // Detect move left
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                left = true;
                direction = Vector2.left; 
            }
            if (prev != left)
            {
                prev = left;
                animator.transform.Rotate(0, 180, 0);
            }
        }

        public void KillEnemy()
        {
            enemiesKilled++; 
            bugsText.text = (enemiesKilled + " / " + enemiesToWin + " Bugs");
            if(enemiesToWin == enemiesKilled)
            {
                sceneSwitcher.setWinState(true); 
            }
            Debug.Log("Killed enemy");
        }

    }
}
