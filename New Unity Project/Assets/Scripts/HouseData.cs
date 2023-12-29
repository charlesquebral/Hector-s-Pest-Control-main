using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseData : MonoBehaviour
{
    public GameObject insides;
    public GameObject fauxInsides;
    public Door[] doors;
    public List<Door> openDoors= new List<Door>();
    public GameObject player;

    public GameObject[] hectorPrefab;
    public bool increment;
    public int[] abilities;
    public int maxAbilities = 0;

    ScoreKeeper sk;

    // Start is called before the first frame update
    void Start()
    {
        sk = FindObjectOfType<ScoreKeeper>();
        insides.SetActive(false);
        int i = Random.Range(0, hectorPrefab.Length);
        GameObject GO = Instantiate(hectorPrefab[i], insides.transform);
        foreach (AI g in GO.GetComponentsInChildren<AI>())
        {
            sk.numTot += 1;
            if (increment)
            {
                g.ability = Random.Range(0, maxAbilities);
            }
            else
            {
                g.ability = abilities[Random.Range(0, abilities.Length)];
            }
            g.bounds = GetComponent<BoxCollider>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i].open)
            {
                if (!openDoors.Contains(doors[i]))
                {
                    openDoors.Add(doors[i]);
                }
            }
            else
            {
                if (openDoors.Contains(doors[i]))
                {
                    for (int j = 0; j < openDoors.Count; j++)
                    {
                        if (openDoors[j] == doors[i])
                        {
                            if (openDoors[j].doorComplete)
                                openDoors.Remove(doors[i]);
                        }
                    }
                }
            }
        }

        if (openDoors.Count > 0 || player != null)
        {
            if (!insides.activeSelf)
            {
                insides.SetActive(true);
            }
        }
        else
        {
            if (insides.activeSelf)
            {
                insides.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerControl>())
        {
            player = other.gameObject;
            player.GetComponent<PlayerHealth>().currentHouse = this;

            if (current != null)
            {
                StopCoroutine(current);
            }
        }
        else if (other.GetComponent<AI>())
        {
            if (other.transform.parent != insides.transform)
            {
                other.gameObject.transform.parent = insides.transform;
            }
        }
    }

    Coroutine current;
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerControl>())
        {
            player.GetComponent<PlayerHealth>().currentHouse = null;
            player = null;
        }
        else if (other.GetComponent<AI>())
        {
            if (other.transform.parent == insides.transform)
            {
                other.gameObject.transform.parent = null;
            }
        }
    }
}
