using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class RandomGenerator : MonoBehaviour
{
    [SerializeField] private TMP_InputField minInputField;
    [SerializeField] private TMP_InputField maxInputField;
    [SerializeField] private TMP_InputField exInputField;
    [SerializeField] private TMP_InputField volInputField;
    [SerializeField] private TextMeshProUGUI resultText;

    private const int UPPER_LIMIT = 9999;
    private const int LOWER_LIMIT = -9999;
    private const int MAX_VOLUME = 99;

    public void OnClick()
    {
        resultText.color = Color.white; // 先にデフォルトの色に戻す

        // 入力値チェック
        if (string.IsNullOrEmpty(minInputField.text) || string.IsNullOrEmpty(maxInputField.text))
        {
            DisplayError("Please enter a value");
            return;
        }


        if (!int.TryParse(minInputField.text, out int min) || !int.TryParse(maxInputField.text, out int max))
        {
            DisplayError("Please enter a value");
            return;
        }

        // 出力個数(空の場合は1とする)
        if (!int.TryParse(volInputField.text, out int volume) || volume < 1)
        {
            volume = 1;
        }

        // 値の範囲チェック
        if (min < LOWER_LIMIT || min > UPPER_LIMIT || max < LOWER_LIMIT || max > UPPER_LIMIT || volume > MAX_VOLUME)
        {
            DisplayError($"value range:\n{LOWER_LIMIT} <= min,max <= {UPPER_LIMIT} \n1 <= vol <= {MAX_VOLUME}");
            return;
        }
        
        if (min > max)
        {
            DisplayError("The maximum value must be greater than or equal to the minimum value.");
            return;
        }

        // 除外リストを生成
        var excludeSet = new HashSet<int>();
        string[] excludeParts = exInputField.text.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string part in excludeParts)
        {
            if (int.TryParse(part.Trim(), out int excludedNumber))
            {
                excludeSet.Add(excludedNumber);
            }
        }
        
        // ユニークな乱数を生成
        long availableCount = (long)max - min + 1 - excludeSet.Count(n => n >= min && n <= max);
        if (availableCount < volume)
        {
            DisplayError("Not enough unique numbers available to generate.");
            return;
        }

        var results = new List<int>();
        var generatedValues = new HashSet<int>();
        int attempts = 0;
        int maxAttempts = volume * 100;

        while (results.Count < volume && attempts < maxAttempts)
        {
            int randomValue = Random.Range(min, max + 1);
            if (!excludeSet.Contains(randomValue) && generatedValues.Add(randomValue))
            {
                results.Add(randomValue);
            }
            attempts++;
        }
        

        DisplayResults(results);
    }


    // 結果を表示
    private void DisplayResults(List<int> numbers)
    {
        if (numbers.Count == 0) return;

        numbers.Sort();

        if (numbers.Count == 1)
        {
            int num = numbers[0];
            if (num >= 1000 || num <= -999){
                resultText.fontSize = 72;
            }
            else{
                resultText.fontSize = 140;
            }
            resultText.text = num.ToString();
        }
        else
        {
            resultText.fontSize = 72;
            resultText.text = string.Join("\n", numbers);
        }
    }

    /// <summary>
    /// Displays an error message in red text.
    private void DisplayError(string message)
    {
        resultText.color = Color.red;
        
        // Adjust font size for specific error messages
        if (message.Contains("value range"))
        {
            resultText.fontSize = 20;
        }
        else
        {
            resultText.fontSize = 50;
        }
        resultText.text = message;
    }
}