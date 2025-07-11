using UnityEngine;

public class Hole : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            Ball ball = other.GetComponent<Ball>();
            RandomGenerator.Instance.ReportWinningNumber(ball.number);
            RandomGenerator.Instance.OnNumberFinished(ball.number);

            other.gameObject.SetActive(false);
            Destroy(other.gameObject, 2.0f);
        }
    }
}