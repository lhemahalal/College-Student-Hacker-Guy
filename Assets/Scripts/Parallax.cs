using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length; 
    private float start; 
    public GameObject cam; 
    public float parallax; 

    // Start is called before the first frame update
    void Start()
    {
        start = transform.position.x; 
        length = GetComponent<SpriteRenderer>().bounds.size.x; 
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (cam.transform.position.x * parallax);
        transform.position = new Vector3(start + distance, transform.position.y, transform.position.z);
    }
}
