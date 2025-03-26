using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
	private void Awake()
	{
		Managers.Game.GameStart();
		Managers.Map.Init();
	}
}
