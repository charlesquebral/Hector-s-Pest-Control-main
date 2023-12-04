using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool open = false;
    public Vector3[] targRot;
    public Vector3 targetRotation;
    public GameObject doors;
    public PlayerControl player;
    public float playerRange = 1;
    public float time = .75f;
    public bool doorComplete = true;

    public string status;
    bool front = false;

    public bool closest = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
    }

    public void Action()
    {
        open = !open;
        if (front)
        {
            targetRotation = targRot[1];
        }
        else
        {
            targetRotation = targRot[2];
        }
        doorComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        player = FindObjectOfType<PlayerControl>();

        if (open)
        {
            doors.transform.localRotation = Quaternion.Slerp(doors.transform.localRotation, Quaternion.Euler(targetRotation), time * Time.deltaTime);

            if (doors.transform.localRotation == Quaternion.Euler(targetRotation))
            {
                doorComplete = true;
            }
        }
        else
        {
            doors.transform.localRotation = Quaternion.Slerp(doors.transform.localRotation, Quaternion.Euler(targRot[0]), time * Time.deltaTime);

            if (doors.transform.localRotation == Quaternion.Euler(targRot[0]))
            {
                doorComplete = true;
            }
        }

        if (player != null)
        {
            Vector3 directionToTarget = transform.position - player.transform.position;
            float angle = Vector3.Angle(transform.right, directionToTarget);
            if (Mathf.Abs(angle) < 90)
            {
                status = "target is in front of me";
                front = true;
            }
            else
            {
                status = "target is behind me";
                front = false;
            }
        }
    }
}
