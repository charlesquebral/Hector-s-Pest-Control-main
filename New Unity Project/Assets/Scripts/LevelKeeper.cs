using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelKeeper : MonoBehaviour
{
    private static LevelKeeper instance;

    public ScoreKeeper sk;
    public Respawn r;

    public Image blackout;
    public Color target;
    public Color[] options;
    public TextMeshProUGUI level;
    public TextMeshProUGUI desc;
    public string[] desctext;
    public int stage;

    bool affect = true;
    bool add = false;

    public HouseData[] hd;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            if (instance == this)
            {
                blackout = GameObject.Find("blackout").GetComponent<Image>();
                level = GameObject.Find("level").GetComponent<TextMeshProUGUI>();
                desc = GameObject.Find("desc").GetComponent<TextMeshProUGUI>();
                add = false;
                instance.StartCoroutine(Begin());
                sk = FindObjectOfType<ScoreKeeper>();
                r = FindObjectOfType<Respawn>();
                hd = FindObjectsOfType<HouseData>();
                SetHouseAbilities();
            }
        }
        else
        {
            Destroy(instance);
        }
    }

    private void Update()
    {
        if (affect)
        {
            blackout.color = Color.Lerp(blackout.color, target, 12 * Time.deltaTime);
        }

        if (sk != null)
        {
            if (sk.started)
            {
                if (sk.gameOver)
                {
                    if (!add)
                    {
                        //if (sk.gradeAvg >= 0.70f)
                        //{
                            stage++;
                        //}
                        StartCoroutine(BeginNew());
                        add = true;
                    }
                }
            }
        }
    }

    IEnumerator BeginNew()
    {
        yield return new WaitForSeconds(5);
        r.active = false;
        target = options[1];
        affect = true;
        yield return new WaitForSeconds(.25f);
        SceneManager.LoadScene(1);
    }

    public IEnumerator Begin()
    {
        target = options[1];
        blackout.color = options[1];
        level.enabled = true;
        desc.enabled = true;
        level.text = "Stage " + (stage + 1);
        yield return new WaitForSeconds(6);
        sk.StartTimer();
        r.active = true;
        r.SpawnPlayer();
        level.enabled = false;
        desc.enabled = false;
        target = options[0];
        affect = false;
    }

    public void SetHouseAbilities()
    {
        if (stage <= 7)
        {
            desc.text = desctext[stage];
            for (int i = 0; i < hd.Length; i++)
            {
                hd[i].increment = true;
                hd[i].maxAbilities = stage + 1;
            }
        }
        else if (stage > 7 && stage <= 10)
        {
            desc.text = desctext[8];
            for (int i = 0; i < hd.Length; i++)
            {
                hd[i].increment = false;
                hd[i].abilities = new int[2];
                for (int j = 0; j < hd[i].abilities.Length; j++)
                {
                    hd[i].abilities[j] = Random.Range(0, 7);
                }
            }
        }
        else if (stage > 11 && stage <= 15)
        {
            for (int i = 0; i < hd.Length; i++)
            {
                hd[i].increment = false;
                hd[i].abilities = new int[3];
                for (int j = 0; j < hd[i].abilities.Length; j++)
                {
                    hd[i].abilities[j] = Random.Range(0, 7);
                }
            }
        }
        else if (stage > 15)
        {
            for (int i = 0; i < hd.Length; i++)
            {
                hd[i].increment = false;
                hd[i].abilities = new int[4];
                for (int j = 0; j < hd[i].abilities.Length; j++)
                {
                    hd[i].abilities[j] = Random.Range(0, 7);
                }
            }
        }
    }
}
