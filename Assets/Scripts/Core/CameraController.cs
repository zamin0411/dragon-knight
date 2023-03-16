using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	//Room camera
	[SerializeField] private float speed;
	private float currentPosX;
	private Vector3 velocity = Vector3.zero;

	//Follow player
	[SerializeField] private Transform player;
	[SerializeField] private float aheadDistance;
	[SerializeField] private float cameraSpeed;
	private float lookAhead;

	// Update is called once per frame
	void Update()
	{
		//Change the position of the room camera
		//transform.position = Vector3.SmoothDamp
		//    (
		//        transform.position,
		//        new Vector3
		//            (
		//                currentPosX,
		//                transform.position.y,
		//                transform.position.z
		//            ),
		//        ref velocity,
		//        speed
		//    );

		//Follow player
		transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
		lookAhead = Mathf.Lerp(lookAhead, aheadDistance * transform.localScale.x, cameraSpeed * Time.deltaTime);

	}

	public void MoveToNewRoom(Transform _newRoom)
	{
		currentPosX = _newRoom.position.x;
	}
}
