using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
	public BossHealth health;
	public Slider slider;

	void Start()
	{
		slider.maxValue = health.currentHealth;
	}

	// Update is called once per frame
	void Update()
    {
		slider.value = health.currentHealth;
		if (health.currentHealth <= 0) {
			Destroy(gameObject);
		}
    }
}
