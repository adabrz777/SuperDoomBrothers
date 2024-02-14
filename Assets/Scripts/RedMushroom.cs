using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMushroom : MonoBehaviour
{
    private BoxCollider2D myCollider;
    private Rigidbody2D myRigidbody;

    private int canGo = 2;

    public int facing = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void Awake()
	{
        myCollider = GetComponent<BoxCollider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();

    }



	// Update is called once per frame
	void Update()
    {
        if (canGo != 0)
            GoUp();
        else
            GoMyWay();
    }

    private void GoUp()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * 1.5f , transform.position.z);

    }

    private void GoMyWay()
	{
        myRigidbody.velocity = new Vector2(facing * 2.5f, myRigidbody.velocity.y);
	}

    public void changeFacing()
	{
        facing *= -1;
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("QuestionMarkBlock"))
		{
            canGo -= 1;

            if(canGo == 0)
			{
                myCollider.isTrigger = false;
                myRigidbody.bodyType = RigidbodyType2D.Dynamic;
                myRigidbody.mass = 2f;
            }

        }

    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<MarioSuperpowers>().grow();
            Destroy(this.gameObject);
        }
    }

}
