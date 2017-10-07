using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigHolder : MonoBehaviour
{
	[SerializeField] ConfigScrolling configScrolling;
	[SerializeField] ConfigRoad configRoad;
	
	void Awake()
	{
		if (null == configScrolling)
		{
			Debug.LogError("No scrolling config assigned. Reverting to defaults");
			configScrolling = ScriptableObject.CreateInstance(typeof(ConfigScrolling)) as ConfigScrolling;
		}
		
		if (null == configRoad)
		{
			Debug.LogError("No road generator config assigned. Reverting to defaults");
			configRoad = ScriptableObject.CreateInstance(typeof(ConfigRoad)) as ConfigRoad;
		}
	}
	
	public float InitialSpeed { get { return configScrolling.speedInitial; } }
	public float TimePerSpeedInc { get { return configScrolling.timePerSpeedInc; } }
	public float SpeedIncStep { get { return configScrolling.speedIncStep; } }
	public float SpeedCap { get { return configScrolling.speedCap; } }
	public float SpeedupTweenTime { get { return configScrolling.speedupTweenTime; } }
	public float GlobalShiftThreshold { get { return configScrolling.globalShiftEvery; } }
	
	public float GeneratorsUpdateTick { get { return configRoad.generatorsUpdateTick; } }
	public float RoadSectionLen { get { return configRoad.sectionLength; } }
	public float SpawnSectionsAhead { get { return configRoad.sectionsAhead; } }
	
	public float FrontRowXPos { get { return configRoad.frontRowXPos; } }
	public float BackRowXPos { get { return configRoad.backRowXPos; } }

	public float BackRowPadding { get { return RndFromVector2(configRoad.BGPaddingRange, false); } }
	public float BackRowXShift { get { return RndFromVector2(configRoad.BGShiftRange, true); } }
	
	public float FrontRowPadding { get { return RndFromVector2(configRoad.FGPaddingRange, false); } }
	public float FrontRowXShift { get { return RndFromVector2(configRoad.FGShiftRange, true); } }
	
	float RndFromVector2(Vector2 v, bool signed)
	{
		var rnd = Random.Range(v.x, v.y);
		if (signed) rnd *= (Random.value < .5f ? -1f : 1f);
		return rnd;
	}
}
