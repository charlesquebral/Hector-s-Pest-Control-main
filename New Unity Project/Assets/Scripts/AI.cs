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
    [Header("invisible")]
    public bool camo = false;
    public Material invisibleMat;
    public int waitTime = 5;
    [Header("poison")]
    public bool poison = false;
    public GameObject poisonProj;
    [Header("explosion")]
    public bool explosion = false;
    public GameObject explosionObj;

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
            case 1: //superfast
                agent.speed = 4.5f;
                waitTime = 0;
                break;
            case 2: //superstrong
                health = 50f;
                agent.speed = .75f;
                break;
            case 3: //invisible
                camo = true;
                waitTime = 15;
                break;
            case 4: //poison
                poison = true;
                break;
            case 5: //explosion
                explosion = true;
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
        if (poison)
        {
            ShootPoison();
        }
        yield return new WaitForSeconds(r.Next(0, waitTime));
        destCoord = new Vector3(spawn.x + r.Next(lowerX, upperX), spawn.y, spawn.z + r.Next(lowerZ, upperZ));
        if (isEnabled)
        {
            agent.SetDestination(destCoord);
        }
        readyNext = true;
    }

    public void ShootPoison()
    {
        PlayerControl pc = FindObjectOfType<PlayerControl>();

        if (pc != null)
        {
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + .75f, transform.position.z);

            Vector3 targPos = new Vector3(pc.gameObject.transform.position.x, pc.gameObject.transform.position.y + 1f, pc.gameObject.transform.position.z);

            RaycastHit hit;
            Vector3 rayDirection = targPos - spawnPos;

            if (Physics.Raycast(spawnPos, rayDirection, out hit, 100))
            {
                print(hit.collider.gameObject);
                if (hit.collider.gameObject == pc.gameObject)
                {
                    GameObject GO = Instantiate(poisonProj, spawnPos, transform.rotation);
                    GO.GetComponent<Poison>().FireCannonAtPoint(targPos);
                }
            }
            else
            {
                print("cant find");
            }
        }
    }

    public void Die()
    {
        if (isEnabled)
        {
            sk.numDead++;
            isEnabled = false;
            agent.enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            anim.enabled = false;

            if (camo)
            {
                rend.sharedMaterial = abilmat[ability];
            }
            else if (explosion)
            {
                Vector3 spawn = new Vector3(transform.position.x, transform.position.y + .25f, transform.position.z);
                GameObject GO = Instantiate(explosionObj, spawn, Quaternion.identity);
                GO.transform.SetParent(null);
                Destroy(gameObject);
            }
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
