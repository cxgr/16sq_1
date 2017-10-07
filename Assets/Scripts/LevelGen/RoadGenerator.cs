using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : GeneratorBase
{
	[SerializeField] GameObjectPool roadPool;
	
	Queue<GameObject> liveRoadPieces = new Queue<GameObject>();
	
	float nextRoadCycleZ;
	
	protected override void Start()
	{
		for (int i = 0; i < config.SpawnSectionsAhead + 1; ++i)
			SpawnRoadPiece(i * config.RoadSectionLen);
		
		nextRoadCycleZ = config.RoadSectionLen;
		
		base.Start();
	}
	
	protected override void TrackMovementTick()
	{
		while (trackedMovingObject.position.z >= nextRoadCycleZ)
		{
			roadPool.Adopt(liveRoadPieces.Dequeue());
			var nextPieceZ = nextRoadCycleZ + config.SpawnSectionsAhead * config.RoadSectionLen;
			SpawnRoadPiece(nextPieceZ);
		}
	}
	
	void SpawnRoadPiece(float z)
	{
		var roadPieceGO = roadPool.Instantiate();
		roadPieceGO.transform.position = Vector3.forward * z;
		liveRoadPieces.Enqueue(roadPieceGO);
		
		nextRoadCycleZ += config.RoadSectionLen;
	}
	
	protected override void GlobalShiftHandler()
	{
		foreach (var r in liveRoadPieces)
			r.transform.Translate(Vector3.back * config.GlobalShiftThreshold);
		nextRoadCycleZ -= config.GlobalShiftThreshold;
	}
	
#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (!Globals.drawGizmos) return;
		
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(Vector3.forward * nextRoadCycleZ, 1f);
	}
#endif
}
