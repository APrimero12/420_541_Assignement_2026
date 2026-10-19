using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int Score { get; private set; }

    private bool isDoubleScoreActive = false; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void AddScore(int amount)
    {
        int finalAmount = isDoubleScoreActive ? amount * 2 : amount;

        Score += finalAmount;


        Debug.Log("Score: " + Score);
    }

    public void SetDoubleScore(bool isActive)
    {
        isDoubleScoreActive = isActive;
        Debug.Log("Double Score Status Changed: " + isActive);
    }

    public static int FinalScore { get; private set; }
 
    public void SaveFinalScore()
    {
        FinalScore = Score;
    }

}