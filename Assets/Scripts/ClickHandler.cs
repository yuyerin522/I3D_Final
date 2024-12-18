using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    public Inventory inventory;           // �κ��丮 ��ũ��Ʈ ����
    public Sprite chickenSprite;          // �߰�� ��������Ʈ
    public Sprite cookedChickenSprite;    // ���� �߰�� ��������Ʈ
    public Sprite burntChickenSprite;     // ź �߰�� ��������Ʈ
    public CookingSlider cookingSlider;   // CookingSlider ��ũ��Ʈ ����
    public DoorController doorController; // DoorController ��ũ��Ʈ ����


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

    // ���߰��
    private void HandleChickenClick(Collider hitCollider)
    {
        if (chickenSprite != null)
        {
            inventory.AddItemToSlot(chickenSprite); // �κ��丮�� ���߰�� �߰�
            Destroy(hitCollider.gameObject);        // Ŭ���� ���߰�� ����
        }
    }

    // ����� �� (����/�ݱ�)
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

    // ���� �� (����/�ݱ�)
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

                // ���� ���� ���� ����Ⱑ ���� ��, CookingSlider �̴ϰ��� ����
                if (inventory.HasItem(chickenSprite))
                {
                    cookingSlider.inventory = inventory;
                    cookingSlider.chickenSprite = chickenSprite;
                    cookingSlider.StartCooking(); // �̴ϰ��� ����
                }
            }
        }
    }

        // ���� �߰�� (�� ��������/ź��)
        private void HandleCookedChickenClick(Collider hitCollider)
        {
        if (hitCollider.CompareTag("CookedChicken"))
        {
            inventory.AddItemToSlot(cookedChickenSprite); // ���� �߰�� �߰�
        }
        else if (hitCollider.CompareTag("BurntChicken"))
        {
            inventory.AddItemToSlot(burntChickenSprite);  // ź �߰�� �߰�
        }

        Destroy(hitCollider.gameObject); // Ŭ���� ���� ��� ����
    }
}