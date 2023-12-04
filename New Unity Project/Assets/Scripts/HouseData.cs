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

    public GameObject Hectors;

    bool playerExists;

    // Start is called before the first frame update
    void Start()
    {
        playerExists = player == null;
        insides.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerExists != (player == null))
        {
            if (player == null)
            {
                player = gameObject;
                current = StartCoroutine(SetAsNull());
            }
            playerExists = player == null;
        }

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
            if (current != null)
            {
                StopCoroutine(current);
            }

            if (!insides.activeSelf)
            {
                insides.SetActive(true);
            }

            if (!Hectors.activeSelf)
            {
                Hectors.SetActive(true);
            }
        }
        else
        {
            if (insides.activeSelf)
            {
                insides.SetActive(false);
            }

            if (Hectors.activeSelf)
            {
                Hectors.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerControl>())
        {
            player = other.gameObject;
        }
        else if (other.GetComponent<AI>())
        {
            if (!other.gameObject.transform.parent == Hectors.transform)
            {
                other.gameObject.transform.SetParent(Hectors.transform);
            }
        }
    }

    Coroutine current;
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerControl>())
        {
            player = null;
        }
        else if (other.GetComponent<AI>())
        {
            if (other.gameObject.transform.parent == Hectors.transform)
            {
                other.gameObject.transform.SetParent(gameObject.transform);
            }
        }
    }

    public IEnumerator SetAsNull()
    {
        yield return new WaitForSeconds(5);
        GameObject ragDoll = GameObject.FindGameObjectWithTag("ragdoll");
        if (ragDoll != null)
        {
            ragDoll.transform.SetParent(insides.transform);
            ragDoll.tag = "Untagged";
        }
        player = null;
    }
}
