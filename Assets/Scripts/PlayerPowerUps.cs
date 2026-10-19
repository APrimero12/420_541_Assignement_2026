using System.Collections;
using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    private CharacterMovement movement;


    [Header("Active States (Debug)")]
    public bool hasSpeedBoost = false;
    public bool hasScoreBoost = false;
    public bool hasJumpBoost = false;

    public float SpeedBoostTimeRemaining { get; private set; } = 0f;
    public float ScoreBoostTimeRemaining { get; private set; } = 0f;
    public float JumpBoostTimeRemaining { get; private set; } = 0f;

    private void Start()
    {
        movement = GetComponent<CharacterMovement>();
        
    }

    public void ActivateSpeedBoost(float duration)
    {
        StartCoroutine(SpeedBoostRoutine(duration));
    }

    private IEnumerator SpeedBoostRoutine(float duration)
    {
        hasSpeedBoost = true;
        SpeedBoostTimeRemaining = duration;
        while (SpeedBoostTimeRemaining > 0)
        {
            SpeedBoostTimeRemaining -= Time.deltaTime;
            yield return null;
        }
        SpeedBoostTimeRemaining = 0f;
        hasSpeedBoost = false;
    }

    public void ActivateScoreBoost(float duration)
    {
        StartCoroutine(ScoreBoostRoutine(duration));
    }

    private IEnumerator ScoreBoostRoutine(float duration)
    {
        hasScoreBoost = true;
        ScoreBoostTimeRemaining = duration;
        if (ScoreManager.Instance != null) 
        {
            ScoreManager.Instance.SetDoubleScore(true);
        }
        while (ScoreBoostTimeRemaining > 0)
        {
            ScoreBoostTimeRemaining -= Time.deltaTime;
            yield return null;
        }
        ScoreBoostTimeRemaining = 0f;
        hasScoreBoost = false;

        if (ScoreManager.Instance != null) 
        {
            ScoreManager.Instance.SetDoubleScore(false);
        }
    }

    public void ActivateJumpBoost(float duration)
    {
        StartCoroutine(JumpBoostRoutine(duration));
    }

    private IEnumerator JumpBoostRoutine(float duration)
    {
       hasJumpBoost = true;
        JumpBoostTimeRemaining = duration;
        while (JumpBoostTimeRemaining > 0)
        {
            JumpBoostTimeRemaining -= Time.deltaTime;
            yield return null;
        }
        JumpBoostTimeRemaining = 0f;
        hasJumpBoost = false;
    }
}