using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkill : MonoBehaviour
{
	public float lifeSpan = 5f;
	private float blackHoleLifeSpan;
	private Animator blackHoleAnimator;

	#region Animation Hash ID's
	private readonly int blackHoleLifeSpanID = Animator.StringToHash("LifeSpan");
	#endregion

	private void Start()
	{
		blackHoleLifeSpan = lifeSpan;
		blackHoleAnimator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (blackHoleLifeSpan > 0f)
		{
			if (blackHoleLifeSpan == lifeSpan)
				blackHoleAnimator.Play("BlackHole");

			blackHoleLifeSpan -= Time.deltaTime;
			blackHoleAnimator.SetFloat(blackHoleLifeSpanID, blackHoleLifeSpan);
		}
	}

	private void OnBecameVisible()
	{
		blackHoleLifeSpan = lifeSpan;
	}

	private void Destroy()
	{
		gameObject.SetActive(false);
	}
}