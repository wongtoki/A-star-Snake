using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject startScreen;
    public GameObject gameOver;
    public GameObject gameScreen;
    public AudioClip gameOverMusic;
    public AudioSource effect;
    public float delay;
    public GameObject Head;
    public Text score;
    public Transform[] tails;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        score.text = tails.Length.ToString();

	}

    public void StartGame(GameObject head){
        Head = head;
        startScreen.SetActive(!startScreen.activeSelf);
        gameScreen.SetActive(true);
        Instantiate(Head,Vector3.zero,Quaternion.identity);

    }

    public void Quit(){

        Application.Quit();

    }

    public IEnumerator GameOver(){

        gameOver.SetActive(true);
        PlaySound(gameOverMusic);
        AudioSource aud = GetComponent<AudioSource>();
        aud.loop = false;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void ReloadScene(){

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void PlaySound(AudioClip clip){
        AudioSource aud = GetComponent<AudioSource>();
        aud.clip = clip;
        aud.Play();

    }

    public void PlayEffectSound(AudioClip clip){
        effect.clip = clip;
        effect.Play();
    }

 
}
