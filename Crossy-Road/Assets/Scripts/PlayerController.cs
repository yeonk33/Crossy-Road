using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float distance = 3.0f; // 한 칸 이동 거리
	[SerializeField] private float speed = 5f;
	private bool isMoving = false;
	private PlayerInput playerInput;
	private InputAction action;


	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();

		action = playerInput.actions["Forward"];
		action.performed += Action_performed;
	}

	private void Action_performed(InputAction.CallbackContext obj)
	{
		OnMove();
	}

	private void OnMove()
	{
		if (!isMoving) {
			StartCoroutine(Move(transform.position + new Vector3(0, 0, distance)));
		}
	}

	private IEnumerator Move(Vector3 dest)
	{
		isMoving = true;
		Vector3 startPos = transform.position;
		float timer = 0f;
		float duration = Vector3.Distance(startPos, dest) / speed; // 이동 시간
		float height = 0.5f; // 점프 높이

		while (timer < duration) {
			timer += Time.deltaTime;
			float t = timer / duration; // 0 -> 1 진행 비율

			// 이동
			Vector3 newPos = Vector3.MoveTowards(startPos, dest, timer / duration * Vector3.Distance(startPos, dest));
			newPos.y += Mathf.Sin(t * Mathf.PI) * height; // y축 점프

			// 최종 위치 적용
			transform.position = newPos;

			yield return null;
		}

		transform.position = dest; // 정확한 위치 보정
		isMoving = false;
	}

}
