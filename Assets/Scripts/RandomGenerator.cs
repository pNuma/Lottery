using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class RandomGenerator : MonoBehaviour
{
    public static RandomGenerator Instance { get; private set; }

    [SerializeField] private TMP_InputField minInputField;
    [SerializeField] private TMP_InputField maxInputField;
    [SerializeField] private TMP_InputField exInputField;
    [SerializeField] private TMP_InputField volInputField;
    [SerializeField] private TextMeshProUGUI resultText;

    [SerializeField] private BallSpawner ballSpawner;

    private const int UPPER_LIMIT = 9999;
    private const int LOWER_LIMIT = -9999;
    private const int MAX_VOLUME = 99;

    private HashSet<int> activeNumbers = new HashSet<int>();
    private List<int> winningNumbers = new List<int>();
    private int targetVolume;

    public bool isAccepting { get; private set; }

    private void Awake()
    {
        isAccepting = false;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void OnClick()
    {
        if (!ValidateInputs(out int min, out int max, out int volume, out var excludeSet))
        {
            return;
        }

        // ビジュアライズシーンの場合
        if (ballSpawner != null)
        {
            targetVolume = volume;
            winningNumbers.Clear();
            activeNumbers.Clear();
            DisplayMessage("Choosing...");

            StopAllCoroutines();
            StartCoroutine(GameLoop(min, max, excludeSet));
        }
        // UIのみのシーンの場合
        else
        {
            var results = GenerateUniqueNumbers(min, max, volume, excludeSet);
            if (results != null)
            {
                winningNumbers = results;
                DisplayWinningResults();
            }
        }

        isAccepting = true; 
    }
    
    // 当たり数に達するまでボールを生成
    private IEnumerator GameLoop(int min, int max, HashSet<int> excludeSet)
    {
        while (winningNumbers.Count < targetVolume)
        {
            int? numberToSpawn = FindAvailableNumber(min, max, excludeSet);

            if (numberToSpawn.HasValue)
            {
                int num = numberToSpawn.Value;
                activeNumbers.Add(num); 
                ballSpawner.SpawnBall(num);
            }
            
            yield return new WaitForSeconds(ballSpawner.spawnInterval);
        }

        // 設定した数に達したらボールの生成を停止
        if (winningNumbers.Count >= targetVolume)
        {
            isAccepting = false;
            StopAllCoroutines();
        }
    }

    private int? FindAvailableNumber(int min, int max, HashSet<int> excludeSet)
    {
        long availableCount = (long)max - min + 1 - excludeSet.Count(n => n >= min && n <= max) - activeNumbers.Count;
        if(availableCount <= 0)
        {
            return null; 
        }

        int maxAttempts = 200;
        for(int attempts = 0; attempts < maxAttempts; attempts++)
        {
            int candidate = Random.Range(min, max + 1);
            if (!excludeSet.Contains(candidate) && !activeNumbers.Contains(candidate))
            {
                return candidate; 
            }
        }

        return null; 
    }

    public void OnNumberFinished(int number)
    {
        activeNumbers.Remove(number);
    }
    
    public void ReportWinningNumber(int number)
    {
        if (!winningNumbers.Contains(number))
        {
            winningNumbers.Add(number);
            DisplayWinningResults();
        }
    }
    
    private void DisplayWinningResults()
    {
        if (winningNumbers == null || winningNumbers.Count == 0) return;

        resultText.color = Color.white;
        winningNumbers.Sort();

        if (winningNumbers.Count == 1)
        {
            int num = winningNumbers[0];
            if (num >= 1000 || num <= -999)
            {
                resultText.fontSize = 72;
            }
            else
            {
                resultText.fontSize = 140;
            }
            resultText.text = num.ToString();
        }
        else
        {
            resultText.fontSize = 72;
            resultText.text = string.Join("\n", winningNumbers);
        }
    }
    
    private void DisplayMessage(string message)
    {
        bool isError = message.Contains("Please") || message.Contains("value range") || message.Contains("Not enough");
        resultText.color = isError ? Color.red : Color.white;
        
        if (isError && message.Contains("value range"))
        {
            resultText.fontSize = 20;
        }
        else
        {
            resultText.fontSize = 50;
        }
        resultText.text = message;
    }
    
    private bool ValidateInputs(out int min, out int max, out int volume, out HashSet<int> excludeSet)
    {
        min = 0;
        max = 0;
        volume = 1;
        excludeSet = new HashSet<int>();

        if (string.IsNullOrEmpty(minInputField.text) || !int.TryParse(minInputField.text, out min) ||
            string.IsNullOrEmpty(maxInputField.text) || !int.TryParse(maxInputField.text, out max))
        {
            DisplayMessage("Please enter a valid number for Min and Max.");
            return false;
        }

        if (!string.IsNullOrEmpty(volInputField.text) && !int.TryParse(volInputField.text, out volume))
        {
            DisplayMessage("Please enter a valid number for Volume.");
            return false;
        }

        if (min < LOWER_LIMIT || max > UPPER_LIMIT || volume < 1 || volume > MAX_VOLUME)
        {
            DisplayMessage($"value range:\n{LOWER_LIMIT} <= min,max <= {UPPER_LIMIT} \n1 <= vol <= {MAX_VOLUME}");
            return false;
        }

        if (min > max)
        {
            DisplayMessage("Max value must be greater than or equal to Min value.");
            return false;
        }

        string[] excludeParts = exInputField.text.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string part in excludeParts)
        {
            if (int.TryParse(part.Trim(), out int excludedNumber))
            {
                excludeSet.Add(excludedNumber);
            }
        }
        
        return true;
    }
    
    // UIのみのシーン用
    private List<int> GenerateUniqueNumbers(int min, int max, int volume, HashSet<int> excludeSet)
    {
        long availableCount = (long)max - min + 1 - excludeSet.Count(n => n >= min && n <= max);
        if (availableCount < volume)
        {
            DisplayMessage("Not enough unique numbers available in the specified range.");
            return null;
        }

        var results = new List<int>();
        var generatedValues = new HashSet<int>();
        int maxAttempts = volume * 200;

        for (int i = 0; i < maxAttempts && results.Count < volume; i++)
        {
            int randomValue = Random.Range(min, max + 1);
            if (!excludeSet.Contains(randomValue) && generatedValues.Add(randomValue))
            {
                results.Add(randomValue);
            }
        }
        
        return results;
    }
}