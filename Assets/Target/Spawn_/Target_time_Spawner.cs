using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_time_Spawner : MonoBehaviour
{
    public GameObject targetPrefab;        // 타겟 프리팹
    public int poolSize = 5;               // 생성할 타겟 수
    public float checkRadius = 0.5f;       // 위치 유효성 검사 반지름
    public LayerMask obstacleLayer;        // 장애물 레이어
    public Vector3 spawnMin = new Vector3(0f, 0.5f, -28f); // 랜덤 범위 시작점
    public Vector3 spawnMax = new Vector3(50f, 0.5f, -10f); // 랜덤 범위 끝점

    private Queue<Target_time> targetPool = new Queue<Target_time>();

    void Start()
    {
        // 오브젝트 풀 초기화
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(targetPrefab);
            obj.SetActive(false);
            Target_time target = obj.GetComponent<Target_time>();
            target.Initialize(this);
            targetPool.Enqueue(target);
        }

        // 초기 배치
        for (int i = 0; i < poolSize; i++)
        {
            SpawnFromPool();
        }
    }

    // Target_time에서 호출됨
    public void RespawnTarget(Target_time target)
    {
        StartCoroutine(RespawnAfterDelay(target, 1f)); // 1초 후 재활성화
    }

    private IEnumerator RespawnAfterDelay(Target_time target, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnFromPool();
    }

    private void SpawnFromPool()
    {
        if (targetPool.Count == 0) return;

        Target_time target = targetPool.Dequeue();
        Vector3 spawnPos = FindValidPosition();

        if (spawnPos != Vector3.zero)
        {
            target.transform.position = spawnPos;
            target.gameObject.SetActive(true);
            targetPool.Enqueue(target); // 다시 풀에 넣기
        }
    }

    // 유효한 위치 찾기 (장애물 없을 때만)
    private Vector3 FindValidPosition()
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(spawnMin.x, spawnMax.x),
                spawnMin.y,
                Random.Range(spawnMin.z, spawnMax.z)
            );

            if (!Physics.CheckSphere(pos, checkRadius, obstacleLayer))
            {
                return pos;
            }
        }

        Debug.LogWarning("유효한 스폰 위치를 찾지 못함.");
        return Vector3.zero;
    }
}
