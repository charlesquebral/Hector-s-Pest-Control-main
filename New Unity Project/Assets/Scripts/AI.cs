using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator anim;
    private static System.Random r = new System.Random();
    public bool readyNext = false;
    public float wanderRadius;
    public Transform source;

    public bool isEnabled;
    public bool squat = false;

    Collider[] rigColliders;
    Rigidbody[] rigRigidbodies;

    public float step = .3f;
    public AudioSource stepSound;
    public SkinnedMeshRenderer rend;
    public SkinnedMeshRenderer deadRend;
    public float health;
    ScoreKeeper sk;

    public int waitMinTime = 0;
    public int waitMaxTime = 0;

    [Header("Abilities")]
    public int ability;
    public Transform trailholder;
    public GameObject trail;
    public Material[] abilmat;
    [Header("invisible")]
    public bool camo = false;
    public Material invisibleMat;
    public GameObject smokeObj;
    [Header("poison")]
    public bool poison = false;
    public GameObject poisonProj;
    [Header("explosion")]
    public bool explosion = false;
    public GameObject explosionObj;
    [Header("fire")]
    public bool fire = false;
    public GameObject firer;
    public GameObject livefirer;
    [Header("missile")]
    public bool missile = false;
    public GameObject launcher;
    public GameObject livelauncher;
    public GameObject rpod;
    public RuntimeAnimatorController missileAnim;

    PlayerControl pc;
    public BoxCollider bounds;
    Coroutine current;

    public GameObject aliveBody;
    public GameObject deadBody;

    public AudioClip splat;

    void Start()
    {
        sk = FindObjectOfType<ScoreKeeper>();
        stepSound = GetComponent<AudioSource>();
        rigColliders = GetComponentsInChildren<Collider>();
        rigRigidbodies = GetComponentsInChildren<Rigidbody>();
        AssignAbilities();
    }

    public void AssignAbilities()
    {
        rend.sharedMaterial = abilmat[ability];
        deadRend.sharedMaterial = abilmat[ability];

        switch (ability)
        {
            case 0: //powerless
                gameObject.name = "Hector Regular";
                waitMinTime = 0;
                waitMaxTime = 10;
                break;
            case 1: //superfast
                gameObject.name = "Hector Fast";
                agent.speed = 3.5f;
                waitMinTime = 0;
                waitMaxTime = 0;
                Instantiate(trail, trailholder);
                break;
            case 2: //superstrong
                gameObject.name = "Hector Strong";
                health = 100f;
                agent.speed = .75f;
                waitMinTime = 0;
                waitMaxTime = 10;
                break;
            case 3: //invisible
                gameObject.name = "Hector Invisible";
                camo = true;
                waitMinTime = 10;
                waitMaxTime = 30;
                break;
            case 4: //poison
                gameObject.name = "Hector Poison";
                poison = true;
                waitMinTime = 0;
                waitMaxTime = 10;
                break;
            case 5: //explosion
                gameObject.name = "Hector Explosive";
                explosion = true;
                waitMinTime = 0;
                waitMaxTime = 10;
                break;
            case 6: //fire
                gameObject.name = "Hector Blazer";
                fire = true;
                livefirer = Instantiate(firer, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                livefirer.transform.SetParent(gameObject.transform);
                livefirer.transform.localPosition = new Vector3(0, .248f, .478f);
                waitMinTime = 5;
                waitMaxTime = 10;
                break;
            case 7: //missile
                gameObject.name = "Hector RPG";
                missile = true;
                waitMinTime = 2;
                waitMaxTime = 10;
                //anim.runtimeAnimatorController = missileAnim;
                livelauncher = Instantiate(launcher, transform);
                break;
        }
    }

    private void OnEnable()
    {
        if (isEnabled)
        {
            StartCoroutine(Wander());
        }
    }

    void Update()
    {
        if (pc == null)
        {
            pc = FindObjectOfType<PlayerControl>();
        }
        anim.SetFloat("mag", GetComponent<NavMeshAgent>().velocity.magnitude);

        if (isEnabled)
        {
            if (agent.remainingDistance <= 0 && readyNext && current == null)
            {
                current = StartCoroutine(Wander());
                readyNext = false;
            }

            //do abilities
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
            else if (fire)
            {
                if (agent.velocity.magnitude > 0)
                {
                    if (livefirer.GetComponent<Fire>().firefire)
                    {
                        livefirer.GetComponent<Fire>().firefire = false;
                    }
                }
                else
                {
                    if (!livefirer.GetComponent<Fire>().firefire)
                    {
                        livefirer.GetComponent<Fire>().firefire = true;
                    }
                }
            }
            //end abilities

            if (agent.velocity.magnitude <= 0)
            {
                if (pc != null)
                {
                    if (poison || fire || missile)
                    {
                        Vector3 playerPos = new Vector3(pc.transform.position.x, transform.position.y, pc.transform.position.z);
                        transform.LookAt(playerPos);
                    }
                }
            }

            if (health <= 0)
            {
                Die();
            }
        }
        else if (!isEnabled && squat)
        {
            if (agent.enabled)
            {
                stepSound.PlayOneShot(splat, 1f);
                sk.numDead++;
            }
            Squat();
        }
    }


    public IEnumerator Wander()
    {
        //do abilities
        if (poison)
        {
            ShootPoison();
        }
        else if (camo)
        {
            CamoEffect();
        }
        else if (missile)
        {
            ShootMissile();
        }
        int waitTime = r.Next(waitMinTime, waitMaxTime);
        //print(gameObject.name + " waiting " + waitTime + " seconds");
        yield return new WaitForSeconds(waitTime);
        //do ablities
        if (camo)
        {
            CamoEffect();
        }
        if (isEnabled)
        {
            if (bounds != null)
            {
                Vector3 randomPoint;
                randomPoint = new Vector3(
                    Random.Range(bounds.bounds.min.x+1, bounds.bounds.max.x-1),
                    transform.position.y,
                    Random.Range(bounds.bounds.min.z+1, bounds.bounds.max.z-1)
                );
                agent.SetDestination(randomPoint);
            }
        }
        current = null;
        Invoke("ResetReady", .5f);
    }

    void ResetReady()
    {
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
                if (hit.collider.gameObject.transform.root.gameObject == pc.gameObject)
                {
                    GameObject GO = Instantiate(poisonProj, spawnPos, transform.rotation);
                    GO.GetComponent<Poison>().FireCannonAtPoint(targPos);
                }
            }
        }
    }

    public void ShootMissile()
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
                if (hit.collider.gameObject.transform.root.gameObject == pc.gameObject)
                {
                    GameObject GO = Instantiate(rpod, transform.position + Vector3.up * .15f, Quaternion.LookRotation(rayDirection), null);
                }
            }
        }
    }

    public void CamoEffect()
    {
        Vector3 spawn = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
        GameObject GO = Instantiate(smokeObj, spawn, Quaternion.identity);
        GO.transform.SetParent(null);
    }

    public void Explode()
    {
        Vector3 spawn = new Vector3(transform.position.x, transform.position.y + .25f, transform.position.z);
        GameObject GO = Instantiate(explosionObj, spawn, Quaternion.identity);
        GO.transform.SetParent(null);
        Destroy(gameObject);
    }

    public void Die()
    {
        if (isEnabled)
        {
            OnAIDeath();
            sk.numDead++;
            isEnabled = false;
            agent.enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            anim.enabled = false;

            if (poison)
            {
                Vector3 spawn = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
                GameObject GO = Instantiate(poisonProj, spawn, Quaternion.identity);
                GO.transform.SetParent(null);
                Destroy(gameObject);
            }
            else if (camo)
            {
                rend.sharedMaterial = abilmat[ability];
                CamoEffect();
            }
            else if (explosion)
            {
                Explode();
            }
            else if (fire)
            {
                Destroy(livefirer);
            }
            else if (missile)
            {
                Destroy(livelauncher);
            }
        }
    }

    public void Squat()
    {
        if (poison)
        {
            Vector3 spawn = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
            GameObject GO = Instantiate(poisonProj, spawn, Quaternion.identity);
            GO.transform.SetParent(null);
            Destroy(gameObject);
        }
        else if (camo)
        {
            rend.sharedMaterial = abilmat[ability];
            if (isEnabled)
            {
                CamoEffect();
            }
        }
        else if (explosion)
        {
            Explode();   
        }
        else if (fire)
        {
            Destroy(livefirer);
        }

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.25f, .1f, 2.5f), Time.deltaTime * 10);

        anim.speed = 0;
        isEnabled = false;
        agent.enabled = false;
    }

    public void OnAIDeath()
    {
        aliveBody.SetActive(false);
        deadBody.SetActive(true);
        Invoke("FlyAway", .02f);
    }

    public void FlyAway()
    {
        if (pc != null)
        {
            Vector3 dir = transform.position - pc.transform.position;
            foreach (Rigidbody rb in deadBody.GetComponentsInChildren<Rigidbody>())
            {
                rb.AddForce(dir.normalized * Random.Range(2500f, 10000f));
                rb.AddForce(transform.up * Random.Range(1000f, 3500f));
            }
        }
    }
}
