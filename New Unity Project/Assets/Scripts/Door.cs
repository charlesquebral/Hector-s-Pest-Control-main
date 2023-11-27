using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool open = false;
    public Vector3[] targRot;
    public GameObject[] doors;
    public GameObject player;
    public float playerRange = 1;
    public float time = .75f;
    public bool doorComplete = true;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControl>().gameObject;    
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist <= playerRange)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                open = !open;
                doorComplete = false;
            }
        }

        if (open)
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].transform.localRotation = Quaternion.Slerp(doors[i].transform.localRotation, Quaternion.Euler(targRot[2 + i]), time * Time.deltaTime);
            }

            if (doors[0].transform.localRotation == Quaternion.Euler(targRot[2]))
            {
                doorComplete = true;
            }
        }
        else
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].transform.localRotation = Quaternion.Slerp(doors[i].transform.localRotation, Quaternion.Euler(targRot[i]), time * Time.deltaTime);
            }

            if (doors[0].transform.localRotation == Quaternion.Euler(targRot[0]))
            {
                doorComplete = true;
            }
        }
    }
}
