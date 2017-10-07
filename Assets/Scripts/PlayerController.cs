using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] HUD hud;
	[SerializeField] ConfigHolder config;
	[SerializeField] UnityEngine.PostProcessing.PostProcessingProfile FX;
	
	public System.Action GlobalShiftEvent;
	
	float speed;
	float totalTime;
	float totalDistance;
	
	Transform cTr;
	
	float speedupTimer;
	Coroutine activeSpeedupTween;
	
	void Awake()
	{
		cTr = transform;
		
		if (null == hud)
			hud = SingletonUtils<HUD>.Instance;
		
		if (null == config)
			config = SingletonUtils<ConfigHolder>.Instance;
	}
	
	void Start()
	{
		speed = config.InitialSpeed;
		UpdatePostFX();
	}
	
	void Update()
	{
		var deltaDistance = speed * Time.deltaTime;
		totalDistance += deltaDistance;
		totalTime += Time.deltaTime;

		cTr.Translate(Vector3.forward * deltaDistance);
		
		speedupTimer += Time.deltaTime;
		if (speedupTimer >= config.TimePerSpeedInc)
		{
			speedupTimer = 0f;
			if (null != activeSpeedupTween)
				StopCoroutine(activeSpeedupTween);
			activeSpeedupTween = StartCoroutine(TweenSpeedup(speed,
				Mathf.Min(config.SpeedCap, speed + config.SpeedIncStep)));
		}
		
		if (cTr.position.z > config.GlobalShiftThreshold)
		{
			cTr.Translate(Vector3.back * config.GlobalShiftThreshold);
			if (null != GlobalShiftEvent)
				GlobalShiftEvent();
		}
		
		hud.UpdateSpeedTxt(speed);
		hud.UpdateTimeTxt(totalTime);
		hud.UpdateDistTxt(totalDistance);
	}
	
	IEnumerator TweenSpeedup(float from, float to)
	{
		var tweenDuration = Mathf.Min(config.SpeedupTweenTime, config.TimePerSpeedInc);
		var tweenTmr = 0f;
		
		while (true)
		{
			yield return null;
			
			tweenTmr += Time.deltaTime;
			speed = Mathf.Lerp(from, to, tweenTmr / tweenDuration);
			UpdatePostFX();
			
			if (tweenTmr >= tweenDuration)
			{
				speed = to;
				yield break;
			}
		}
	}
	
	void UpdatePostFX()
	{
		if (null != FX)
		{
			var settings = FX.motionBlur.settings;
			settings.frameBlending = speed / config.SpeedCap;
			FX.motionBlur.settings = settings;
		}
	}
}
