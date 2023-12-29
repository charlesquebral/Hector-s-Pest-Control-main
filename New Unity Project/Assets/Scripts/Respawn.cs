using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Respawn : MonoBehaviour
{
    public Vector3 spawn;
    public GameObject player;
    public Color[] options;
    public Color target;
    public Image blackout;
    public Image damage;

    public bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            blackout.color = Color.Lerp(blackout.color, target, 12 * Time.deltaTime);
        }
    }

    public IEnumerator BeginRespawn()
    {
        yield return new WaitForSeconds(3);
        target = options[1];
        damage.color = options[0];
        yield return new WaitForSeconds(1);
        HouseData[] houses = FindObjectsOfType<HouseData>();
        for (int i = 0; i < houses.Length; i++)
        {
            houses[i].player = null;
        }
        SpawnPlayer();
        target = options[0];
    }

    public void SpawnPlayer()
    {
        GameObject GO = Instantiate(player, spawn, Quaternion.identity);
        GO.transform.SetParent(null);
        GO.GetComponent<PlayerControl>().playerCamera.gameObject.SetActive(false);
        GO.GetComponent<PlayerControl>().playerCamera.gameObject.SetActive(true);
    }
}
