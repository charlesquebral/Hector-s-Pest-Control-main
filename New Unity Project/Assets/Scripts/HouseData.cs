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

    // Start is called before the first frame update
    void Start()
    {
        Destroy(insides);
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
            if (insides == null)
            {
                GameObject GO = Instantiate(fauxInsides, transform.position, transform.rotation);
                insides = GO;
            }

            if (!Hectors.activeSelf)
            {
                Hectors.SetActive(true);
            }
        }
        else
        {
            if (insides != null)
            {
                Destroy(insides);
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
}
