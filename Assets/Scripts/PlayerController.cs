using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float distance = 3.0f; // 한 칸 이동 거리
	[SerializeField] private float speed = 20.0f;
	[SerializeField] private float minDis = 50f; // 최소 스와이프 거리
	[SerializeField] private float jumpHeight = 0.5f;

	private bool isMoving = false;
	private PlayerInput playerInput;
	private InputAction moveAction;
	private bool isHolding = false;
	private Vector2 mouseDownPos;
	private Vector2 mouseUpPos;
	private Vector3 dir;
	private float preZ;

	private CameraController camera;

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
		moveAction = playerInput.actions["Move"];
		moveAction.started += MoveAction_started; // mouse down시 실행
		moveAction.performed += MoveAction_performed; // 누르는 중일 시 실행
		moveAction.canceled += MoveAction_canceled; // mouse up시 실행

		preZ = transform.position.z;

		camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
	}

	private void MoveAction_started(InputAction.CallbackContext obj)
	{
		if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) {
			return; // UI(광고버튼)클릭이면 게임 시작 막기
		}

		mouseDownPos = Mouse.current.position.ReadValue(); // 클릭 시작 위치 저장

		if (!Managers.Game.gameStart) {
			Managers.Game.GameStart();
		}
	}

	private void MoveAction_performed(InputAction.CallbackContext obj)
	{
		if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) {
			return; // UI(광고버튼)클릭이면 게임 시작 막기
		}

		isHolding = true; // 긴 터치(스와이프)를 구분하기 위한 변수
	}

	private void MoveAction_canceled(InputAction.CallbackContext obj)
	{
		if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) {
			return; // UI(광고버튼)클릭이면 게임 시작 막기
		}

		if (!isHolding) { // 짧은 터치면 앞으로
			dir = Vector3.forward;
		} else {
			mouseUpPos = Mouse.current.position.ReadValue();
			dir = CheckDirection(mouseDownPos, mouseUpPos); // 스와이프 방향 체크
			isHolding = false;
		}

		if (!isMoving) {
			StartCoroutine(Move(transform.position + dir * distance));
		}
	}

	private Vector3 CheckDirection(Vector2 downPos, Vector2 upPos)
	{
		if (Vector3.Distance(downPos, upPos) < 10f) { // 스와이프 너무 조금하면 앞으로 이동
			return Vector3.forward;

		} else if (Math.Abs(downPos.x - upPos.x) > Math.Abs(downPos.y - upPos.y)) {
			if (downPos.x < upPos.x) { // 오른쪽 이동
				return Vector3.right;
			} else { // 왼쪽 이동
				return Vector3.left;
			}

		} else {
			if (downPos.y < upPos.y) { // 앞으로 이동
				return Vector3.forward;
			} else { // 뒤로 이동
				return Vector3.back;
			}
		}
	}

	private void Update()
	{
		Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

		//if (viewPos.z < 0 || viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1) {
		//	Debug.Log("카메라 벗어남");
		//	Managers.Game.GameOver();
		//}
		
	}
	private void OnDestroy()
	{

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
			Debug.Log("Move Camera called");
			camera.MoveCamera();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Car")) {
			Debug.Log("게임오버");
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
