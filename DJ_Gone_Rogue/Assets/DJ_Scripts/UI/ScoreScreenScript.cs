using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class ScoreScreenScript : MonoBehaviour {
	public bool isActivated = false;
	private int deathCount;
	private float completedTime;

	public AudioSource reward1, reward2, reward3, missedStar;

	//objects used for shaking
	private GameObject shakeObject;
	private float shake_intensity, shake_decay;
	private Vector3 originalPosition;

	//references to the gameObjects we use
	public GameObject completionStar,deathStar, timeStar;
	public GameObject completionFailed, deathFailed, timeFailed;
	public GameObject starDescription;

	private int starAmount = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(shake_intensity > 0){
			shakeObject.transform.position = originalPosition + Random.insideUnitSphere * shake_intensity;
			shake_intensity -= shake_decay;
		}
	}

	public void Activate(int deathCount, float completedTime){
		starDescription.GetComponent<UILabel>().text = "";
		this.deathCount = deathCount;
        this.completedTime = completedTime;

		//this should only go in once
		if(isActivated == false){

			//moves the Score menu
			//TweenPosition.Begin(gameObject, 1, gameObject.transform.localPosition + new Vector3(0,650,0));

            StartCoroutine(moveScreenAfterSeconds(4));

			//calls the stars at different times
			StartCoroutine(CompletionStar(6));
			StartCoroutine(DeathStar(8));
			StartCoroutine(TimeStar(10));
		}
		isActivated = true;
	}

	//checks to see if the player earned the completion star and shows it if they have
	private IEnumerator CompletionStar(float waitTime){
		yield return new WaitForSeconds(waitTime);
		//Debug.Log("Activating completion star");
		starDescription.GetComponent<UILabel>().text = "Completed the level.";
		DropStar(completionStar, 345);
	}

	//checks to see if the player earned the completion star and shows it if they have
	private IEnumerator DeathStar(float waitTime){
		yield return new WaitForSeconds(waitTime);
		//Debug.Log("Activating death star");
		starDescription.GetComponent<UILabel>().text = 
			" Max amount of deaths: " + PlayerPrefs.GetInt("maxDeaths" + DJ_LevelManager.currentLevel.ToString()) +
			"\nYour deaths: " + deathCount;
		if (deathCount <= PlayerPrefs.GetInt("maxDeaths" + DJ_LevelManager.currentLevel.ToString())){
			DropStar(deathStar, 0);
		}
		else {
			shake (deathFailed);
		}
	}

	//checks to see if the player earned the completion star and shows it if they have
	private IEnumerator TimeStar(float waitTime){
		yield return new WaitForSeconds(waitTime);
		//Debug.Log("Activating time star");
		starDescription.GetComponent<UILabel>().text = 
			"Goal Time: " + PlayerPrefs.GetFloat("starTime" + DJ_LevelManager.currentLevel.ToString()) + " seconds" +
            "\nYour Time: " + (int)completedTime + " seconds";
        if (completedTime <= PlayerPrefs.GetFloat("starTime" + DJ_LevelManager.currentLevel.ToString()))
        {
			DropStar(timeStar, 15);
		}
		else {
			shake (timeFailed);
			missedStar.Play();
		}

	}

	private void shake(GameObject shakeObject){
		missedStar.Play();
		this.shakeObject = shakeObject;
		shakeObject.GetComponent<UITexture>().color = Color.grey;
		originalPosition = shakeObject.transform.position;
		shake_intensity = .08f;
		shake_decay = 0.003f;
	}

	private void DropStar(GameObject star, float endAngle){
		starAmount++;
		star.GetComponent<UITexture>().color = Color.cyan;
		TweenPosition.Begin(star, 1, star.transform.localPosition + new Vector3(0,0,10));
		TweenRotation.Begin(star, 1, Quaternion.AngleAxis(endAngle, -star.transform.forward));
		TweenScale.Begin(star, 1, new Vector3(0.015f, 0.015f, 0.015f));
		switch(starAmount){
		case 1:
			StartCoroutine(playSoundAfterSeconds(1, reward1));
			break;
		case 2:
			StartCoroutine(playSoundAfterSeconds(1, reward2));
			break;
		case 3:
			StartCoroutine(playSoundAfterSeconds(1, reward3));
			break;
		default:
			break;
		}
	}

	private IEnumerator playSoundAfterSeconds(float waitTime, AudioSource sound){
		yield return new WaitForSeconds(waitTime);
		sound.Play();
	}

    private IEnumerator moveScreenAfterSeconds(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        TweenPosition.Begin(gameObject, 1, gameObject.transform.localPosition + new Vector3(0, 650, 0));
    }

}
