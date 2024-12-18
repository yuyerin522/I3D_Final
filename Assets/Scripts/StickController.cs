using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    public GameObject handStick;           // �÷��̾� �տ� ��Ÿ�� �����
    public GameObject[] itemPrefabs;       // ������ ������ ������ �迭
    public Transform[] spawnPoints;        // �������� ������ ���� ��ġ �迭
    public Inventory inventory;            // �κ��丮 Ŭ���� ����
    public Sprite ballItemSprite;          // Ball ������Ʈ�� �����ϴ� 2D ��������Ʈ �̹���
    public Sprite collarItemSprite;        // Collar ������Ʈ�� �����ϴ� 2D ��������Ʈ �̹���

    private Camera playerCamera;           // �÷��̾� ī�޶�
    private bool isShaking = false;        // ��鸲 ���� Ȯ��
    private int clickCount = 0;            // Ŭ�� Ƚ��

    void Start()
    {
        playerCamera = Camera.main; // ���� ī�޶� ����
        if (handStick != null)
        {
            handStick.SetActive(false); // ó���� ��Ȱ��ȭ ����
        }
    }

    void Update()
    {
        // ���콺 Ŭ�� ����
        if (Input.GetMouseButtonDown(0)) // ���� Ŭ��
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ����ĳ��Ʈ�� ������Ʈ Ŭ�� üũ
            if (Physics.Raycast(ray, out hit))
            {
                // ���� ����� Ŭ�� ��
                if (hit.collider.CompareTag("WorldStick"))
                {
                    Destroy(hit.collider.gameObject); // ���� ����� ����
                    ShowHandStick();                  // �տ� ����� ǥ��
                }
                // UnderSofa Ŭ�� ��
                else if (hit.collider.CompareTag("UnderSofa") && handStick.activeSelf && !isShaking)
                {
                    StartCoroutine(ShakeStick());
                    HandleItemGeneration(); // ������ ���� ���� �߰�
                }
                // Ball ������ Ŭ�� ��
                else if (hit.collider.CompareTag("Ball"))
                {
                    // Ball �±׸� Ŭ������ ��, �κ��丮�� 2D ��������Ʈ �߰�
                    inventory.AddItemToSlot(ballItemSprite);  // 2D ��������Ʈ �߰�
                    Destroy(hit.collider.gameObject);         // Ŭ���� Ball ������ ����
                }
                // Collar ������ Ŭ�� ��
                else if (hit.collider.CompareTag("Collar"))
                {
                    // Collar �±׸� Ŭ������ ��, �κ��丮�� 2D ��������Ʈ �߰�
                    inventory.AddItemToSlot(collarItemSprite);  // 2D ��������Ʈ �߰�
                    Destroy(hit.collider.gameObject);           // Ŭ���� Collar ������ ����
                }
            }
        }
    }

    void ShowHandStick()
    {
        if (handStick != null)
        {
            handStick.SetActive(true); // �տ� �ִ� ����� Ȱ��ȭ
        }
    }

    IEnumerator ShakeStick()
    {
        isShaking = true; // ��鸲 ���� Ȱ��ȭ

        float shakeAngle = 20.0f;        // ��鸱 Z�� ����
        float shakeYRotation = -15.0f;  // ��鸱 Y�� ����
        float shakeYOffset = -0.1f;     // Y������ �������� ��
        float shakeDuration = 0.4f;     // ��鸲 ���� �ð�

        Vector3 originalRotation = handStick.transform.localEulerAngles; // ���� ���� ����
        Vector3 originalPosition = handStick.transform.localPosition;    // ���� ��ġ ����

        // ù ��° ��鸲
        handStick.transform.localEulerAngles = new Vector3(
            originalRotation.x,
            originalRotation.y + shakeYRotation,
            originalRotation.z + shakeAngle
        );
        handStick.transform.localPosition = new Vector3(
            originalPosition.x,
            originalPosition.y + shakeYOffset,
            originalPosition.z
        );

        yield return new WaitForSeconds(shakeDuration / 2);

        // �� ��° ��鸲
        handStick.transform.localEulerAngles = new Vector3(
            originalRotation.x,
            originalRotation.y - shakeYRotation,
            originalRotation.z - shakeAngle
        );
        handStick.transform.localPosition = new Vector3(
            originalPosition.x,
            originalPosition.y + shakeYOffset,
            originalPosition.z
        );

        yield return new WaitForSeconds(shakeDuration / 2);

        // ���� ���·� ����
        handStick.transform.localEulerAngles = originalRotation;
        handStick.transform.localPosition = originalPosition;

        isShaking = false; // ��鸲 ���� ����
    }

    void HandleItemGeneration()
    {
        clickCount++; // Ŭ�� Ƚ�� ����

        if (clickCount % 3 == 0) // Ŭ�� Ƚ���� 3�� ����� ��
        {
            GenerateRandomItem(); // ���� ������ ����
        }
    }

    void GenerateRandomItem()
    {
        if (itemPrefabs.Length > 0 && spawnPoints.Length > 0)
        {
            // ������ �����۰� ���� ��ġ ����
            int randomItemIndex = Random.Range(0, itemPrefabs.Length);
            int randomSpawnIndex = Random.Range(0, spawnPoints.Length);

            GameObject randomItem = itemPrefabs[randomItemIndex];
            Transform randomSpawnPoint = spawnPoints[randomSpawnIndex];

            // ���� ȸ���� ����
            Quaternion randomRotation = Random.rotation;

            // ������ ����
            Instantiate(randomItem, randomSpawnPoint.position, randomRotation);
        }
    }
}