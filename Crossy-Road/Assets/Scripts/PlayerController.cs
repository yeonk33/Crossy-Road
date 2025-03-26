using System.Collections;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float distance = 3.0f; // �� ĭ �̵� �Ÿ�
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

		// Ŭ�� �� ����
		forwardAction = playerInput.actions["Forward"];
		forwardAction.performed += ForwardAction_performed;

		// Ŭ����ä�� �������� �� ��/��/�ڷ� �̵�
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
		if (delta.magnitude < minDis) return;   // �ʹ� ���� ���������Ѱ� ����

		Vector3 dir = Vector3.zero;
		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {  // �¿�� �� ���� ������
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
		float duration = Vector3.Distance(startPos, dest) / speed; // �̵� �ð�
		float height = 0.5f; // ���� ����

		while (timer < duration) {
			timer += Time.deltaTime;
			float t = timer / duration; // 0 -> 1 ���� ����

			// �̵�
			Vector3 newPos = Vector3.MoveTowards(startPos, dest, timer / duration * Vector3.Distance(startPos, dest));
			newPos.y += Mathf.Sin(t * Mathf.PI) * height; // y�� ����
			transform.position = newPos;

			yield return null;
		}

		transform.position = dest; // ��Ȯ�� ��ġ ����
		isMoving = false;
	}

}
