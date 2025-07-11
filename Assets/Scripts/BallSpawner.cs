using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public float spawnInterval = 0.2f;
    public float spawnPositionWidth = 2.0f;

    public void SpawnBall(int number)
    {
        if (ballPrefab == null) return;

        float randomX = Random.Range(-spawnPositionWidth / 2, spawnPositionWidth / 2);
        Vector3 spawnPosition = transform.position + new Vector3(randomX, 0, 0);
        GameObject newBallObject = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

        Ball ballScript = newBallObject.GetComponent<Ball>();
        ballScript.SetNumber(number);
        
    }
}