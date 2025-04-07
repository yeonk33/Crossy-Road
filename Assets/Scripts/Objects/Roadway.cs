using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roadway : MonoBehaviour
{
	[SerializeField] Transform[] spawnPoint;
	[SerializeField] float minInterval = 1.0f;
	[SerializeField] float maxInterval = 4.0f;
	[SerializeField] float minSpeed = 4.5f;
	[SerializeField] float maxSpeed = 5.5f;

	private Queue<Poolable> cars;
	private int index;
	private Quaternion quaternion;
	private Vector3 dir;
	public Vector3 startPos;
	private Vector3 endPos;
		
	private float interval; // ������ �ð��� car ��߽�Ű��
	private float speed;

	private void Awake() // Roadway�� ó�� ������ ���� ����� (�� ��)
	{
		cars = new Queue<Poolable>();
		interval = Random.Range(minInterval, maxInterval);
	}

	private void OnEnable() // SetActive(true)�� ������ ����� (���� ��)
	{
		//Init();
		//StartCoroutine(StartDelay());
	}

	private void Start() // �����ǰ� ó�� SetActive(true)�� ���� ����� (�� ��)
	{

	}

	private void OnDisable() // SetActive(false)�� ������ ����� (���� ��)
	{

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
		GameObject car = Managers.Pool.Pop(Managers.Map.carPrefabs[0], (int)startPos.z).gameObject;
		car.transform.position = startPos;
		cars.Enqueue(car.GetOrAddComponent<Poolable>());
		car.GetComponent<Car>().Init(quaternion, dir, endPos, speed);
	}

	// �������� ���� ����
	public void Init()
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

	public void StartCarSpawn()
	{
		StartCoroutine(StartDelay());
	}
}
