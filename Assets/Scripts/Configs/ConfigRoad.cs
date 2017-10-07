using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ConfigRoad : ScriptableObject
{
	[Header("density settings")]
	[Tooltip("core X position of foreground rows, relative offset from the road")]
	public float frontRowXPos = 3f;
	[Tooltip("core X position of background rows, relative offset from the road")]
	public float backRowXPos = 6f;
	
	[Tooltip("min-max range for distance between consecutive objects in BG rows")]
	public Vector2 BGPaddingRange;
	[Tooltip("min-max range for added random X shift of objects in BG rows, relative to the core X")]
	public Vector2 BGShiftRange;
	
	[Tooltip("min-max range for distance between consecutive objects in FG rows")]
	public Vector2 FGPaddingRange;
	[Tooltip("min-max range for added random X shift of objects in FG rows, relative to the core X")]
	public Vector2 FGShiftRange;
	
	[Header("internal")]
	
	[Tooltip("generators refresh rate")]
	[Range(0, 1)]
	public float generatorsUpdateTick = .1f;
	[Tooltip("base generated section length")]
	public float sectionLength = 10f;
	[Tooltip("this many sections are generated ahead of camera")]
	public int sectionsAhead = 5;
}
