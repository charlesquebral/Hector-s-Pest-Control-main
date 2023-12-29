using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxhealth = 100;
    public int looper = 5;
    public GameObject[] ragDoll;
    public GameObject bar;
    public GameObject damage;
    public float transp;

    public bool godMode = false;

    public HouseData currentHouse;

    // Start is called before the first frame update
    void Start()
    {
        health = maxhealth;
        bar = GameObject.Find("Health");
        damage = GameObject.Find("Damage");
    }

    private void Update()
    {
        if (health <= 0 || Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }

        float perc = (health / maxhealth);
        if (bar != null)
        {
            bar.GetComponent<Image>().fillAmount = perc;
        }

        if (transp > 0)
        {
            transp -= .2f * Time.deltaTime;
        }
        Color newCol = new Color(1, 1, 1, transp);

        if (damage != null)
        {
            damage.GetComponent<Image>().color = newCol;
        }
    }

    public void Injure(float reduction)
    {
        if (!godMode)
        {
            if (reduction <= 2.5f)
            {
                transp = .75f;
            }
            else
            {
                transp = 1f;
            }
            health -= reduction;
        }
    }

    public IEnumerator InjureLoop(float reduction)
    {
        Injure(reduction);
        looper--;
        yield return new WaitForSeconds(1.25f);
        if (looper > 0)
        {
            StartCoroutine(InjureLoop(reduction));
        }
    }

    public void Die()
    {
        Respawn r = FindObjectOfType<Respawn>();
        r.StartCoroutine(r.BeginRespawn());
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        GameObject GO = Instantiate(ragDoll[Random.Range(0, ragDoll.Length)], spawnPos, transform.rotation);
        currentHouse.player = GO;
        GO.transform.SetParent(currentHouse.insides.transform);
        Destroy(gameObject);
    }
}
