using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Image[] inventorySlots;      // �κ��丮 ���� �̹��� �迭 (3ĭ ��)
    public Sprite emptySlotSprite;      // �� ���� �̹���
    private int currentSlotIndex = 0;   // ���� Ȱ��ȭ�� ������ �ε���

    void Start()
    {
        // ��� ������ �� �̹����� �ʱ�ȭ
        foreach (Image slot in inventorySlots)
        {
            slot.sprite = emptySlotSprite;
        }
    }

    // �������� ȹ������ �� ȣ��Ǵ� �Լ�
    public void AddItemToSlot(Sprite itemSprite)
    {
        if (currentSlotIndex < inventorySlots.Length) // ������ �������� ��츸
        {
            inventorySlots[currentSlotIndex].sprite = itemSprite; // �� ������ ������ �̹����� ����
            currentSlotIndex++; // ���� �������� �ε��� �̵�
            Debug.Log("������ ȹ��! ���� ä��: " + currentSlotIndex);
        }
        else
        {
            Debug.Log("������ ��� ���� á���ϴ�!");
        }
    }

    // �κ��丮 ���� �Լ� (���� �ʱ�ȭ)
    public void ResetInventory()
    {
        currentSlotIndex = 0;
        foreach (Image slot in inventorySlots)
        {
            slot.sprite = emptySlotSprite;
        }
    }
}

