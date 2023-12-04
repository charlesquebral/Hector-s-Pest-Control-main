using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    public float lifeTime = 2;
    public bool fromAwake = true;

    // Start is called before the first frame update
    void Start()
    {
        if (fromAwake)
        {
            StartCoroutine(Die());
        }
    }


    public void StartDeath()
    {
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
