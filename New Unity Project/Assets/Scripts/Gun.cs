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

    // Start is called before the first frame update
    void Start()
    {
        ammo = maxammo;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ammo > 0 && time <= 0)
        {
            Fire();
            ammo--;
            time = timeBetweenShots;
        }

        if (ammo <= 0)
        {
            ammo = maxammo;
        }

        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            gunAnim.SetInteger("state", 0);
        }
    }

    public void Fire()
    {
        auso.Play();
        ps.Play();
        for (int i = 0; i < 12; i++)
        {
            Vector3 spreadDirection = muzzle.forward;
            float randomZangle = Random.Range(-spreadAngle, spreadAngle);
            float randomXangle = Random.Range(-spreadAngle, spreadAngle);
            spreadDirection = Quaternion.Euler(randomXangle, randomZangle, 0) * spreadDirection;
            GameObject GO = GameObject.Instantiate(bullet, muzzle.transform.position, Quaternion.identity);
            Rigidbody bulletRigidbody = GO.GetComponent<Rigidbody>();
            bulletRigidbody.velocity = spreadDirection * GO.GetComponent<Bullet>().bulletStrength;
        }
        gunAnim.SetInteger("state", 1);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<AI>())
        {
            hit.gameObject.GetComponent<AI>().Squat();
        }
    }
}