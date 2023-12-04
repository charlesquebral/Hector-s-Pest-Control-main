using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoHUD : MonoBehaviour
{
    public Color col1, col2;
    public Image[] shells;
    public Image[] shells2;
    public Gun gun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = shells.Length - 1; i >= 0; i--)
        {
            if (i < gun.ammo)
            {
                shells[i].color = col1;
                shells[i].transform.GetChild(0).GetComponent<Image>().color = col2;
            }
            else
            {
                shells[i].color = Color.grey;
                shells[i].transform.GetChild(0).GetComponent<Image>().color = Color.grey;
            }
        }
    }
}
