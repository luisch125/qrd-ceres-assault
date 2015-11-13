using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Puntuaciones : MonoBehaviour {

    public Text puntuacion1;
    public Text puntuacion2;
    public Text puntuacion3;
    public Text puntuacion4;
    public Text puntuacion5;

    private int[] highScores = new int[5];
    string highScoreKey = "";

    void Start()
    {
        for (int i = 0; i < highScores.Length; i++)
        {
            highScoreKey = "Record " + (i + 1).ToString();
            highScores[i] = PlayerPrefs.GetInt(highScoreKey, 0);
            //use these values in whatever shows the leaderboard(s).
        }
        puntuacion1.text = "1. " + highScores[0];
        puntuacion2.text = "2. " + highScores[1];
        puntuacion3.text = "3. " + highScores[2];
        puntuacion4.text = "4. " + highScores[3];
        puntuacion5.text = "5. " + highScores[4];
    }
}