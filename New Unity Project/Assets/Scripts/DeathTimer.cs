using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    public float lifeTime = 2;
    public bool fromAwake = true;
    public GameObject shrinkRend;
    public bool shrink = false;
    public bool dying = false;

    // Start is called before the first frame update
    void Start()
    {
        if (fromAwake)
        {
            StartCoroutine(Die());
        }
    }

    void Update()
    {
        if (shrink && dying && shrinkRend != null)
        {
            shrinkRend.transform.localScale = Vector3.Lerp(shrinkRend.transform.localScale, Vector3.zero, lifeTime * Time.deltaTime);
        }
    }


    public void StartDeath()
    {
        dying = true;
        StartCoroutine(Die());
    }

    public IEnumerator Die()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
