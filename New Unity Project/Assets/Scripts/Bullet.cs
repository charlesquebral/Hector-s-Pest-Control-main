using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public float bulletStrength;
    public int lifetime;

    // Start is called before the first frame update
    void Start()
    {
        //rb.AddForce(transform.forward * bulletStrength);
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<AI>())
        {
            collision.gameObject.GetComponent<AI>().Die();
        }
        Destroy(gameObject);
    }

    IEnumerator DieAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
