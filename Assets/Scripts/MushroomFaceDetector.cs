using UnityEngine;

public class MushroomFaceDetector : MonoBehaviour
{
	private GameObject myMushroom;
	// Start is called before the first frame update
	void Start()
	{

	}

	private void Awake()
	{
		myMushroom = transform.parent.gameObject;
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{

		myMushroom.GetComponent<RedMushroom>().facing *= -1;


		//else
		//   myMushroom.GetComponent<GreenMushroom>().facing *= -1;
	}
}
