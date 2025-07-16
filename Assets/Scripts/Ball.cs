using UnityEngine;
using TMPro; 

public class Ball : MonoBehaviour
{
    public int number;
    private TextMeshPro textMeshPro;

    void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    public void SetNumber(int num)
    {
        number = num;
        
        if (textMeshPro != null)
        {
            textMeshPro.text = number.ToString();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DestroyZone"))
        {
            RandomGenerator.Instance.OnNumberFinished(number);
            Destroy(gameObject);
        }
    }
}