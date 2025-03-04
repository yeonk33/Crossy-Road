using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] GameObject _sidewalk;
	[SerializeField] GameObject _roadway;

	public bool gameStart = false;

	private void Awake()
	{
		// pool����
		Managers.Pool.CreatePool(_sidewalk, 10);
		Managers.Pool.CreatePool(_roadway, 10);

		// �� ���� @@@@ ���߿� �������� �ٲٱ�
		//Managers.Pool.Pop(_sidewalk);
		//Managers.Pool.Pop(_sidewalk, 3);
		//Managers.Pool.Pop(_roadway, 6);
		//Managers.Pool.Pop(_roadway, 9);
		//Managers.Pool.Pop(_sidewalk, 12);

	}
}
