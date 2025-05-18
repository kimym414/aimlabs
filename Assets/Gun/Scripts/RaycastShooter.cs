using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastShooter : MonoBehaviour
{
    public Camera mainCam;
    public float range = 100f;
    public LayerMask targetLayer;

    public GameObject bulletTrailPrefab; // TrailRenderer가 붙은 프리팹
    public Transform muzzleTransform;    // 총구 위치

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("발사");
            Shoot();
        }
    }

    void Shoot()
    {
        if (muzzleTransform == null)
        {
            Debug.LogWarning("muzzleTransform이 할당되지 않았습니다.");
            return;
        }

        // 카메라 중앙 방향 벡터
        Vector3 camCenterDir = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).direction;

        // 머즐 위치에서 카메라 정중앙 방향으로 레이 쏘기
        Ray ray = new Ray(muzzleTransform.position, camCenterDir);
        RaycastHit hit;

        Vector3 targetPoint = ray.origin + ray.direction * range;

        if (Physics.Raycast(ray, out hit, range, targetLayer))
        {
            targetPoint = hit.point;

            if (hit.collider.CompareTag("Target"))
            {
                Debug.Log("hit!");
                Destroy(hit.collider.gameObject);
            }
        }

        SpawnTrail(muzzleTransform.position, targetPoint);
    }

    void SpawnTrail(Vector3 start, Vector3 end)
    {
        GameObject trailObj = Instantiate(bulletTrailPrefab, start, Quaternion.identity);
        TrailRenderer trail = trailObj.GetComponent<TrailRenderer>();

        if (trail != null)
        {
            StartCoroutine(AnimateTrail(trail, start, end));
        }
    }

    System.Collections.IEnumerator AnimateTrail(TrailRenderer trail, Vector3 start, Vector3 end)
    {
        float time = 0;
        float duration = 0.05f;
        while (time < duration)
        {
            trail.transform.position = Vector3.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        trail.transform.position = end;

        Destroy(trail.gameObject, trail.time);
    }
}
