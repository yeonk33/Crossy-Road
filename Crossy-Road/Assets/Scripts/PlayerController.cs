using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float jumpForce = 2f;
	[SerializeField] private float moveForce = 2.55f;
	public float JumpForce => jumpForce;
	public float MoveForce => moveForce;

	private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction swipeAction;
	private Vector2 touchStart;
	private Vector2 touchEnd;

	private Rigidbody rigid;
	private bool onGround;



	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
		moveAction = playerInput.actions["Touch"];
		moveAction.performed += OnMoveForward;    // 이동 입력 감지
		swipeAction = playerInput.actions["Swipe"];
		swipeAction.performed += SwipeAction_performed;
		swipeAction.canceled += SwipeAction_canceled;

		rigid = GetComponent<Rigidbody>();
		onGround = false;
	}

	private void SwipeAction_canceled(InputAction.CallbackContext obj)
	{
		Vector2 swipeVector = -(obj.ReadValue<Vector2>() - touchStart);
		Vector3 dir;

		if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y)) {  // 좌우 이동
			dir = swipeVector.x >= 0 ? Vector3.right : Vector3.left;
		} else if (swipeVector.y < 0) {
			dir = Vector3.back;
		} else {
			dir = Vector3.forward;
		}

		if (onGround) Move(dir);

		Debug.Log($"swipeVector {swipeVector}\ndir {dir}");
	}

	private void SwipeAction_performed(InputAction.CallbackContext obj)
	{
		touchStart = obj.ReadValue<Vector2>();
		//Debug.Log("SwipeAction_performed");
	}

	private void OnMoveForward(InputAction.CallbackContext context)
	{
		if (onGround) Move(Vector3.forward);
	}

	private void Move(Vector3 direction)
	{
		onGround = false;
		Vector3 jumpDirection = direction * moveForce + Vector3.up * jumpForce;
		rigid.AddForce(jumpDirection, ForceMode.Impulse);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("Ground")) {
			onGround = true;
		}
	}
}
