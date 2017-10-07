using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	[SerializeField] Transform txtRoot;
	[SerializeField] Text txtTime;
	[SerializeField] Text txtDist;
	[SerializeField] Text txtSpeed;
	
	[SerializeField] CanvasGroup fadeCG;
	
	void Awake()
	{
		if (null == txtRoot)
			txtRoot = transform.Find("Root");
		if (null == txtTime)
			txtTime = txtRoot.Find("Time").GetComponent<Text>();
		if (null == txtDist)
			txtDist = txtRoot.Find("Distance").GetComponent<Text>();
		if (null == txtSpeed)
			txtSpeed = txtRoot.Find("Speed").GetComponent<Text>();
		
		if (null == fadeCG)
			fadeCG = GetComponentInChildren<CanvasGroup>();
		
		GetComponentInChildren<Toggle>().onValueChanged.AddListener((val) =>
		{
			RenderSettings.fog = val;
			QualitySettings.shadows = val ? ShadowQuality.All : ShadowQuality.Disable;
		});
	}
	
	IEnumerator Start()
	{
		fadeCG.alpha = 1f;
		var tmr = 3f;
		
		yield return new WaitForSeconds(1f);
		while (tmr > 0)
		{
			fadeCG.alpha = tmr / 3f;
			yield return null;
			tmr -= Time.deltaTime;
		}
		fadeCG.gameObject.SetActive(false);
	}
	
	public void UpdateTimeTxt(float newVal)
	{
		txtTime.text = "TIME: " + newVal.ToString("0.000");
	}
	
	public void UpdateDistTxt(float newVal)
	{
		txtDist.text = "DIST: " + newVal.ToString("0.00");
	}
	
	public void UpdateSpeedTxt(float newVal)
	{
		txtSpeed.text = "SPEED " + newVal.ToString("0.000");
	}
	
	public void ToggleFog(bool on)
	{
		RenderSettings.fog = false;
	}
}
