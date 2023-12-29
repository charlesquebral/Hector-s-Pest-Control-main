using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRecoil : MonoBehaviour
{
    Vector3 currentRotation, targetRotation, targetPosition, currentPosition, initialGunPosition;
    public Transform cam;

    public float recoilX;
    public float recoilY;
    public float recoilZ;

    public float kickBackZ;

    public float snappiness, returnAmount;

    // Start is called before the first frame update
    void Start()
    {
        initialGunPosition = transform.localPosition;    
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.fixedDeltaTime * snappiness);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(currentRotation), Time.deltaTime * 12);
            Back();
        }
    }

    public void Recoil()
    {
        targetPosition -= new Vector3(0, 0, kickBackZ);
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }

    public void Back()
    {
        targetPosition = Vector3.Lerp(targetPosition, initialGunPosition, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Lerp(currentRotation, targetPosition, Time.deltaTime * snappiness);
        transform.localPosition = currentPosition;
    }
}
