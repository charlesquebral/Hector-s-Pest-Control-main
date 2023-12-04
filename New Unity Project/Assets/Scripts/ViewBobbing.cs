using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PositionFollower))]
public class ViewBobbing : MonoBehaviour
{
    public float effectIntensity;
    public float effectIntensityx;
    public float effectSpeed;

    private PositionFollower followerInstance;
    private Vector3 origOffset;
    private float SinTime;

    public PlayerControl pc;

    public float[] _intense;
    public float[] _intensex;
    public float[] _effectspeed;

    void Start()
    {
        followerInstance = GetComponent<PositionFollower>();
        origOffset = followerInstance.offset;
    }


    void Update()
    {
        Vector3 inputVector = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));
        if (inputVector.magnitude > 0f)
        {
            SinTime += Time.deltaTime * effectSpeed;
        }
        else
        {
            SinTime = 0f;
        }

        float sinAmountY = -Mathf.Abs(effectIntensity * Mathf.Sin(SinTime));
        Vector3 sinAmountX = followerInstance.transform.right * effectIntensity * Mathf.Cos(SinTime) * effectIntensityx;

        if (pc.characterController.isGrounded)
        {
            followerInstance.offset = new Vector3
            {
                x = origOffset.x,
                y = origOffset.y + sinAmountY,
                z = origOffset.z
            };

            followerInstance.offset += sinAmountX;
        }
        else
        {
            followerInstance.offset = Vector3.zero;
        }

        if (!pc.crouch && !pc.prone && !pc.legs.GetBool("run"))
        {
            effectIntensity = _intense[0];
            effectIntensityx = _intensex[0];
            effectSpeed = _effectspeed[0];
        }
        else if (!pc.crouch && !pc.prone && pc.legs.GetBool("run"))
        {
            effectIntensity = _intense[1];
            effectIntensityx = _intensex[1];
            effectSpeed = _effectspeed[1];
        }
        else if (pc.crouch && !pc.prone)
        {
            effectIntensity = _intense[2];
            effectIntensityx = _intensex[2];
            effectSpeed = _effectspeed[2];
        }
        else if (!pc.crouch && pc.prone)
        {
            effectIntensity = _intense[3];
            effectIntensityx = _intensex[3];
            effectSpeed = _effectspeed[3];
        }
    }
}
