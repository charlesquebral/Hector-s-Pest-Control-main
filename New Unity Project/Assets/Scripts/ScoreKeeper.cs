using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    public float numTot;
    public float numDead;
    public float finalDead;
    public float time = 0;
    public float maxTime = 180;
    public bool gameOver = false;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI accuracyText;
    public float shotsMade = 0;
    public float shotsTaken = 0;
    public float finalShotsMade;
    public float finalShotsTaken;
    public float perc;

    public GameObject gameOverScore;
    public float gradeAvg;
    public TextMeshProUGUI gameOverGrade;
    public Gradient col;
    public AI[] hectors;

    // Start is called before the first frame update
    void Start()
    {
        gameOverScore.SetActive(false);
        time = maxTime * 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver && time > 0)
        {
            RenderGameplay();
        }
        else
        {
            if (!gameOverScore.activeSelf)
            {
                RenderGameOver();
            }
        }
    }

    public void RenderGameplay()
    {
        time -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timeText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);

        if (numDead < numTot)
        {
            scoreText.text = "Hectors Killed: " + numDead + "/" + numTot;
        }

        if (shotsTaken <= 0)
        {
            perc = 0;
            accuracyText.text = "Accuracy: 0.00%";
        }
        else
        {
            perc = shotsMade / shotsTaken;
            accuracyText.text = "Accuracy: " + (perc * 100f).ToString("F2") + "%";
        }
    }

    public void RenderGameOver()
    {
        gameOver = true;
        finalDead = numDead;
        finalShotsMade = shotsMade;
        finalShotsTaken = shotsTaken;

        timeText.text = "Time: 00:00";

        scoreText.text = "Hectors Killed: " + finalDead + "/" + numTot;

        if (shotsTaken <= 0)
        {
            perc = 0;
            accuracyText.text = "Accuracy: 0.00%";
        }
        else
        {
            perc = finalShotsMade / finalShotsTaken;
            accuracyText.text = "Accuracy: " + (perc * 100f).ToString("F2") + "%";
        }

        gameOverScore.SetActive(true);
        float percDead = (finalDead / numTot);
        gradeAvg = ((perc + percDead) / 2);
        if (gradeAvg <= 1.00f && gradeAvg >= .97f)
        {
            gameOverGrade.text = "A+";
        }
        else if (gradeAvg < .97 && gradeAvg >= .93f)
        {
            gameOverGrade.text = "A";
        }
        else if (gradeAvg < .93 && gradeAvg >= .90f)
        {
            gameOverGrade.text = "A-";
        }
        else if (gradeAvg < .90 && gradeAvg >= .87f)
        {
            gameOverGrade.text = "B+";
        }
        else if (gradeAvg < .97 && gradeAvg >= .83f)
        {
            gameOverGrade.text = "B";
        }
        else if (gradeAvg < .83 && gradeAvg >= .80f)
        {
            gameOverGrade.text = "B-";
        }
        else if (gradeAvg < .80 && gradeAvg >= .77f)
        {
            gameOverGrade.text = "C+";
        }
        else if (gradeAvg < .77 && gradeAvg >= .73f)
        {
            gameOverGrade.text = "C";
        }
        else if (gradeAvg < .73 && gradeAvg >= .70f)
        {
            gameOverGrade.text = "C-";
        }
        else if (gradeAvg < .70 && gradeAvg >= .67f)
        {
            gameOverGrade.text = "D+";
        }
        else if (gradeAvg < .67 && gradeAvg >= .63f)
        {
            gameOverGrade.text = "D";
        }
        else if (gradeAvg < .63 && gradeAvg >= .60f)
        {
            gameOverGrade.text = "D-";
        }
        else
        {
            gameOverGrade.text = "F";
        }

        hectors = FindObjectsOfType<AI>();

        for (int i = 0; i < hectors.Length; i++)
        {
            if (hectors[i].agent.enabled)
            {
                hectors[i].isEnabled = false;
                hectors[i].agent.enabled = false;
                hectors[i].anim.SetBool("gameover", true);
            }
        }

        gameOverGrade.color = col.Evaluate(gradeAvg);
    }
}
