using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    //[SerializeField]
    //public bool again = true;

    // Start is called before the first frame update
    void Update()
    {
        

    }

    // Update is called once per frame
    void Start()
    {
        
    }

    public void LoadNormalMode()
    {

        SceneManager.LoadScene(1);
      //  again = false;
    }


    public void LoadRandomMode()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadScanMode()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadMainMenu()
    {
       // again = true;
        SceneManager.LoadScene(0);
    }

    public void LoadHelpScene()
    {
        SceneManager.LoadScene(4);
    }



    public void QuitGame()
    {
        Application.Quit();
    }
}
