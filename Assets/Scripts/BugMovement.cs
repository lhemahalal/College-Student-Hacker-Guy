using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.Example; 

public class BugMovement : MonoBehaviour    
{

    public int health = 3; 
    public float speed;
    private float waitTime;
    public float startWaitTime;


    public Transform[] moveSpots;
    private int randomSpot;

    public GameObject player; 

    void Start()
    {
        randomSpot = Random.Range(0, moveSpots.Length);
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);

        if(Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
            } else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    public void GetPunched() 
    {
        health--; 
        Debug.Log("Enemy health = " + health);
        
        if (health <= 0)
        {
            player = GameObject.Find("Player");
            player.GetComponent<PlayerCharacter>().KillEnemy(); 
            Destroy(gameObject);
        }
    }
}
