using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioClip[] expSound;
    public AudioClip clip;

    public bool jobDone = false;

    public GameObject ragDoll;
    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth ph = FindObjectOfType<PlayerHealth>();
        if (ph != null)
        {
            float dist = Vector3.Distance(transform.position, ph.gameObject.transform.position);
            if (dist <= 6)
            {
                ph.Injure(100f);
            }
            else if (dist > 6 && dist <= 8)
            {
                ph.Injure(50f);
            }
            else if (dist > 8 && dist <= 11)
            {
                ph.Injure(25f);
            }
        }

        GetComponent<AudioSource>().PlayOneShot(clip, 1f);

        Invoke("Ragdoll", .1f);
    }

    void Ragdoll()
    {
        if (!jobDone)
        {
            ragDoll = GameObject.FindGameObjectWithTag("ragdoll");
            if (ragDoll != null)
            {
                ragDoll.GetComponent<Rigidbody>().AddExplosionForce(470000, transform.position, 100, 3.0F);
                ragDoll.tag = "Untagged";
                jobDone = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
