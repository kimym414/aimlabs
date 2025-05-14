using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_time : MonoBehaviour
{
    private Target_time_Spawner spawner;
    public float lifeTime = 5f;  // 타겟의 생존 시간
    private float timer;

    public void Initialize(Target_time_Spawner spawner)
    {
        this.spawner = spawner;
    }

    private void OnEnable()
    {
        timer = lifeTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            gameObject.SetActive(false);
            spawner.RespawnTarget(this);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            gameObject.SetActive(false);         // 비활성화
            spawner.RespawnTarget(this);         // 스포너에 재생성 요청
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            // 아무 동작도 하지 않음
        }
    }
}
