using UnityEngine;

public class Hole : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            // 入ったボールを消す
            Destroy(other.gameObject, 0.1f);
        }
    }
}