using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterClickController : MonoBehaviour
{
    public Image crosshair;              // 화면 중앙 크로스헤어

    public Inventory inventory;          // 인벤토리 스크립트 참조

    public Sprite chickenSprite;         // 닭고기 스프라이트
    public Sprite cookedChickenSprite;   // 구운 닭고
    public Sprite burntChickenSprite;    // 탄 닭고기

    public GameObject ovenGamePanel;     // 오븐 게임 UI 패널
    public CookingSlider cookingSlider;  // CookingSlider 스크립트 참조

    private Camera mainCamera;
    private bool ovenIsOpen = false;  
    private bool refrigeratorIsOpen = false; 

    void Start()
    {
        mainCamera = Camera.main;

        if (crosshair != null)
        {
            crosshair.rectTransform.anchoredPosition = Vector2.zero; 
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 화면 중앙에서 Raycast 실행
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, 100f)) // Ray가 오브젝트에 닿았는지 확인
            {
                Debug.Log("클릭된 오브젝트: " + hit.collider.name);

                if (hit.collider.CompareTag("Chicken")) // 생닭고기 클릭
                {
                    if (chickenSprite != null) // chickenSprite가 설정되었는지 확인
                    {
                        inventory.AddItemToSlot(chickenSprite); // 인벤토리에 생닭고기 추가
                        Destroy(hit.collider.gameObject);       // 클릭된 생닭고기 삭제
                    }
                    else
                    {
                        Debug.LogWarning("chickenSprite가 설정되지 않았습니다!");
                    }
                }
                else if (hit.collider.CompareTag("Refrigerator")) // 냉장고 문 열기/닫기
                {
                    Transform refrigeratorDoor = hit.collider.transform.Find("RefrigeratorDoor");
                    if (refrigeratorDoor != null)
                    {
                        if (refrigeratorIsOpen)
                        {
                            CloseRefrigeratorDoor(refrigeratorDoor.gameObject);
                        }
                        else
                        {
                            OpenRefrigeratorDoor(refrigeratorDoor.gameObject);
                        }
                    }
                }
                else if (hit.collider.CompareTag("Oven")) // 오븐 문 열기/닫기
                {
                    Transform ovenDoor = hit.collider.transform.Find("OvenDoor");
                    if (ovenIsOpen)
                    {
                        CloseOvenDoor(ovenDoor.gameObject);
                    }
                    else
                    {
                        if (HasChickenInInventory()) // 닭고기가 인벤토리에 있을 때만 오븐 게임 시작
                        {
                            OpenOvenDoor(ovenDoor.gameObject);
                            ShowOvenGamePanel(); // 게임 패널 표시
                            cookingSlider.StartCooking(); // 오븐 게임 시작

                            // 인벤토리에서 생닭고기 제거
                            inventory.RemoveItemFromSlot(chickenSprite);
                        }
                    } 
                }
                else if (hit.collider.CompareTag("CookedChicken")) // 구운 닭고기 클릭
                {
                    // 구운 닭고기 또는 탄 닭고기를 인벤토리에 추가
                    if (hit.collider.CompareTag("CookedChicken"))
                    {
                        inventory.AddItemToSlot(cookedChickenSprite);
                    }
                    else if (hit.collider.CompareTag("BurntChicken"))
                    {
                        inventory.AddItemToSlot(burntChickenSprite);
                    }

                    Destroy(hit.collider.gameObject); // 클릭된 구운 고기 삭제
                }
            }
        }
    }

    bool HasChickenInInventory()
    {
        // 인벤토리에 닭고기가 있는지 확인하는 함수 (여기선 간단히 스프라이트가 있는지 체크)
        return inventory.HasItem(chickenSprite);
    }

    void ShowOvenGamePanel()
    {
        if (ovenGamePanel != null)
        {
            ovenGamePanel.SetActive(true); // 게임 패널 활성화
        }
    }

    void HideOvenGamePanel()
    {
        if (ovenGamePanel != null)
        {
            ovenGamePanel.SetActive(false); // 게임 패널 비활성화
        }
    }

    void OpenRefrigeratorDoor(GameObject refrigeratorDoor)
    {
        // 냉장고 문이 열린 상태로 회전하도록 설정
        refrigeratorDoor.transform.localRotation = Quaternion.Euler(0, -90, 0);
        refrigeratorIsOpen = true;
    }

    void CloseRefrigeratorDoor(GameObject refrigeratorDoor)
    {
        // 냉장고 문을 닫는 상태로 회전
        refrigeratorDoor.transform.localRotation = Quaternion.Euler(0, 0, 0);
        refrigeratorIsOpen = false;
    }

    void OpenOvenDoor(GameObject ovenDoor)
    {
        if (!ovenIsOpen) // 오븐 문이 열려 있지 않다면 열기
        {
            ovenDoor.transform.localRotation = Quaternion.Euler(90, 0, 0);
            ovenIsOpen = true;
        }
    }

    void CloseOvenDoor(GameObject ovenDoor)
    {
        ovenDoor.transform.localRotation = Quaternion.Euler(0, 0, 0);
        ovenIsOpen = false;
    }
}