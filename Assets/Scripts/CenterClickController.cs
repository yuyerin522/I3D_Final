using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterClickController : MonoBehaviour
{
    public Inventory inventory;          // 인벤토리 스크립트 참조
    public Image crosshair;              // 화면 중앙의 크로스헤어 이미지
    public Sprite chickenSprite;         // 닭고기 스프라이트 참조

    private Camera mainCamera;
    private bool ovenIsOpen = false;     // 오븐 문이 열린 상태인지 확인하는 변수
    private bool refrigeratorIsOpen = false; // 냉장고 문이 열린 상태인지 확인하는 변수

    void Start()
    {
        mainCamera = Camera.main;

        // 화면 중앙에 크로스헤어 이미지 활성화
        if (crosshair != null)
        {
            crosshair.rectTransform.anchoredPosition = Vector2.zero; // 화면 중앙에 배치
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 클릭 감지
        {
            // 화면 중앙에서 Raycast 실행
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, 100f)) // Ray가 오브젝트에 닿았는지 확인
            {
                Debug.Log("클릭된 오브젝트: " + hit.collider.name);

                if (hit.collider.CompareTag("Chicken")) // 닭고기 클릭
                {
                    if (chickenSprite != null) // chickenSprite가 설정되었는지 확인
                    {
                        inventory.AddItemToSlot(chickenSprite); // 인벤토리에 아이템 추가
                        Destroy(hit.collider.gameObject);       // 클릭된 오브젝트 삭제
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
                    if (ovenDoor != null)
                    {
                        if (ovenIsOpen)
                        {
                            CloseOvenDoor(ovenDoor.gameObject);
                        }
                        else
                        {
                            OpenOvenDoor(ovenDoor.gameObject);
                        }
                    }
                }
            }
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