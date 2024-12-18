using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    public Inventory inventory;           // 인벤토리 스크립트 참조
    public Sprite chickenSprite;          // 닭고기 스프라이트
    public Sprite cookedChickenSprite;    // 구운 닭고기 스프라이트
    public Sprite burntChickenSprite;     // 탄 닭고기 스프라이트
    public CookingSlider cookingSlider;   // CookingSlider 스크립트 참조
    public DoorController doorController; // DoorController 스크립트 참조


    public void HandleClick(Collider hitCollider)
    {
        switch (hitCollider.tag)
        {
            case "Chicken":
                HandleChickenClick(hitCollider);
                break;
            case "Refrigerator":
                HandleRefrigeratorDoorClick(hitCollider);
                break;
            case "Oven":
                HandleOvenDoorClick(hitCollider);
                break;
            case "CookedChicken":
            case "BurntChicken":
                HandleCookedChickenClick(hitCollider);
                break;
            default:
                Debug.LogWarning("Unknown tag: " + hitCollider.tag);
                break;
        }
    }

    // 생닭고기
    private void HandleChickenClick(Collider hitCollider)
    {
        if (chickenSprite != null)
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
        if (hitCollider.CompareTag("CookedChicken"))
        {
            inventory.AddItemToSlot(cookedChickenSprite); // 구운 닭고기 추가
        }
        else if (hitCollider.CompareTag("BurntChicken"))
        {
            inventory.AddItemToSlot(burntChickenSprite);  // 탄 닭고기 추가
        }

        Destroy(hitCollider.gameObject); // 클릭된 구운 고기 삭제
    }
}