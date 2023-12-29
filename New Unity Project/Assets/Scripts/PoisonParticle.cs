using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonParticle : MonoBehaviour
{
    public PlayerControl pc;
    public bool jobDone = false;

    // Start is called before the first frame update
    void Start()
    {
        if (pc == null)
        {
            pc = FindObjectOfType<PlayerControl>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pc == null)
        {
            pc = FindObjectOfType<PlayerControl>();
        }
    }

    public void OnParticleCollision(GameObject other)
    {
        if (pc != null)
        {
            if (other.gameObject.transform.root.gameObject == pc.gameObject)
            {
                if (!jobDone)
                {
                    pc.GetComponent<PlayerHealth>().Injure(.125f);
                    jobDone = true;
                }
            }
        }
    }
}
