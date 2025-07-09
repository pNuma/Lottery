using UnityEngine;

public class BallSpawner : MonoBehaviour
{
public GameObject ballPrefab;
public float spawnInterval = 1.0f;
private float timer = 0f;

void Update()
{
    timer += Time.deltaTime;
    if (Input.GetKeyDown(KeyCode.Space) && timer >= spawnInterval)
    {
        SpawnBall();
        timer = 0f;
    }
}

void SpawnBall()
{
    if (ballPrefab != null)
    {
        GameObject newBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        Ball ballScript = newBall.GetComponent<Ball>();

            int randomNumber = Random.Range(1, 100); // 例として1から99の乱数を生成
            ballScript.SetNumber(randomNumber);     
    }
}
}