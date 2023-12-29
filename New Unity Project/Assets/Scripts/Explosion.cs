using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioClip[] expSound;
    public AudioClip clip;

    public bool jobDone = false;

    public GameObject ragDoll;

    public float killrange;
    public float midrange;
    public float farrange;

    public float killDamage, midDamage, farDamage;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth ph = FindObjectOfType<PlayerHealth>();
        if (ph != null)
        {
            float dist = Vector3.Distance(transform.position, ph.gameObject.transform.position);
            if (dist <= killrange)
            {
                ph.Injure(killDamage);
            }
            else if (dist > killrange && dist <= midrange)
            {
                ph.Injure(midDamage);
            }
            else if (dist > midrange && dist <= farrange)
            {
                ph.Injure(farDamage);
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
                Rigidbody[] rdrb = ragDoll.GetComponentsInChildren<Rigidbody>();
                rdrb[0].AddExplosionForce(470000, transform.position, 100, 3.0F);
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
