using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorPicker : MonoBehaviour
{
    public Color[] options;
    public Image colLeft, colMid, colRight;
    public int x = 0;
    public MainMenu mm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Right()
    {
        x--;
        if (x < 0)
        {
            x = options.Length - 1;
        }

        colMid.color = options[x];
        if (x == 0)
        {
            colLeft.color = options[options.Length - 1];
            colRight.color = options[x + 1];
        }
        else if (x >= options.Length - 1)
        {
            colLeft.color = options[x - 1];
            colRight.color = options[0];
        }
        else
        {
            colLeft.color = options[x - 1];
            colRight.color = options[x + 1];
        }

        mm.suit = options[x];
        mm.suitMat.color = options[x];
    }

    public void Left()
    {
        x++;
        if (x > options.Length - 1)
        {
            x = 0;
        }

        colMid.color = options[x];
        if (x == 0)
        {
            colLeft.color = options[options.Length - 1];
            colRight.color = options[x + 1];
        }
        else if (x >= options.Length - 1)
        {
            colLeft.color = options[x - 1];
            colRight.color = options[0];
        }
        else
        {
            colLeft.color = options[x - 1];
            colRight.color = options[x + 1];
        }

        mm.suit = options[x];
        mm.suitMat.color = options[x];
    }
}
