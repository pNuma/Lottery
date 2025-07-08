using UnityEngine;

public class NailPlacer : MonoBehaviour
{
    public GameObject nailPrefab;
    public int circles = 5; // 同心円の数
    public int nailsOnFirstCircle = 6; // 最初の円の釘の数
    public int nailIncrementPerCircle = 3; // 円ごとに増える釘の数
    public float startRadius = 1.0f; // 最初の円の半径
    public float radiusStep = 0.8f; // 半径の増加量
    public float positionRandomness = 0.1f; // 位置のランダム性

    [ContextMenu("Generate Nails")]
    void GenerateNails()
    {
        // 事前に生成した釘があれば全て削除してリセットする
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        // 釘プレハブが設定されていなければ何もしない
        if (nailPrefab == null)
        {
            Debug.LogError("Nail Prefab is not set!");
            return;
        }

        // 同心円状に釘を配置する
        for (int i = 0; i < circles; i++)
        {
            float currentRadius = startRadius + (i * radiusStep);
            int nailsInThisCircle = nailsOnFirstCircle + (i * nailIncrementPerCircle);

            for (int j = 0; j < nailsInThisCircle; j++)
            {
                // 円周上の角度を計算
                float angle = j * (360f / nailsInThisCircle);
                float angleRad = angle * Mathf.Deg2Rad; 

                // 基本的なX, Y座標を計算
                float x = Mathf.Cos(angleRad) * currentRadius;
                float y = Mathf.Sin(angleRad) * currentRadius;

                // ランダムなズレを加える
                float randomX = Random.Range(-positionRandomness, positionRandomness);
                float randomY = Random.Range(-positionRandomness, positionRandomness);

                Vector3 nailPosition = new Vector3(x + randomX, y + randomY, 0);

                // プレハブを生成し、このオブジェクトの子にする
                GameObject newNail = Instantiate(nailPrefab, transform.position + nailPosition, Quaternion.identity);
                newNail.transform.parent = this.transform;
            }
        }
    }
}