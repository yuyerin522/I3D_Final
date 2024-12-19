using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguineController : MonoBehaviour
{
    public float moveSpeed = 2f;        // �̵� �ӵ�
    public float rotationSpeed = 30f;   // ȸ�� �ӵ�

    private void Update()
    {
        // ������ �̵�
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // ���� ȸ�� (�¿�� õõ�� ȸ��)
        float randomRotation = Random.Range(-rotationSpeed, rotationSpeed);
        transform.Rotate(Vector3.up, randomRotation * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // "Wall" �±׷� �� ����
        {
            // ���� ����
            transform.Rotate(Vector3.up, 180f);
        }
    }
}
