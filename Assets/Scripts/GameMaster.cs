using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityTimer;

public class GameMaster : MonoBehaviour

{
	public static GameMaster Instance;

	public ActionBasedContinuousMoveProvider abContinousMovePB;
	public enum HealthStatus { Healthy, OK, Tired, Bad, Critical };
	public enum Disposition { Friendly, Neutral, Hostile, ExtremeHatred };
	public enum HitTypes { Max, Regular, Small };
	private float currentHealth = 100f;
	private readonly float maxHealth = 100f;
	private readonly float minHealth = 10f;
	[Range(1f,10f)]
	public float healAmount = 1f;
	public float healWaitTime = 3f;
	public float maxHit = 10f;
	public float regularHit = 5f;
	public float smallHit = 1f;
	private float currentSpeed;
	public float maxSpeed = 5f;
	public float minSpeed = 0.5f;
	public float startStationaryTime = 0f;
	public MeterScript healthMeter;
	public GameObject radar1UI, radar2UI;
	private TextMeshProUGUI radar1, radar2;
	public SightDetector sightDetector;
	private List<FriendOrFoe> TheAnimals;

	private Timer timer;
	public GameObject thePlayer;
	private bool moving = false;
	public bool IsMoving
	{ 
		get
		{
			return moving;
		}

		set
		{
				if (value == true)
				{
					timer = Timer.Register(healWaitTime, TimedHealPlayer, isLooped: true);
					Debug.Log("heal");
				} 
				else
				{
						Timer.Cancel(timer);
					Debug.Log("no healing");
				}
		
			moving = value;
		}
	}

	private void Awake()
    {
        if(Instance == null)
        {
			Instance = this;
        }
		else
        {
			Destroy(this);
        }
		IsMoving = true;

		TheAnimals = new List<FriendOrFoe>();

	}

	public void Start()
	{
		currentSpeed = abContinousMovePB.moveSpeed;
		Debug.Log("Current Speed is " + currentSpeed);
		healthMeter.SetMaxHealth(maxHealth);
		radar1 = radar1UI.GetComponent<TextMeshProUGUI>();
		radar2 = radar2UI.GetComponent<TextMeshProUGUI>();
	}
    public void FixedUpdate()
    {
		healthMeter.SetHealth(currentHealth);
	//	Debug.Log("current health " + currentHealth);
		radar1.SetText("Animals: " + sightDetector.animals + "<br>Trees: " + sightDetector.trees + "<br>Other: " + sightDetector.other);
		radar2.SetText("Rocks: " + sightDetector.rocks + "<br>Bushes: " + sightDetector.bushes);
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
				ret = 0f;
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
				ret = 0f;
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
				ret = 0f;
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
	public void TimedHealPlayer()
    {
		AddHealth(healAmount);
	}

	public void AnimalListAdd(FriendOrFoe fof)
    {
		if(!TheAnimals.Contains(fof))
        {
			TheAnimals.Add(fof);
        }
    }

	public void AnimalListRemove(FriendOrFoe fof)
	{
		if (TheAnimals.Contains(fof))
		{
			TheAnimals.Remove(fof);
		}
	}
}
