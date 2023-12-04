using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSound : MonoBehaviour
{
    public AudioClip[] walk;
    public AudioClip[] sprint;
    public AudioSource asource;

    public PlayerControl pc;

    public float moving0;
    public float moving1;
    public float waitTime;

    public bool wasUngrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        wasUngrounded = pc.characterController.isGrounded;
        StartCoroutine(Sound());
    }

    // Update is called once per frame
    void Update()
    {
        moving0 = pc.legs.GetFloat("forward");
        moving1 = pc.legs.GetFloat("strafe");

        if (wasUngrounded != pc.characterController.isGrounded)
        {
            if (pc.characterController.isGrounded)
            {
                AudioClip clip = sprint[Random.Range(0, sprint.Length)];
                asource.PlayOneShot(clip, 1f);
            }
            wasUngrounded = pc.characterController.isGrounded;
        }

        if (moving0 != 0 || moving1 != 0)
        {
            if (pc.movementSpeed == 8)
            {
                waitTime = .28f;
            }
            else if (pc.movementSpeed == 5)
            {
                waitTime = .5f;
            }
        }
    }

    public IEnumerator Sound()
    {
        if ((moving0 != 0 || moving1 != 0) && pc.characterController.isGrounded)
        {
            if (pc.movementSpeed == 8)
            {
                AudioClip clip = sprint[Random.Range(0, sprint.Length)];
                asource.PlayOneShot(clip, .75f);
            }
            else if (pc.movementSpeed == 5)
            {
                AudioClip clip = walk[Random.Range(0, walk.Length)];
                asource.PlayOneShot(clip, .75f);
            }
            else if (pc.movementSpeed == 3)
            {
                AudioClip clip = walk[Random.Range(0, walk.Length)];
                asource.PlayOneShot(clip, .75f);
            }
        }
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(Sound());
    }
}
