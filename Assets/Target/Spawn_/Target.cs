using UnityEngine;

public class Target : MonoBehaviour
{
    private TargetSpawner spawner;

    // 스포너와 연결
    public void Initialize(TargetSpawner targetSpawner)
    {
        spawner = targetSpawner;
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

