using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoObject : MonoBehaviour
{
	public enum EDecoObjectType { BGTree, FGTree, FGSmall1, FGSmall2, };
	
	public BoxCollider boundingBox;
	public EDecoObjectType decoType;
	
	[SerializeField] Vector2 scaleRandomRange = Vector2.one;
	
	void Awake()
	{
		if (null == boundingBox)
			boundingBox = GetComponent<BoxCollider>();
	}
	
	public void Randomize()
	{
		transform.rotation = Quaternion.Euler(new Vector3(0f, Random.value * 360f, 0f));
		transform.localScale = Vector3.one * Random.Range(scaleRandomRange.x, scaleRandomRange.y);
	}
	
#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		if (null != boundingBox)
		{
			Gizmos.DrawWireCube(boundingBox.bounds.center - Vector3.forward * .02f, boundingBox.bounds.size);
			Gizmos.DrawWireCube(boundingBox.bounds.center - Vector3.right * .02f, boundingBox.bounds.size);
			Gizmos.DrawWireCube(boundingBox.bounds.center, boundingBox.bounds.size);
			Gizmos.DrawWireCube(boundingBox.bounds.center + Vector3.right * .02f, boundingBox.bounds.size);
			Gizmos.DrawWireCube(boundingBox.bounds.center + Vector3.forward * .02f, boundingBox.bounds.size);
		}
	}
#endif
}
