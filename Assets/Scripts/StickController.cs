using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    public GameObject handStick;           // 플레이어 손에 나타날 막대기
    public GameObject[] itemPrefabs;       // 생성할 아이템 프리팹 배열
    public Transform[] spawnPoints;        // 아이템이 생성될 스폰 위치 배열
    public Inventory inventory;            // 인벤토리 클래스 참조
    public Sprite ballItemSprite;          // Ball 오브젝트에 대응하는 2D 스프라이트 이미지
    public Sprite collarItemSprite;        // Collar 오브젝트에 대응하는 2D 스프라이트 이미지

    private Camera playerCamera;           // 플레이어 카메라
    private bool isShaking = false;        // 흔들림 상태 확인
    private int clickCount = 0;            // 클릭 횟수

    void Start()
    {
        playerCamera = Camera.main; // 메인 카메라 참조
        if (handStick != null)
        {
            handStick.SetActive(false); // 처음엔 비활성화 상태
        }
    }

    void Update()
    {
        // 마우스 클릭 감지
        if (Input.GetMouseButtonDown(0)) // 왼쪽 클릭
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 레이캐스트로 오브젝트 클릭 체크
            if (Physics.Raycast(ray, out hit))
            {
                // 월드 막대기 클릭 시
                if (hit.collider.CompareTag("WorldStick"))
                {
                    Destroy(hit.collider.gameObject); // 월드 막대기 제거
                    ShowHandStick();                  // 손에 막대기 표시
                }
                // UnderSofa 클릭 시
                else if (hit.collider.CompareTag("UnderSofa") && handStick.activeSelf && !isShaking)
                {
                    StartCoroutine(ShakeStick());
                    HandleItemGeneration(); // 아이템 생성 로직 추가
                }
                // Ball 아이템 클릭 시
                else if (hit.collider.CompareTag("Ball"))
                {
                    // Ball 태그를 클릭했을 때, 인벤토리에 2D 스프라이트 추가
                    inventory.AddItemToSlot(ballItemSprite);  // 2D 스프라이트 추가
                    Destroy(hit.collider.gameObject);         // 클릭한 Ball 아이템 제거
                }
                // Collar 아이템 클릭 시
                else if (hit.collider.CompareTag("Collar"))
                {
                    // Collar 태그를 클릭했을 때, 인벤토리에 2D 스프라이트 추가
                    inventory.AddItemToSlot(collarItemSprite);  // 2D 스프라이트 추가
                    Destroy(hit.collider.gameObject);           // 클릭한 Collar 아이템 제거
                }
            }
        }
    }

    void ShowHandStick()
    {
        if (handStick != null)
        {
            handStick.SetActive(true); // 손에 있는 막대기 활성화
        }
    }

    IEnumerator ShakeStick()
    {
        isShaking = true; // 흔들림 상태 활성화

        float shakeAngle = 20.0f;        // 흔들릴 Z축 각도
        float shakeYRotation = -15.0f;  // 흔들릴 Y축 각도
        float shakeYOffset = -0.1f;     // Y축으로 내려가는 값
        float shakeDuration = 0.4f;     // 흔들림 지속 시간

        Vector3 originalRotation = handStick.transform.localEulerAngles; // 원래 각도 저장
        Vector3 originalPosition = handStick.transform.localPosition;    // 원래 위치 저장

        // 첫 번째 흔들림
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

        // 두 번째 흔들림
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

        // 원래 상태로 복원
        handStick.transform.localEulerAngles = originalRotation;
        handStick.transform.localPosition = originalPosition;

        isShaking = false; // 흔들림 상태 해제
    }

    void HandleItemGeneration()
    {
        clickCount++; // 클릭 횟수 증가

        if (clickCount % 3 == 0) // 클릭 횟수가 3의 배수일 때
        {
            GenerateRandomItem(); // 랜덤 아이템 생성
        }
    }

    void GenerateRandomItem()
    {
        if (itemPrefabs.Length > 0 && spawnPoints.Length > 0)
        {
            // 랜덤한 아이템과 스폰 위치 선택
            int randomItemIndex = Random.Range(0, itemPrefabs.Length);
            int randomSpawnIndex = Random.Range(0, spawnPoints.Length);

            GameObject randomItem = itemPrefabs[randomItemIndex];
            Transform randomSpawnPoint = spawnPoints[randomSpawnIndex];

            // 랜덤 회전값 생성
            Quaternion randomRotation = Random.rotation;

            // 아이템 생성
            Instantiate(randomItem, randomSpawnPoint.position, randomRotation);
        }
    }
}