using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.XR.Interaction.Toolkit;

public class GameMaster : MonoBehaviour
{
	public ActionBasedContinuousMoveProvider abContinousMovePB;
	public enum HealthStatus { Healthy, OK, Tired, Bad, Critical };
	public enum Disposition { Friendly, Neutral, Hostile, ExtremeHatred };
	public enum HitTypes { Max, Regular, Small };
	private float currentHealth = 100f;
	private readonly float maxHealth = 100f;
	private readonly float minHealth = 10f;
	public float maxHit = 10f;
	public float regularHit = 5f;
	public float smallHit = 1f;
	private float currentSpeed;
	public float maxSpeed = 5f;
	public float minSpeed = 0.5f;
	public GameObject thePlayer;

    public void Start()
    {
		currentSpeed = abContinousMovePB.moveSpeed;
		Debug.Log("Current Speed is " + currentSpeed);
    }

	[ContextMenu("Reduce Health Test" )]
	public void TestReduceHelath()
    {
		ReduceHealth(10);
    }

	public void ReduceHealth(float dmg)
	{
		currentHealth -= dmg;
		if (currentHealth < minHealth)
		{
			currentHealth = minHealth;
		}
		UpdateSpeed();
	}

	[ContextMenu("Add Health Test")]
	public void TestAddHelath()
	{
		AddHealth(10);
	}

	public void AddHealth(float heal)
	{
		currentHealth += heal;
		
		if(currentHealth > maxHealth)
        {
			currentHealth = maxHealth;
        }
		UpdateSpeed();
	}

	private void UpdateSpeed()
	{
		// calculate currentSpeed by multipling currentHealth as a percentage times maxSpeed;
		float per = currentHealth / 100;
		currentSpeed = maxSpeed * per;
		if (currentSpeed < minSpeed)
		{
			currentSpeed = minSpeed;
		}
		abContinousMovePB.moveSpeed = currentSpeed;
		Debug.Log("current Speed is " + currentSpeed);
		Debug.Log("System Move Speed is " + abContinousMovePB.moveSpeed);

	}

	public float GetHealth()
	{
		return currentHealth;
	}

	public float GetCurrentSpeed()
	{
		return currentSpeed;
	}

	[ContextMenu("Get Helath Status")]
	public HealthStatus GetHealthStatus()
    {
		HealthStatus thestatus;

		int currentH = (int)math.round(currentHealth);

		// 80, 60, 40, 20, 10
		if (currentH >= 80)
		{
			thestatus = HealthStatus.Healthy;
		}
		else if (currentH >= 60)
		{
			thestatus = HealthStatus.OK;
		}
		else if (currentH >= 40)
		{
			thestatus = HealthStatus.Tired;
		}
		else if (currentH >= 20)
		{
			thestatus = HealthStatus.Bad;
		}
		else if (currentH >= 10)
		{
			thestatus = HealthStatus.Critical;
		} else
        {
			thestatus = HealthStatus.Critical;
			Debug.Log("Could not Find Health Status");
		}

		Debug.Log("HealthStatus is " + thestatus.ToString());

		return thestatus;
		
    }

	public float HitTypesValues(HitTypes hit)
    {
        float ret;
        switch (hit)
		{
			case HitTypes.Max:
				ret = maxHit;
				break;
			case HitTypes.Regular:
				ret = regularHit;
				break;
			case HitTypes.Small:
				ret = smallHit;
				break;
			default:
				ret = 0;
				Debug.Log("No Value Found for HitType");
				break;
		}
		return ret;
	}

	public float GetDamageAmount(Disposition myDisposition)
    {
		float ret;

		switch (myDisposition)
		{
			case Disposition.Friendly:
				ret = 0;
				break;
			case Disposition.Neutral:
				ret = smallHit;
				break;
			case Disposition.Hostile:
				ret = regularHit;
				break;
			case Disposition.ExtremeHatred:
				ret = maxHit;
				break;
			default:
				ret = 0;
				Debug.Log("Item Tag Not Found");
				break;
		}
		return ret;
	}

	public Disposition ChangeDisposition(Disposition myDisposition, bool increase)
    {

		Disposition ret;

		// increase = make more hostile, otherwise decrease hositility;
		if (increase)
		{
			switch (myDisposition)
			{
				case Disposition.Friendly:
					ret = Disposition.Neutral;
					break;
				case Disposition.Neutral:
					ret = Disposition.Hostile;
					break;
				case Disposition.Hostile:
					ret = Disposition.ExtremeHatred;
					break;
				case Disposition.ExtremeHatred:
					ret = Disposition.ExtremeHatred;
					break;
				default:
					ret = myDisposition;
					Debug.Log("Disposition Not Found");
					break;
			}
		} else
		{
			switch (myDisposition)
			{
				case Disposition.Friendly:
					ret = Disposition.Friendly;
					break;
				case Disposition.Neutral:
					ret = Disposition.Friendly;
					break;
				case Disposition.Hostile:
					ret = Disposition.Neutral;
					break;
				case Disposition.ExtremeHatred:
					ret = Disposition.Hostile;
					break;
				default:
					ret = myDisposition;
					Debug.Log("Disposition Not Found");
					break;
			}
		}
		
		return ret;
	}
}
