using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator anim;
    private static System.Random r = new System.Random();
    public Vector3 spawn;
    public Vector3 destCoord = new Vector3(r.Next(-50, 50), 0, r.Next(-50, 50));
    public bool readyNext = false;
    public int lowerX, upperX, lowerZ, upperZ;
    public GameObject ragDoll;

    public bool isEnabled;
    public bool squat = false;

    Collider[] rigColliders;
    Rigidbody[] rigRigidbodies;

    public float step = .3f;
    public AudioSource stepSound;
    public SkinnedMeshRenderer rend;
    public float health;
    ScoreKeeper sk;

    [Header("Abilities")]
    public int ability;
    public Material[] abilmat;
    public Material invisibleMat;
    public bool camo = false;

    void Start()
    {
        sk = FindObjectOfType<ScoreKeeper>();
        sk.numTot += 1;
        stepSound = GetComponent<AudioSource>();
        spawn = transform.position;
        StartCoroutine(WalkSound());
        StartCoroutine(Wander());

        rigColliders = GetComponentsInChildren<Collider>();
        rigRigidbodies = GetComponentsInChildren<Rigidbody>();

        AssignAbilities();
    }

    public void AssignAbilities()
    {
        rend.sharedMaterial = abilmat[ability];

        switch (ability)
        {
            case 1:
                agent.speed = 4.5f;
                break;
            case 2:
                health = 50f;
                agent.speed = .75f;
                break;
            case 3:
                camo = true;
                break;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(WalkSound());
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

            if (camo)
            {
                if (agent.velocity.magnitude > 0)
                {
                    rend.sharedMaterial = abilmat[ability];
                }
                else
                {
                    rend.sharedMaterial = invisibleMat;
                }
            }

            if (health <= 0)
            {
                Die();
            }
        }
        else if (!isEnabled && squat)
        {
            if (rigColliders[0].enabled)
            {
                sk.numDead++;
                OnAIDeath();
            }
            Squat();
        }
    }

    IEnumerator WalkSound()
    {
        yield return new WaitForSeconds(step * Random.Range(0.9f, 1.1f));
        if (isEnabled && GetComponent<NavMeshAgent>().velocity.magnitude > 0f)
        {
            //stepSound.Play();
        }
        StartCoroutine(WalkSound());
    }

    IEnumerator Wander()
    {
        yield return new WaitForSeconds(r.Next(0, 5));
        destCoord = new Vector3(spawn.x + r.Next(lowerX, upperX), spawn.y, spawn.z + r.Next(lowerZ, upperZ));
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
            if (camo)
            {
                rend.sharedMaterial = abilmat[ability];
            }

            sk.numDead++;
            isEnabled = false;
            agent.enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            anim.enabled = false;
        }
    }

    public void Squat()
    {
        if (camo)
        {
            rend.sharedMaterial = abilmat[ability];
        }

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(2.5f, .1f, 3.5f), Time.deltaTime * 10);
        anim.speed = 0;
        isEnabled = false;
        agent.enabled = false;
    }

    public void OnAIDeath()
    {
        foreach (Collider col in rigColliders)
        {
            col.enabled = false;
        }

        foreach (Rigidbody rb in rigRigidbodies)
        {
            rb.isKinematic = true;
        }
    }
}
