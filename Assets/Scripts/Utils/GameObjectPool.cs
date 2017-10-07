using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObjectPool
{
	public GameObject prefab;
	public Transform root;
	
	private LinkedList<GameObject> pool = new LinkedList<GameObject>();
	
	public void Adopt(GameObject obj)
	{
		pool.AddLast(obj);
		obj.SetActive(false);
		obj.transform.parent = root;
	}
	
	public GameObject Instantiate()
	{
		GameObject ret = null;

		if (null != pool.First)
		{
			ret = pool.First.Value;
			pool.RemoveFirst();
			ret.SetActive(true);
			return ret;
		}
		
		if (null != prefab)
		{
			ret = Object.Instantiate(prefab, root, false) as GameObject;
			ret.transform.parent = root;
		}
		
		return ret;
	}
}