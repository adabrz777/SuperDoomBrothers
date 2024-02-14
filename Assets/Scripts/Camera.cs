using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private bool isTouchingEnd;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingEnd = GameObject.Find("PlayerCenter").GetComponent<PlayerCenter>().isTouchingEnd;

        if (!isTouchingEnd)
            transform.position = new Vector3(GameObject.Find("PlayerCenter").transform.position.x, transform.position.y, transform.position.z);
    }

	

	
}
