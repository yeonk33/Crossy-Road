using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Vector3 positionDelta = new Vector3(5.6f, 17f, 3f);
	[SerializeField] Vector3 rotateDelta = new Vector3(65, -22, 0);
	[SerializeField] float speed = 2.0f;
	[SerializeField] float duration = 1.0f;

	private void Start()
	{
		transform.rotation = Quaternion.Euler(rotateDelta);
		transform.position = positionDelta;
	}
	private void LateUpdate()
	{
		if (Managers.Game.gameStart) {
			//transform.position += Vector3.forward * Time.deltaTime * speed;
		}
	}

	public void MoveCamera()
	{
		StartCoroutine(MoveCoroutine());
	}

	private IEnumerator MoveCoroutine()
	{
		Vector3 start = transform.position;
		Vector3 dest = transform.position + Vector3.forward * 3;
		float elapsed = 0f;
		
		while (elapsed < duration) {
			float t = elapsed / duration;
			t = Mathf.Sin(t * Mathf.PI * 0.5f);

			transform.position = Vector3.Lerp(start, dest, t);
			elapsed += Time.deltaTime;

			yield return null; // 다음 프레임까지 기다리기
		}

	}
}
