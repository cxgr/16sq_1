using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DecoGenerator : GeneratorBase
{
	class DecoRow
	{
		public enum EPlacementType { Background, Foreground, };
		
		public DecoRow(EPlacementType p, float x)
		{
			placementType = p;
			FixedRowX = x;
		}
		
		public EPlacementType placementType;
		public LinkedList<DecoObject> liveDecoPieces = new LinkedList<DecoObject>();
		public float FixedRowX { get; private set; }
		public float EndsAtZ { get; set; } = 0f;
		public void RecalculateEndZ()
		{
			if (liveDecoPieces.Count > 0)
			{
				var obj = liveDecoPieces.Last.Value;
				EndsAtZ = obj.transform.position.z + obj.boundingBox.bounds.extents.z;
			}
			else
				EndsAtZ = 0f;
		}
	}
	
	[SerializeField] GameObjectPool[] decoPools;
	
	List<DecoRow> decoRows = new List<DecoRow>();
	
	Dictionary<DecoObject.EDecoObjectType, GameObjectPool> poolsByType = 
		new Dictionary<DecoObject.EDecoObjectType, GameObjectPool>();
	
	DecoObject.EDecoObjectType[] fgRandomSelection =
	{
		DecoObject.EDecoObjectType.FGTree,
		DecoObject.EDecoObjectType.FGSmall1,
		DecoObject.EDecoObjectType.FGSmall2,
	};
	
	protected override void Awake()
	{
		base.Awake();
		
		decoRows.Add(new DecoRow(DecoRow.EPlacementType.Background, -config.BackRowXPos));
		decoRows.Add(new DecoRow(DecoRow.EPlacementType.Background, config.BackRowXPos));
		decoRows.Add(new DecoRow(DecoRow.EPlacementType.Foreground, -config.FrontRowXPos));
		decoRows.Add(new DecoRow(DecoRow.EPlacementType.Foreground, config.FrontRowXPos));
		
		foreach (var p in decoPools)
		{
			var pType = p.prefab.GetComponent<DecoObject>().decoType;
			if (!poolsByType.ContainsKey(pType))
				poolsByType.Add(pType, p);
			else
				Debug.LogError("duplicate type in deco prefab " + p.prefab.name);
		}
	}
	
	protected override void Start()
	{
		var targetZ = config.SpawnSectionsAhead * config.RoadSectionLen;
		foreach (var r in decoRows)
			while (r.EndsAtZ < targetZ)
				SpawnInRow(r);
		
		base.Start();
	}
	
	protected override void TrackMovementTick()
	{
		foreach (var r in decoRows)
		{
			//accounting for long tree shadows
			var lag = config.RoadSectionLen / 2f;
			while (r.liveDecoPieces.Count > 0 &&
				r.liveDecoPieces.First.Value.transform.position.z < trackedMovingObject.position.z - lag)
			{
				var tmp = r.liveDecoPieces.First.Value;
				r.liveDecoPieces.RemoveFirst();
				poolsByType[tmp.decoType].Adopt(tmp.gameObject);
			}
			
			while (r.EndsAtZ < trackedMovingObject.position.z + config.SpawnSectionsAhead * config.RoadSectionLen)
				SpawnInRow(r);
		}
	}
	
	DecoObject.EDecoObjectType GetRandomFGDecoType()
	{
		return fgRandomSelection[Random.Range(0, fgRandomSelection.Length)];
	}
	
	void SpawnInRow(DecoRow row)
	{
		var typeToSpawn = (row.placementType == DecoRow.EPlacementType.Background) ? 
			DecoObject.EDecoObjectType.BGTree : GetRandomFGDecoType();
		
		var newObj = poolsByType[typeToSpawn].Instantiate();
		var decoComp = newObj.GetComponent<DecoObject>();
		decoComp.Randomize();
		
		var offsetFromLast = decoComp.boundingBox.bounds.extents.z;
		var finalPos = new Vector3(row.FixedRowX, 0f, row.EndsAtZ + offsetFromLast);
		
		var padding = Vector3.forward;
		var shift = Vector3.right;
		if (row.placementType == DecoRow.EPlacementType.Background)
		{
			padding *= config.BackRowPadding;
			shift *= config.BackRowXShift;
		}
		else
		{
			padding *= config.FrontRowPadding;
			shift *= config.FrontRowXShift;
		}
		finalPos += padding;
		finalPos += shift;
		
		newObj.transform.position = finalPos;
		row.liveDecoPieces.AddLast(decoComp);
		
		row.EndsAtZ = newObj.transform.position.z + decoComp.boundingBox.bounds.extents.z;
	}
	
	protected override void GlobalShiftHandler()
	{
		foreach (var r in decoRows)
		{
			foreach (var d in r.liveDecoPieces)
				d.transform.Translate(Vector3.back * config.GlobalShiftThreshold, Space.World);
			r.RecalculateEndZ();
		}
	}
}
