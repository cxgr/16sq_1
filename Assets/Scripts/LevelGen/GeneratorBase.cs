using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneratorBase : MonoBehaviour
{
	protected Transform trackedMovingObject;
	protected ConfigHolder config;
	
	protected abstract void TrackMovementTick();
	protected abstract void GlobalShiftHandler();
	
	protected virtual void Awake()
	{
		config = SingletonUtils<ConfigHolder>.Instance;
		
		var player = SingletonUtils<PlayerController>.Instance;
		player.GlobalShiftEvent += GlobalShiftHandler;
		trackedMovingObject = player.transform;
	}
	
	protected virtual void Start()
	{
		StartCoroutine(TrackMovementCor());
	}
	
	IEnumerator TrackMovementCor()
	{
		while (true)
		{
			TrackMovementTick();
			yield return new WaitForSeconds(config.GeneratorsUpdateTick);
		}
	}
}
