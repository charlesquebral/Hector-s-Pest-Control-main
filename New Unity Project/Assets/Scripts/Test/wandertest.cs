using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class wandertest : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator anim;
    private static System.Random r = new System.Random();
    public bool readyNext = false;
    public float wanderRadius;
    public Transform source;
    public bool isEnabled;
    public int waitMinTime, waitMaxTime;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(Wander());
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("mag", GetComponent<NavMeshAgent>().velocity.magnitude);

        if (isEnabled)
        {
            if (agent.remainingDistance <= 0 && readyNext)
            {
                StartCoroutine(Wander());
                readyNext = false;
            }
        }
    }

    public IEnumerator Wander()
    {
        int waitTime = r.Next(waitMinTime, waitMaxTime);
        print("waiting " + waitTime + " seconds");
        yield return new WaitForSeconds(waitTime);
        if (isEnabled)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
        }
        readyNext = true;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
