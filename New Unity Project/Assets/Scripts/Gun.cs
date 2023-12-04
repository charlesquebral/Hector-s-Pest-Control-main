using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int ammo;
    public int maxammo;

    public float time;
    public float timeBetweenShots;

    public Transform muzzle;
    public GameObject bullet;

    public float spreadAngle = 5.0f;

    public AudioSource auso;
    public ParticleSystem ps;

    public Animator gunAnim;

    public ScoreKeeper sk;

    public GameObject laser;

    public Camera cam;

    public ProceduralRecoil pr;

    public bool isReloading = false;
    public int reloadingFrom = 9;
    bool indoor = false;
    public AudioClip ind, outd, cock, reload;

    Coroutine reloading;

    public bool batchCalc = false;

    // Start is called before the first frame update
    void Awake()
    {
        sk = FindObjectOfType<ScoreKeeper>();
        ammo = maxammo;
        time = timeBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ammo > 0 && time >= timeBetweenShots)
        {
            if (reloading != null)
            {
                StopCoroutine(reloading);
                isReloading = false;
            }
            Fire();
            time = 0;
        }

        if (((Input.GetKeyDown(KeyCode.R) && ammo < maxammo) || (ammo <= 0)) && !isReloading)
        {
            isReloading = true;
            reloadingFrom = ammo;
            gunAnim.SetInteger("state", 2);
            reloading = StartCoroutine(ReloadTimer());
        }

        if (time < timeBetweenShots)
        {
            time += Time.deltaTime;
        }
        else
        {
            if (!isReloading)
            {
                gunAnim.SetInteger("state", 0);
            }
        }

        RaycastHit hit;
        Ray ray = new Ray(muzzle.position, muzzle.forward);
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            if (hit.collider != null)
            {
                laser.transform.position = hit.point;
            }
        }
    }

    public void Reload()
    {
        ammo++;
    }

    public IEnumerator ReloadTimer()
    {
        yield return new WaitForSeconds(0.38333333333f);
        if (ammo < maxammo)
        {
            Reload();
            auso.PlayOneShot(reload, 1f);
        }
        yield return new WaitForSeconds(0.33333333333f);
        if (ammo < maxammo)
        {
            reloading = StartCoroutine(ReloadTimer());
        }
        else
        {
            if (reloadingFrom == 0)
            {
                isReloading = false;
                gunAnim.SetInteger("state", 1);
                auso.PlayOneShot(cock, 1f);
            }
            else
            {
                isReloading = false;
                gunAnim.SetInteger("state", 0);
            }
        }
    }

    public void Fire()
    {
        batchCalc = false;
        ammo--;
        sk.shotsTaken++;
        if (indoor)
        {
            auso.PlayOneShot(ind, 1f);
        }
        else
        {
            auso.PlayOneShot(outd, 1f);
        }
        if (ammo > 0)
        {
            auso.PlayOneShot(cock, 1f);
        }
        ps.Play();

        pr.Recoil();

        for (int i = 0; i < 12; i++)
        {
            Vector3 spreadDirection = muzzle.forward;
            float randomZangle = Random.Range(-spreadAngle, spreadAngle);
            float randomXangle = Random.Range(-spreadAngle, spreadAngle);
            spreadDirection = Quaternion.Euler(randomXangle, randomZangle, 0) * spreadDirection;
            GameObject GO = GameObject.Instantiate(bullet, muzzle.transform.position, Quaternion.identity);
            Rigidbody bulletRigidbody = GO.GetComponent<Rigidbody>();
            GO.GetComponent<Bullet>().shooter = this;
            bulletRigidbody.velocity = spreadDirection * GO.GetComponent<Bullet>().bulletStrength;
        }
        gunAnim.SetInteger("state", 1);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<AI>())
        {
            if (hit.gameObject.GetComponent<AI>().isEnabled && hit.gameObject.GetComponent<AI>().ability != 2)
            {
                hit.gameObject.GetComponent<AI>().isEnabled = false;
                hit.gameObject.GetComponent<AI>().squat = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<HouseData>())
        {
            indoor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<HouseData>())
        {
            indoor = false;
        }
    }
}