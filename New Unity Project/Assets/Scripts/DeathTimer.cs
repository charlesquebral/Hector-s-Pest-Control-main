using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    public int lifeTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Die());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
