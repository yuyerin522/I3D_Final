using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingSlider : MonoBehaviour
{
    public RectTransform sliderRect;       // �����̴�
    public RectTransform movingBoxRect;    // �����̴� �׸�
    public GameObject cookedChickenPrefab; // ������ �߰��
    public GameObject burntChickenPrefab;  // ź �߰��
    public Transform resultSpawnPoint;     // ����� ������ġ
    public GameObject uiPanel;             // �µ� ���߱� UI �г�

    private bool isMovingRight = true;     // �׸� �̵� ����
    private bool isStopped = false;        // �׸� ������� Ȯ��
    private float moveSpeed = 300f;        // �׸� �̵� �ӵ�
    private float sliderMinX, sliderMaxX;  // �����̴��� �ּ�, �ִ� X ��ġ

    void Start()
    {
        // �����̴��� �¿� ���� ���
        sliderMinX = sliderRect.position.x - sliderRect.rect.width / 2f;
        sliderMaxX = sliderRect.position.x + sliderRect.rect.width / 2f;
    }

    void Update()
    {
        if (!isStopped)
        {
            MoveBox();
            if (Input.GetKeyDown(KeyCode.Space)) StopBox();
        }
    }

    void MoveBox()
    {
        // �̵� ���⿡ ���� �׸��� X ��ġ�� ����
        float moveStep = moveSpeed * Time.deltaTime * (isMovingRight ? 1 : -1);
        movingBoxRect.position += new Vector3(moveStep, 0, 0);

        // �׸� �����̴� ���� �����ϸ� ���� ����
        if (movingBoxRect.position.x >= sliderMaxX)
        {
            isMovingRight = false;
        }
        else if (movingBoxRect.position.x <= sliderMinX)
        {
            isMovingRight = true;
        }
    }

    void StopBox()
    {
        isStopped = true;

        // �׸��� ��ġ�� �������� ���� �µ� ���� �˻�
        float boxPositionRatio = Mathf.InverseLerp(sliderMinX, sliderMaxX, movingBoxRect.position.x);
        float temperature = Mathf.Lerp(100f, 300f, boxPositionRatio);

        GameObject resultPrefab;

        if (temperature >= 180f && temperature <= 200f)
        {
            resultPrefab = cookedChickenPrefab;
            Debug.Log("���� �µ�: ������ �߰��!");
        }
        else
        {
            resultPrefab = burntChickenPrefab;
            Debug.Log("������ �µ�: ź �߰��!");
        }

        Instantiate(resultPrefab, resultSpawnPoint.position, Quaternion.identity);

        // �κ��丮�� ������� �߰�
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            Sprite resultSprite = resultPrefab.GetComponent<SpriteRenderer>().sprite; // ��������Ʈ ����
            inventory.AddItemToSlot(resultSprite);
        }

        // UI �����
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
    }
}