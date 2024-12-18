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

    private bool isGameStarted = false;    // ������ ���۵Ǿ����� ����

    void Start()
    {
        // �����̴��� �¿� ���� ���
        sliderMinX = sliderRect.position.x - sliderRect.rect.width / 2f;
        sliderMaxX = sliderRect.position.x + sliderRect.rect.width / 2f;
    }

    void Update()
    {
        if (isGameStarted && !isStopped)
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

        // �µ��� 180�� ~ 200�� ���� ���� ������ ������ �߰��
        if (temperature >= 180f && temperature <= 200f)
        {
            Instantiate(cookedChickenPrefab, resultSpawnPoint.position, Quaternion.identity);
            Debug.Log("���� �µ�: ������ �߰��!");
        }
        else
        {
            Instantiate(burntChickenPrefab, resultSpawnPoint.position, Quaternion.identity);
            Debug.Log("������ �µ�: ź �߰��!");
        }

        // UI �г� �����
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
    }

    // �̴ϰ����� �����ϴ� �Լ�
    public void StartCooking()
    {
        isGameStarted = true;
        isStopped = false;
        movingBoxRect.gameObject.SetActive(true); // �����̴� ����
        uiPanel.SetActive(true); // UI Ȱ��ȭ
    }
}