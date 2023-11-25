using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator anim;
    private static System.Random r = new System.Random();
    public Vector3 destCoord = new Vector3(r.Next(-50, 50), 0, r.Next(-50, 50));
    public bool readyNext = false;
    public int lowerX, upperX, lowerZ, upperZ;
    public GameObject ragDoll, squat;

    public bool isEnabled;

    void Start()
    {
        StartCoroutine(Wander());
    }

    void Update()
    {
        anim.SetFloat("mag", GetComponent<NavMeshAgent>().velocity.magnitude);

        if (isEnabled)
        {
            if (agent.remainingDistance == 0 && readyNext)
            {
                StartCoroutine(Wander());
                readyNext = false;
            }
        }
    }

    IEnumerator Wander()
    {
        yield return new WaitForSeconds(r.Next(0, 5));
        destCoord = new Vector3(r.Next(lowerX, upperX), 0, r.Next(lowerZ, upperZ));
        if (isEnabled)
        {
            agent.SetDestination(destCoord);
        }
        readyNext = true;
    }

    public void Die()
    {
        if (isEnabled)
        {
            ragDoll.SetActive(true);
            ragDoll.transform.SetParent(null);
            Destroy(gameObject);
        }
    }

    public void Squat()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(2.5f, .1f, 3.5f), Time.deltaTime * 5);
        anim.enabled = false;
        isEnabled = false;
        agent.enabled = false;
    }
}
