using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCenter : MonoBehaviour
{
    public bool isTouchingEnd = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("EndOfLevel"))
            isTouchingEnd = true;



    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("EndOfLevel"))
            isTouchingEnd = false;
    }
}
