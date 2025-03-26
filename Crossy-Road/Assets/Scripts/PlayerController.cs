using System.Collections;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float distance = 3.0f; // 한 칸 이동 거리
	[SerializeField] private float speed = 5f;
	[SerializeField] private bool isMoving = false;
	private PlayerInput playerInput;
	private InputAction forwardAction;
	private InputAction swipeAction;
	private InputAction clickAction;
	private Vector2 swipeStart;
	private Vector2 swipeEnd;
	[SerializeField] private bool isDragging;
	[SerializeField] private float minDis = 50f;


	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();

		// 클릭 시 전진
		forwardAction = playerInput.actions["Forward"];
		forwardAction.performed += ForwardAction_performed;

		// 클릭한채로 스와이프 시 좌/우/뒤로 이동
		swipeAction = playerInput.actions["Swipe"];
		swipeAction.performed += SwipeAction_performed;
		swipeAction.canceled += SwipeAction_canceled;

		clickAction = playerInput.actions["Click"];
		clickAction.performed += ClickAction_performed;
		clickAction.canceled += ClickAction_canceled;
	}

	private void ClickAction_canceled(InputAction.CallbackContext obj)
	{
		if (!isDragging) return;

		isDragging = false;
		swipeEnd = swipeAction.ReadValue<Vector2>();
		Vector2 delta = swipeEnd - swipeStart;
		Debug.Log($"{swipeEnd} - {swipeStart} = {delta}");
		if (delta.magnitude < minDis) return;   // 너무 조금 스와이프한건 무시

		Vector3 dir = Vector3.zero;
		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {  // 좌우로 더 많이 움직임
			dir = delta.x > 0 ? Vector3.right : Vector3.left;
		} else {
			dir = delta.y > 0 ? Vector3.back : Vector3.forward;
		}

		if (!isMoving) StartCoroutine(Move(transform.position + dir * distance));
	}

	private void ClickAction_performed(InputAction.CallbackContext obj)
	{
		swipeStart = swipeAction.ReadValue<Vector2>();
		isDragging = true;
	}

	private void SwipeAction_canceled(InputAction.CallbackContext obj)
	{
		//swipeEnd = Mouse.current.position.ReadValue();

	}

	private void SwipeAction_performed(InputAction.CallbackContext obj)
	{
		//swipeStart = Mouse.current.position.ReadValue();
	}

	private void ForwardAction_performed(InputAction.CallbackContext obj)
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
			transform.position = newPos;

			yield return null;
		}

		transform.position = dest; // 정확한 위치 보정
		isMoving = false;
	}

}
