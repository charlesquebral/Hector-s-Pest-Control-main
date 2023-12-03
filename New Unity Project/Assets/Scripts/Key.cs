using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Door[] doors;
    public Door closestDoor;
    public Door _closestDoor;
    public float playerRange = 1;

    public string status;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit, 5f))
        {
            if (hit.collider.gameObject.transform.parent != null)
            {
                if (hit.collider.gameObject.transform.parent.parent != null)
                {
                    if (hit.collider.gameObject.transform.parent.parent.GetComponent<Door>())
                    {
                        closestDoor = hit.collider.gameObject.transform.parent.parent.GetComponent<Door>();
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            closestDoor.Action();
                        }
                    }
                    else
                    {
                        closestDoor = null;
                    }
                }
            }
        }

    }
}
