using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float distance = 3.0f; // �� ĭ �̵� �Ÿ�
	[SerializeField] private float speed = 5f;
	[SerializeField] private float minDis = 50f; // �ּ� �������� �Ÿ�
	[SerializeField] private float jumpHeight = 0.5f;

	private bool isMoving = false;
	private Vector2 swipeStart;
	private PlayerInput playerInput;
	private InputAction holdAction;
	private InputAction swipeAction;

	private float preZ;

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();

		holdAction = playerInput.actions["Hold"]; // Interaction : Tap
		swipeAction = playerInput.actions["Swipe"];

		holdAction.started += HoldAction_started;
		holdAction.performed += HoldAction_performed; // ª�� ���� ��� performed ����
		holdAction.canceled += HoldAction_canceled;   // ��� ���� ��� canceled ����

		preZ = transform.position.z;
	}

	private void Update()
	{
		Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

		if (viewPos.z < 0 || viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1) {
			Debug.Log("ī�޶� ���");
			Managers.Game.GameOver();
		}
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
		if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) {
			return; // UI(�����ư)Ŭ���̸� ���� ���� ����
		}

		if (!Managers.Game.gameStart) {
			Managers.Game.GameStart();
		}

		if (!isMoving) {
			StartCoroutine(Move(transform.position + Vector3.forward * distance));
		}
	}

	private void HoldAction_canceled(InputAction.CallbackContext obj)
	{
		if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) {
			return; // UI(�����ư)Ŭ���̸� ���� ���� ����
		}

		if (!Managers.Game.gameStart) {
			Managers.Game.GameStart();
		}

		Vector2 swipeEnd = swipeAction.ReadValue<Vector2>();
		Vector2 delta = swipeEnd - swipeStart;

		Vector3 dir = Vector3.zero;

		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
			dir = delta.x > 0 ? Vector3.right : Vector3.left;
		} else {
			dir = delta.y > 0 ? Vector3.forward : Vector3.back;
		}

		if (delta.magnitude < minDis)
			dir = Vector3.forward; // ������ �̵�

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

		if (preZ < transform.position.z) {
			Managers.Game.MoveForward();
			Managers.Map.RepositionMap();
			preZ = transform.position.z;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Car")) {
			Debug.Log("���ӿ���");
			Managers.Game.GameOver();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Coin")) {
			Managers.Game.GetCoin();
			Managers.Pool.Push(other.gameObject.GetComponent<Poolable>());
		}
	}

	public void PlayerReset()
	{
		preZ = 0;
	}
}
