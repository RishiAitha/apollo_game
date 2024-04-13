using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool  : MonoBehaviour
{
	public static BulletPool bulletPoolInstance;

	public GameObject poolBullet;
	public int totalBulletsInPool;

	private bool needMoreBullets = true;
	private List<GameObject> bulletList;

	private void Awake()
	{
		bulletPoolInstance = this;
	}

	private void Start()
	{
		bulletList = new List<GameObject>();
		totalBulletsInPool = totalBulletsInPool <= 0 ? 10 : totalBulletsInPool;
	}

	public GameObject GetBullet(int index)
	{
		//if (index > bulletList.Count - 1) return null;

		if (bulletList.Count > 0 && !needMoreBullets)
		{
			if (!bulletList[index].activeInHierarchy)
			{
				return bulletList[index];
			}
		}
		else
		{
			GameObject bullet = Instantiate(poolBullet);
			bullet.SetActive(false);
			bulletList.Add(bullet);

			if (bulletList.Count == totalBulletsInPool)
				needMoreBullets = false;

			return bullet;
		}

		/*if (notEnoughtBulletsInPool)
		{
			GameObject bullet = Instantiate(poolBullet);
			bullet.SetActive(false);
			bulletList.Add(bullet);
			return bullet;
		}*/

		return null;
	}
}