using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Image[] inventorySlots;      // 인벤토리 슬롯 이미지 배열 (3칸 등)
    public Sprite emptySlotSprite;      // 빈 슬롯 이미지
    private int currentSlotIndex = 0;   // 현재 활성화될 슬롯의 인덱스

    void Start()
    {
        // 모든 슬롯을 빈 이미지로 초기화
        foreach (Image slot in inventorySlots)
        {
            slot.sprite = emptySlotSprite;
        }
    }

    // 아이템을 획득했을 때 호출되는 함수
    public void AddItemToSlot(Sprite itemSprite)
    {
        if (currentSlotIndex < inventorySlots.Length) // 슬롯이 남아있을 경우만
        {
            inventorySlots[currentSlotIndex].sprite = itemSprite; // 빈 슬롯을 아이템 이미지로 변경
            currentSlotIndex++; // 다음 슬롯으로 인덱스 이동
            Debug.Log("아이템 획득! 슬롯 채움: " + currentSlotIndex);
        }
        else
        {
            Debug.Log("슬롯이 모두 가득 찼습니다!");
        }
    }

    // 아이템을 제거하는 함수
    public void RemoveItemFromSlot(Sprite itemSprite)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].sprite == itemSprite)
            {
                inventorySlots[i].sprite = emptySlotSprite;
                currentSlotIndex--; // 슬롯 인덱스 감소
                break;
            }
        }
    }

    // 인벤토리 리셋 (슬롯 초기화)
    public void ResetInventory()
    {
        currentSlotIndex = 0;
        foreach (Image slot in inventorySlots)
        {
            slot.sprite = emptySlotSprite;
        }
    }

    // 인벤토리에 특정 아이템이 있는지 확인
    public bool HasItem(Sprite itemSprite)
    {
        foreach (Image slot in inventorySlots)
        {
            if (slot.sprite == itemSprite)
            {
                return true; 
            }
        }
        return false; 
    }
}