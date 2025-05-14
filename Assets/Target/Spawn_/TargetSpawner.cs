using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public int poolSize = 3;
    public float spawnRange = 10f;
    public float checkRadius = 0.5f;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;

    private Queue<Target> targetPool = new Queue<Target>();

    void Start()
    {
        // 오브젝트 풀 초기화
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(targetPrefab);
            obj.SetActive(false);
            Target target = obj.GetComponent<Target>();
            target.Initialize(this);
            targetPool.Enqueue(target);
        }

        // 초기 타겟 배치
        for (int i = 0; i < poolSize; i++)
        {
            SpawnFromPool();
        }
    }

    // 타겟 재배치 요청
    public void RespawnTarget(Target target)
    {
        StartCoroutine(RespawnAfterDelay(target, 1f));
    }

    private IEnumerator RespawnAfterDelay(Target target, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnFromPool();
    }

    // 타겟을 랜덤 위치에 재배치
    private void SpawnFromPool()
    {
        if (targetPool.Count == 0) return;

        Target target = targetPool.Dequeue();
        Vector3 spawnPos = FindValidPosition();

        if (spawnPos != Vector3.zero)
        {
            target.transform.position = spawnPos;
            target.gameObject.SetActive(true);
            targetPool.Enqueue(target); // 재사용 대기열에 다시 넣기
        }
    }

    // 유효한 위치 찾기 (장애물 제외 + 바닥 Raycast)
    private Vector3 FindValidPosition()
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(0f, 50f),
                0.5f,
                Random.Range(-28f, -10f)
            );

            // 장애물 충돌 검사
            if (!Physics.CheckSphere(pos, checkRadius, obstacleLayer))
            {
                return pos;
            }
        }

        Debug.LogWarning("유효한 위치를 찾지 못했습니다.");
        return Vector3.zero;
    }

}
