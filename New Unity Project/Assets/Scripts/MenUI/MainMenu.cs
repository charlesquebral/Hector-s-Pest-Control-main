using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject main;
    public GameObject options;
    public Color suit;
    public Material suitMat;

    LevelKeeper lk;

    // Start is called before the first frame update
    void Start()
    {
        lk = FindObjectOfType<LevelKeeper>();
        if (lk != null)
        {
            Destroy(lk.gameObject);
        }
        suitMat.color = suit;
        OpenMain();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        suitMat.color = suit;
        SceneManager.LoadScene(1);
    }

    public void OpenOptions()
    {
        main.SetActive(false);
        options.SetActive(true);
    }

    public void OpenMain()
    {
        main.SetActive(true);
        options.SetActive(false);
    }
}
