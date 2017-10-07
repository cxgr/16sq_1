using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ConfigScrolling : ScriptableObject
{
	[Tooltip("min speed")]
	public float speedInitial = 3f;
	[Tooltip("max possible speed")]
	public float speedCap = 30f;
	[Tooltip("target speed increment per acceleration event")]
	public float speedIncStep = 3f;
	[Tooltip("period of acceleration event timer")]
	public float timePerSpeedInc = 10f;
	[Tooltip("time to tween actual speed to target speed")]
	public float speedupTweenTime = 1.5f;
	[Tooltip("distance travelled before everything is teleported back to point zero (float precision maintenance)")]
	public float globalShiftEvery = 1000f;
}