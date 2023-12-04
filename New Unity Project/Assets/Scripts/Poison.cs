using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    [SerializeField]
    private Rigidbody cannonballInstance;

    public GameObject impact;

    [SerializeField]
    [Range(10f, 80f)]
    private float angle = 45f;

    public bool jobDone = false;

    private void Update()
    {

    }

    public void FireCannonAtPoint(Vector3 point)
    {
        var velocity = BallisticVelocity(point, angle);

        cannonballInstance.transform.position = transform.position;
        cannonballInstance.velocity = velocity;
    }

    private Vector3 BallisticVelocity(Vector3 destination, float angle)
    {
        Vector3 dir = destination - transform.position; // get Target Direction
        float height = dir.y; // get height difference
        dir.y = 0; // retain only the horizontal difference
        float dist = dir.magnitude; // get horizontal direction
        float a = angle * Mathf.Deg2Rad; // Convert angle to radians
        dir.y = dist * Mathf.Tan(a); // set dir to the elevation angle.
        dist += height / Mathf.Tan(a); // Correction for small height differences

        // Calculate the velocity magnitude
        float velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return velocity * dir.normalized; // Return a normalized vector.
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!jobDone)
        {
            GameObject GO = Instantiate(impact, transform.position, Quaternion.identity);
            GetComponent<DeathTimer>().StartDeath();
            GO.transform.SetParent(null);
            if (collision.gameObject.GetComponent<PlayerHealth>())
            {
                PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
                ph.looper = 5;
                ph.StartCoroutine(ph.InjureLoop(2.5f));
            }
            else
            {
                PlayerHealth ph = FindObjectOfType<PlayerHealth>();
                if (ph != null)
                {
                    float dist = Vector3.Distance(transform.position, ph.gameObject.transform.position);
                    if (dist <= 5)
                    {
                        ph.looper = 3;
                        ph.StartCoroutine(ph.InjureLoop(2.5f));
                    }
                }
            }
            jobDone = true;
        }
    }
}
