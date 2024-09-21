using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    public GameObject credits;
    private bool toggle = false;

    public void toggleCredits()
    {
        toggle = !toggle;
        credits.SetActive(toggle);
    }

    private void Start()
    {
        credits.SetActive(toggle);
    }


    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void QuitGameM()
    {
        Application.Quit();
    }

}
