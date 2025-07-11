using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    
    public float spawnInterval = 1.0f;
    
    public float spawnPositionWidth = 2.0f; 

    private float timer = 0f;

    void Update()
    {
        // テスト用
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBall();
        }
    }

    void SpawnBall()
    {

        float randomX = Random.Range(-spawnPositionWidth / 2, spawnPositionWidth / 2);
        Vector3 spawnPosition = transform.position + new Vector3(randomX, 0, 0);
        GameObject newBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        
        Ball ballScript = newBall.GetComponent<Ball>();

        int randomNumber = Random.Range(1, 100);
        ballScript.SetNumber(randomNumber);
        
    }
}