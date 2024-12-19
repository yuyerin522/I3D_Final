using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    [Header("Script")]
    public Inventory inventory;           // 인벤토리 스크립트 참조
    public CookingSlider cookingSlider;   // CookingSlider 스크립트 참조
    public DoorController doorController; // DoorController 스크립트 참조
    public RecordPlayer recordPlayer;     // RecordPlayer 스크립트 참조
    public TemperatureController temperatureController; // TemperatureController 스크립트 참조

    [Header("Sprite")]
    public Sprite chickenSprite;          // 닭고기 스프라이트
    public Sprite cookedChickenSprite;    // 구운 닭고기 스프라이트
    public Sprite burntChickenSprite;     // 탄 닭고기 스프라이트
    public Sprite ballItemSprite;         // 축구공 스프라이트
    public Sprite collarItemSprite;       // 목줄 스프라이트

    [Header("Object")]
    public GameObject dogPrefab;          // 강아지 프리팹
    public GameObject handStick;          // 플레이어 손에 나타날 막대기
    public GameObject help;               // 도움말 UI
    public GameObject[] itemPrefabs;      // 생성할 아이템 프리팹 배열
    public GameObject door;               // 문

    [Header("SpawnPoint")]
    public Transform spawnPoint;          // 기본 강아지 생성 위치
    public Transform itemSpawnPoint;      // 축구공/목줄 클릭 시 강아지 생성 위치
    public Transform[] spawnPoints;       // 소파 밑 아이템 생성 스폰 위치 배열

    private bool isShaking = false;       // 막대기 흔들림 상태
    private int clickCount = 0;           // 클릭 횟수

    private void Start()
    {
        if (handStick != null) handStick.SetActive(false);
        StartCoroutine(ShowGameObjectForSeconds(10f));
    }

    public  void HandleClick(Collider hitCollider)
    {
        switch (hitCollider.tag)
        {
            case "Chicken": HandleChickenClick(hitCollider); break;
            case "Refrigerator": HandleRefrigeratorDoorClick(hitCollider); break;
            case "Oven": HandleOvenDoorClick(hitCollider); break;
            case "CookedChicken": case "BurntChicken": HandleCookedChickenClick(hitCollider); break;
            case "Bed": HandleBedClick(hitCollider); break;
            case "Dog": HandleDogClick(hitCollider); break;
            case "WorldStick": HandleWorldStickClick(hitCollider); break;
            case "UnderSofa": HandleUnderSofaClick(); break;
            case "Ball": HandleItemClick(hitCollider, ballItemSprite); break;
            case "Collar": HandleItemClick(hitCollider, collarItemSprite); break;
            default: Debug.Log("Unknown tag: " + hitCollider.tag); break;
        }
    }

    // 생닭고기
    private void HandleChickenClick(Collider hitCollider)
    {
        // 냉장고 문이 열려 있을 때만 닭고기 집을 수 있음
        if (chickenSprite != null && doorController.refrigeratorIsOpen)
        {
            inventory.AddItemToSlot(chickenSprite); // 인벤토리에 생닭고기 추가
            Destroy(hitCollider.gameObject);        // 클릭된 생닭고기 삭제
        }
    }

    // 냉장고 문 (열기/닫기)
    private void HandleRefrigeratorDoorClick(Collider hitCollider)
    {
        Transform refrigeratorDoor = hitCollider.transform.Find("RefrigeratorDoor");
        if (refrigeratorDoor != null && doorController != null)
        {
            if (doorController.refrigeratorIsOpen)
            {
                doorController.CloseRefrigeratorDoor(refrigeratorDoor.gameObject);
            }
            else
            {
                doorController.OpenRefrigeratorDoor(refrigeratorDoor.gameObject);
            }
        }
    }

    // 오븐 문 (열기/닫기)
    private void HandleOvenDoorClick(Collider hitCollider)
    {
        Transform ovenDoor = hitCollider.transform.Find("OvenDoor");
        if (ovenDoor != null && doorController != null)
        {
            if (doorController.ovenIsOpen)
            {
                doorController.CloseOvenDoor(ovenDoor.gameObject);
            }
            else
            {
                doorController.OpenOvenDoor(ovenDoor.gameObject);

                // 오븐 문을 열고 생고기가 있을 때, CookingSlider 미니게임 시작
                if (inventory.HasItem(chickenSprite))
                {
                    cookingSlider.inventory = inventory;
                    cookingSlider.chickenSprite = chickenSprite;
                    cookingSlider.StartCooking(); // 미니게임 시작
                }
            }
        }
    }

    // 구운 닭고기 (잘 구워진거/탄거)
    private void HandleCookedChickenClick(Collider hitCollider)
    {
        // 오븐 문이 열려 있을 때만 구운 닭고기를 집을 수 있음
        if (doorController.ovenIsOpen)
        {
            if (hitCollider.CompareTag("CookedChicken"))
            {
                inventory.AddItemToSlot(cookedChickenSprite);
            }
            else if (hitCollider.CompareTag("BurntChicken"))
            {
                inventory.AddItemToSlot(burntChickenSprite);
            }

            Destroy(hitCollider.gameObject);
        }
    }

    // 침대 클릭
    private void HandleBedClick(Collider hitCollider)
    {
        if (inventory.HasItem(cookedChickenSprite))
        {
            Instantiate(dogPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else if (inventory.HasItem(ballItemSprite) || inventory.HasItem(collarItemSprite))
        {
            Instantiate(dogPrefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
        }
    }

    // 강아지 클릭
    private void HandleDogClick(Collider hitCollider)
    {
            StartCoroutine(DogJumpAnimation(hitCollider.transform));
            inventory.RemoveItemFromSlot(cookedChickenSprite);
    }

    // 월드스틱
    private void HandleWorldStickClick(Collider hitCollider)
    {
        Destroy(hitCollider.gameObject);
        ShowHandStick();
    }

    //소파밑
    private void HandleUnderSofaClick()
    {
        if (handStick.activeSelf && !isShaking)
        {
            StartCoroutine(ShakeStick());
            HandleItemGeneration();
        }
    }

    //아이템
    private void HandleItemClick(Collider hitCollider, Sprite itemSprite)
    {
        inventory.AddItemToSlot(itemSprite);
        Destroy(hitCollider.gameObject);
    }

    //강아지 점프
    private IEnumerator DogJumpAnimation(Transform dogTransform)
    {
        Vector3 originalPosition = dogTransform.position;
        Vector3 jumpPosition = originalPosition + new Vector3(0, 2, 0);

        for (int i = 0; i < 2; i++)
        {
            float elapsedTime = 0;
            while (elapsedTime < 0.5f)
            {
                dogTransform.position = Vector3.Lerp(originalPosition, jumpPosition, elapsedTime / 0.5f);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0;
            while (elapsedTime < 0.5f)
            {
                dogTransform.position = Vector3.Lerp(jumpPosition, originalPosition, elapsedTime / 0.5f);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        dogTransform.position = originalPosition;
        door.transform.localRotation = Quaternion.Euler(0, -80, 0); // 문 회전
    }

    // 막대 휘적휘적
    private IEnumerator ShakeStick()
    {
        isShaking = true;

        Vector3 originalRotation = handStick.transform.localEulerAngles;
        Vector3 originalPosition = handStick.transform.localPosition;

        handStick.transform.localEulerAngles += new Vector3(0, -15, 20);
        handStick.transform.localPosition += new Vector3(0, -0.1f, 0);

        yield return new WaitForSeconds(0.2f);

        handStick.transform.localEulerAngles = originalRotation;
        handStick.transform.localPosition = originalPosition;

        isShaking = false;
    }

    // 아이템 랜덤생성
    private void HandleItemGeneration()
    {
        clickCount++;
        if (clickCount % 3 == 0) GenerateRandomItem();
    }

    // 아이템 랜덤생성
    private void GenerateRandomItem()
    {
        if (itemPrefabs.Length > 0 && spawnPoints.Length > 0)
        {
            int randomItemIndex = Random.Range(0, itemPrefabs.Length);
            int randomSpawnIndex = Random.Range(0, spawnPoints.Length);

            Instantiate(itemPrefabs[randomItemIndex], spawnPoints[randomSpawnIndex].position, Random.rotation);
        }
    }

    // 손 스틱
    private void ShowHandStick()
    {
        if (handStick != null) handStick.SetActive(true);
    }

    // 도움말 화면 코루틴
    private IEnumerator ShowGameObjectForSeconds(float seconds)
    {
        if (help != null)
        {
            help.SetActive(true);
            yield return new WaitForSeconds(seconds);
            help.SetActive(false);
        }
    }
}