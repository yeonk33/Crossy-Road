using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Vector3 positionDelta = new Vector3(5.6f, 17f, 3f);
	[SerializeField] Vector3 rotateDelta = new Vector3(65, -22, 0);
	[SerializeField] float speed = 2.0f;

	private void Start()
	{
		transform.rotation = Quaternion.Euler(rotateDelta);
		transform.position = positionDelta;
	}
	private void LateUpdate()
	{
		if (Managers.Game.gameStart) {
			transform.position += new Vector3(0, 0, 1) * Time.deltaTime * speed;
		}
	}
}
