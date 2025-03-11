using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Roadway : MonoBehaviour
{
	[SerializeField] Transform[] spawnPoint;
	[SerializeField] float minInterval = 1.0f;
	[SerializeField] float maxInterval = 4.0f;
	[SerializeField] float minSpeed = 4.5f;
	[SerializeField] float maxSpeed = 5.5f;

	private int index;
	private Quaternion quaternion;
	private Vector3 dir;
	private Vector3 startPos;
	private Vector3 endPos;
		
	private float interval; // ������ �ð��� car ��߽�Ű��
	private float speed;

	private void Start()
	{
		Init();
		interval = Random.Range(minInterval, maxInterval);
		StartCoroutine(StartDelay());
	}

	IEnumerator StartDelay()
	{
		yield return new WaitForSeconds(interval);
		SpawnCar();
		interval = Random.Range(minInterval, maxInterval);
		StartCoroutine(StartDelay());
	}

	// pool���� car ������ spawnpoint�� ����
	private void SpawnCar()
	{
		GameObject car = Managers.Pool.Pop(Resources.Load<GameObject>("Prefabs/Car")).gameObject;
		car.transform.position = startPos;
		car.GetComponent<Car>().Init(quaternion, dir, endPos, speed);
	}

	// �������� ���� ����
	private void Init()
	{
		index = Random.Range(0, spawnPoint.Length); // 0: left, 1: right
		if (index == 0) {
			quaternion = Quaternion.Euler(0, 90, 0);
			dir = Vector3.right;
			startPos = spawnPoint[0].position;
			endPos = spawnPoint[1].position;
		} else {
			quaternion = Quaternion.Euler(0, -90, 0);
			dir = Vector3.left;
			startPos = spawnPoint[1].position;
			endPos = spawnPoint[0].position;
		}
		speed = Random.Range(minSpeed, maxSpeed);
	}
}
