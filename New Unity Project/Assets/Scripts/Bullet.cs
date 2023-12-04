using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public float bulletStrength;
    public int lifetime;
    public GameObject Impact;
    public ScoreKeeper sk;
    public Gun shooter;

    // Start is called before the first frame update
    void Start()
    {
        //rb.AddForce(transform.forward * bulletStrength);
        sk = FindObjectOfType<ScoreKeeper>();
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<AI>())
        {
            if (collision.gameObject.GetComponent<AI>().isEnabled)
            {
                if (!shooter.batchCalc)
                {
                    sk.shotsMade++;
                    shooter.batchCalc = true;
                }
                collision.gameObject.GetComponent<AI>().health -= 5f;
            }
        }
        else
        {
            GameObject GO = Instantiate(Impact, transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
        }
        Destroy(gameObject);
    }

    IEnumerator DieAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
