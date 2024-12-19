using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguineController : MonoBehaviour
{
    public float moveSpeed = 2f;        // 이동 속도
    public float rotationSpeed = 30f;   // 회전 속도

    private void Update()
    {
        // 앞으로 이동
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // 랜덤 회전 (좌우로 천천히 회전)
        float randomRotation = Random.Range(-rotationSpeed, rotationSpeed);
        transform.Rotate(Vector3.up, randomRotation * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // "Wall" 태그로 벽 감지
        {
            // 방향 반전
            transform.Rotate(Vector3.up, 180f);
        }
    }
}
