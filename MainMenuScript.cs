using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{

    public GameObject[] slides;

    public GameObject playbutton;
    //public GameObject mainMenu;
    public AudioSource tutSound;
    public AudioSource music;

    public bool tut;
    public float tutTime;

    public Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        print(PlayerPrefs.GetInt("PlayedTut"));
        if (PlayerPrefs.GetInt("PlayedTut") == 0)
        {
            playbutton.SetActive(false);
        }
        timeText.gameObject.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void StartTutorial()
    {
        tut = true;
        tutSound.Play();
        music.Stop();
        tutTime = 0;
    }

    public void SkipTutrial()
    {
        tutTime = 200f;
    }

    public void SlideIn(int id)
    {
        foreach (GameObject s in slides)
        {
            s.SetActive(false);
        }

        slides[id].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;

        if (tut == true)
        {
            tutTime += Time.deltaTime;
            timeText.text = (105 - (int)tutTime).ToString() + " Sec";
        }
        if (1 < tutTime && tutTime < 40)
        {
            SlideIn(1);
            timeText.gameObject.SetActive(true);
        }
        if (40 < tutTime && tutTime < 44)
        {
            SlideIn(2);
        }
        if (44 < tutTime && tutTime < 55)
        {
            SlideIn(3);
        }
        if (55 < tutTime && tutTime < 60)
        {
            SlideIn(4);
        }
        if (60 < tutTime && tutTime < 73)
        {
            SlideIn(5);
        }
        if (73 < tutTime && tutTime < 80)
        {
            SlideIn(6);
        }
        if (80 < tutTime && tutTime < 101)
        {
            SlideIn(7);
        }
        if (tutTime > 105)
        {
            tut = false;
            tutSound.Stop();
            music.Play();
            PlayerPrefs.SetInt("PlayedTut", 1);
            playbutton.SetActive(true);
            tutTime = 0;
            SlideIn(0);
            timeText.text = "";
            timeText.gameObject.SetActive(true);
        }
    }
}

/*
 40 seconds sword(first slot)
44 second weapon bow
55 sec handgun
1:00 spell book (60 sec)
1:13 teleporter (73sec)
1:20 shop (80)
end (101)
*/
