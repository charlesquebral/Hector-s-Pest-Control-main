using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 5f;

    public GameObject explosion;
    public GameObject smoke;

    public PlayerControl pc;

    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerControl>();
        if (pc != null)
        {
            transform.LookAt(pc.transform.position + (Vector3.up * 1.5f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject GO = Instantiate(explosion, transform.position + transform.forward * -1, Quaternion.identity);
        GO.transform.SetParent(null);
        smoke.transform.SetParent(null);
        DeathTimer dt = smoke.GetComponent<DeathTimer>();
        dt.StartDeath();
        Destroy(gameObject);
    }
}
