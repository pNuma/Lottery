using UnityEngine;
using TMPro; 

public class Ball : MonoBehaviour
{
    public int number;
    private TextMeshPro textMeshPro;

    void Awake()
    {

        textMeshPro = GetComponentInChildren<TextMeshPro>();
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro component not found in children!");
        }
    }


    public void SetNumber(int num)
    {
        number = num;
        if (textMeshPro != null)
        {
            textMeshPro.text = number.ToString();
        }
    }
}