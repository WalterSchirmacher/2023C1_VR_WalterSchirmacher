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
	public float currentHealth = 100f;
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

	public List<GameObject> whoIsVisible;
	public List<GameObject> aniIsVisible;
	public List<GameObject> rockIsVisible;
	public List<GameObject> plantIsVisible;
	public List<GameObject> treeIsVisible;
	public List<GameObject> bushIsVisible;
	public List<GameObject> mushIsVisible;
	public List<GameObject> otherIsVisible;
	public int animals, rocks, plants, trees, bushes, other;
	private Timer radarTimer;

	[Tooltip("How often the radar refreshes.")]
	[Range(1f,20f)]
	public float cycleTime = 5f;

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
					Timer.Cancel(timer);
					Debug.Log("no healing");
				
				} 
				else
				{
					timer = Timer.Register(healWaitTime, TimedHealPlayer, isLooped: true);
					Debug.Log("heal");
				
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
		

		TheAnimals = new List<FriendOrFoe>();

	}

	public void Start()
	{
		currentSpeed = abContinousMovePB.moveSpeed;
		Debug.Log("Current Speed is " + currentSpeed);
		healthMeter.SetMaxHealth(maxHealth);
		radar1 = radar1UI.GetComponent<TextMeshProUGUI>();
		radar2 = radar2UI.GetComponent<TextMeshProUGUI>();
		animals = rocks = plants = trees = bushes = other = 0;
		radarTimer = Timer.Register(cycleTime, UpdateRadar, isLooped: true);
		IsMoving = true;
		UpdateRadar();
	}

	private void UpdateRadar()
    {
		PlayRadarSnds();
		healthMeter.SetHealth(currentHealth);
		radar1.SetText("Animals: " + animals + "<br>Trees: " + trees + "<br>Other: " + other);
		radar2.SetText("Rocks: " + rocks + "<br>Bushes: " + bushes);
	}

	public void AddtoVisible(GameObject go, string goTag)
    {
			switch (goTag)
			{
				case "Animal":
					if (!aniIsVisible.Contains(go))
					{
						aniIsVisible.Add(go);
						AddRadar(goTag);
					}
					break;
				case "Tree":
					if (!treeIsVisible.Contains(go))
					{
						treeIsVisible.Add(go);
						AddRadar(goTag);
					}
					break;
				case "Bush":
						if (!bushIsVisible.Contains(go))
						{
							bushIsVisible.Add(go);
							AddRadar(goTag);
						}
					break;
				case "Rock":
					if (!rockIsVisible.Contains(go))
					{
						rockIsVisible.Add(go);
						AddRadar(goTag);
					}
					break;
				case "Mushroom":
					if (!mushIsVisible.Contains(go))
					{
						mushIsVisible.Add(go);
						AddRadar(goTag);
					}
					break;
				case "MonsterPlant":
					if (!otherIsVisible.Contains(go))
					{
						otherIsVisible.Add(go);
						AddRadar(goTag);
					}
					break;
				default:
					Debug.Log("Item Tag Not Found");
					break;
			}
		
		
	}
	public void NoLongerVisible(GameObject go, string goTag)
    {

		switch (goTag)
		{
			case "Animal":
				if (aniIsVisible.Contains(go))
				{
					aniIsVisible.Remove(go);
					SubtractRadar(goTag);
				}
				break;
			case "Tree":
				if (treeIsVisible.Contains(go))
				{
					treeIsVisible.Remove(go);
					SubtractRadar(goTag);
				}
				break;
			case "Bush":
				if (bushIsVisible.Contains(go))
				{
					bushIsVisible.Remove(go);
					SubtractRadar(goTag);
				}
				break;
			case "Rock":
				if (rockIsVisible.Contains(go))
				{
					rockIsVisible.Remove(go);
					SubtractRadar(goTag);
				}
				break;
			case "Mushroom":
				if (mushIsVisible.Contains(go))
				{
					mushIsVisible.Remove(go);
					SubtractRadar(goTag);
				}
				break;
			case "MonsterPlant":
				if (otherIsVisible.Contains(go))
				{
					otherIsVisible.Remove(go);
					SubtractRadar(goTag);
				}
				break;
			default:
				Debug.Log("Item Tag Not Found");
				break;
		}
    }

	public void PlayRadarSnds()
    {
		Debug.Log("playing sounds");
		StartCoroutine(PLayLocalChucker());
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

	void AddRadar(string gTag)
	{
		switch (gTag)
		{
			case "Rock":
				rocks++;
				break;
			case "Tree":
				trees++;
				break;
			case "Bush":
				bushes++;
				break;
			case "Animal":
				animals++;
				break;
			case "Mushroom":
				other++;
				break;
			case "MonsterPlant":
				other++;
				break;
			default:
				Debug.Log("Item Tag Not Found");
				break;
		}
	}

	void SubtractRadar(string gTag)
	{
		switch (gTag)
		{
			case "Rock":
				rocks--;
				break;
			case "Tree":
				trees--;
				break;
			case "Bush":
				bushes--;
				break;
			case "Animal":
				animals--;
				break;
			case "Mushroom":
				other--;
				break;
			case "MonsterPlant":
				other--;
				break;
			default:
				Debug.Log("Item Tag Not Found");
				break;
		}
	}

	IEnumerator PLayLocalChucker()
	{
		float wait = 0.5f;

		if (aniIsVisible.Count > 0)
		{
			foreach (GameObject go in aniIsVisible)
			{
				ChuckSounds ck = go.GetComponent<ChuckSounds>();
				if (ck)
				{
					ck.AnimalRadarSnd();
				}
			}
			yield return new WaitForSeconds(wait);
		}

		if (treeIsVisible.Count > 0)
		{
			foreach (GameObject go in treeIsVisible)
			{
				ChuckSounds ck = go.GetComponent<ChuckSounds>();
				if (ck)
				{
					ck.TreeRadarSnd();
				}
			}
			yield return new WaitForSeconds(wait);
		}
		/*
		if (bushIsVisible.Count > 0)
		{
			foreach (GameObject go in bushIsVisible)
			{
				ChuckSounds ck = go.GetComponent<ChuckSounds>();
				if (ck)
				{
					ck.BushRadarSnd();
				}
			}
			yield return new WaitForSeconds(wait);
		}
*/
		if (rockIsVisible.Count > 0)
		{
			foreach (GameObject go in rockIsVisible)
			{
				ChuckSounds ck = go.GetComponent<ChuckSounds>();
				if (ck)
				{
					ck.RockRadarSnd();
				}
			}
			yield return new WaitForSeconds(wait);
		}

		if (mushIsVisible.Count > 0)
		{
			foreach (GameObject go in mushIsVisible)
			{
				ChuckSounds ck = go.GetComponent<ChuckSounds>();
				if (ck)
				{
					ck.MushroomRadarSnd();
				}
			}
			yield return new WaitForSeconds(wait);
		}

		if (mushIsVisible.Count > 0)
		{
			foreach (GameObject go in otherIsVisible)
			{
				ChuckSounds ck = go.GetComponent<ChuckSounds>();
				if (ck)
				{
					ck.OtherRadarSnd();
				}
			}
		}
	}
}
