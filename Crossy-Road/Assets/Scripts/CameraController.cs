using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] Vector3 rotateDelta = new Vector3(70, -15, 0);
	[SerializeField] Vector3 positionDelta = new Vector3(3, 15, 0);
	[SerializeField] float speed = 3.0f;

	private void Start()
	{
		transform.Rotate(rotateDelta);
		transform.position = positionDelta;
	}
	private void LateUpdate()
	{
		//if (Managers.Game.gameStart) {
			transform.position += new Vector3(0, 0, 1) * Time.deltaTime * speed;
		//}
	}
}
