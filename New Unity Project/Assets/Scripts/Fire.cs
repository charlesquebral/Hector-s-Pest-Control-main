using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public ParticleSystem fire;
    public PlayerControl pc;
    public float dist = 10;
    public bool firefire = false;

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
        else
        {
            Vector3 targ = pc.transform.position + Vector3.up * 1.5f;
            dist = Vector3.Distance(transform.position, pc.transform.position);
            RaycastHit hit;
            Vector3 spawn = transform.position + transform.up * .25f;
            Vector3 rayDirection = targ - spawn;

            if (Physics.Raycast(spawn, rayDirection, out hit, 100f))
            {
                if (hit.collider.gameObject.transform.root.gameObject == pc.gameObject && dist < 10f && firefire)
                {
                    if (!fire.isPlaying)
                    {
                        fire.Play();
                    }
                }
                else
                {
                    if (fire.isPlaying)
                    {
                        fire.Stop();
                    }
                }
            }
        }
    }

    public void OnParticleCollision(GameObject other)
    {
        if (pc != null)
        {
            if (other.gameObject.transform.root.gameObject == pc.gameObject)
            {
                pc.GetComponent<PlayerHealth>().Injure(.125f);
            }
        }
    }
}
