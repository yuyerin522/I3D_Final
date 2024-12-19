using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingSlider : MonoBehaviour
{
    public RectTransform sliderRect;       // 슬라이더
    public RectTransform movingBoxRect;    // 움직이는 네모
    public GameObject cookedChickenPrefab; // 구워진 닭고기
    public GameObject burntChickenPrefab;  // 탄 닭고기
    public Transform resultSpawnPoint;     // 결과물 생성위치
    public GameObject uiPanel;             // 온도 맞추기 UI 패널

    private bool isMovingRight = true;     // 네모 이동 방향
    private bool isStopped = false;        // 네모가 멈췄는지 확인
    private float moveSpeed = 300f;        // 네모 이동 속도
    private float sliderMinX, sliderMaxX;  // 슬라이더의 최소, 최대 X 위치

    private bool isGameStarted = false;    // 게임이 시작되었는지 여부

    public Inventory inventory;            // 인벤토리 스크립트 참조
    public Sprite chickenSprite;           // 생닭고기 스프라이트

    void Start()
    {
        // 슬라이더의 좌우 끝값 계산
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
        // 이동 방향에 따라 네모의 X 위치를 변경
        float moveStep = moveSpeed * Time.deltaTime * (isMovingRight ? 1 : -1);
        movingBoxRect.position += new Vector3(moveStep, 0, 0);

        // 네모가 슬라이더 끝에 도달하면 방향 반전
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

        // 네모의 위치를 기준으로 적합 온도 구간 검사
        float boxPositionRatio = Mathf.InverseLerp(sliderMinX, sliderMaxX, movingBoxRect.position.x);
        float temperature = Mathf.Lerp(100f, 300f, boxPositionRatio);

        // 온도가 180도 ~ 200도 범위 내에 있으면 구워진 닭고기
        if (temperature >= 180f && temperature <= 200f)
        {
            Instantiate(cookedChickenPrefab, resultSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            Instantiate(burntChickenPrefab, resultSpawnPoint.position, Quaternion.identity);
        }

        // UI 패널 숨기기
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
    }

    // 미니게임을 시작하는 함수
    public void StartCooking()
    {
        if (inventory.HasItem(chickenSprite))
        {
            inventory.RemoveItemFromSlot(chickenSprite); // 인벤토리에서 생닭고기 제거
        }
        isGameStarted = true;
        isStopped = false;
        movingBoxRect.gameObject.SetActive(true); // 슬라이더 시작
        uiPanel.SetActive(true); // UI 활성화

    }
}