using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float distance = 3.0f; // 한 칸 이동 거리
	[SerializeField] private float speed = 5f;
	[SerializeField] private float minDis = 50f; // 최소 스와이프 거리
	[SerializeField] private float jumpHeight = 0.5f;

	private bool isMoving = false;
	private Vector2 swipeStart;
	private PlayerInput playerInput;
	private InputAction holdAction;
	private InputAction swipeAction;

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();

		holdAction = playerInput.actions["Hold"]; // Interaction : Tap
		swipeAction = playerInput.actions["Swipe"];

		holdAction.started += HoldAction_started;
		holdAction.performed += HoldAction_performed; // 짧게 탭할 경우 performed 실행
		holdAction.canceled += HoldAction_canceled;	  // 길게 누를 경우 canceled 실행
	}

	private void OnDestroy()
	{
		holdAction.started -= HoldAction_started;
		holdAction.performed -= HoldAction_performed;
		holdAction.canceled -= HoldAction_canceled;
	}

	private void HoldAction_started(InputAction.CallbackContext context)
	{
		swipeStart = swipeAction.ReadValue<Vector2>();
	}

	private void HoldAction_performed(InputAction.CallbackContext context)
	{
		if (!isMoving) {
			StartCoroutine(Move(transform.position + Vector3.forward * distance));
		}
	}

	private void HoldAction_canceled(InputAction.CallbackContext obj)
	{
		Vector2 swipeEnd = swipeAction.ReadValue<Vector2>();
		Vector2 delta = swipeEnd - swipeStart;

		Vector3 dir = Vector3.zero;

		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
			dir = delta.x > 0 ? Vector3.right : Vector3.left;
		} else {
			dir = delta.y > 0 ? Vector3.forward : Vector3.back;
		}

		if (delta.magnitude < minDis)
			dir = Vector3.forward; // 앞으로 이동

		if (!isMoving) {
			StartCoroutine(Move(transform.position + dir * distance));
		}
	}

	private IEnumerator Move(Vector3 dest)
	{
		isMoving = true;
		Vector3 startPos = transform.position;
		float timer = 0f;
		float duration = Vector3.Distance(startPos, dest) / speed;

		while (timer < duration) {
			timer += Time.deltaTime;
			float t = timer / duration;

			Vector3 newPos = Vector3.Lerp(startPos, dest, t);
			newPos.y += Mathf.Sin(t * Mathf.PI) * jumpHeight;
			transform.position = newPos;

			yield return null;
		}

		transform.position = dest;
		isMoving = false;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Car")) {
			Debug.Log("게임오버");
			// reset game
		}
	}
}
