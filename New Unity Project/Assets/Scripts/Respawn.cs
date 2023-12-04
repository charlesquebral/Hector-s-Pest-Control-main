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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        blackout.color = Color.Lerp(blackout.color, target, 12 * Time.deltaTime);
    }

    public IEnumerator BeginRespawn()
    {
        yield return new WaitForSeconds(3);
        target = options[1];
        yield return new WaitForSeconds(1);
        GameObject GO = Instantiate(player, spawn, Quaternion.identity);
        GO.transform.SetParent(null);
        GO.GetComponent<PlayerControl>().playerCamera.gameObject.SetActive(false);
        GO.GetComponent<PlayerControl>().playerCamera.gameObject.SetActive(true);
        target = options[0];
    }
}
