using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterClickController : MonoBehaviour
{
    public Image crosshair;              // ȭ�� �߾� ũ�ν����

    public Inventory inventory;          // �κ��丮 ��ũ��Ʈ ����

    public Sprite chickenSprite;         // �߰�� ��������Ʈ
    public Sprite cookedChickenSprite;   // ���� �߰�
    public Sprite burntChickenSprite;    // ź �߰��

    public GameObject ovenGamePanel;     // ���� ���� UI �г�
    public CookingSlider cookingSlider;  // CookingSlider ��ũ��Ʈ ����

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
            // ȭ�� �߾ӿ��� Raycast ����
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, 100f)) // Ray�� ������Ʈ�� ��Ҵ��� Ȯ��
            {
                Debug.Log("Ŭ���� ������Ʈ: " + hit.collider.name);

                if (hit.collider.CompareTag("Chicken")) // ���߰�� Ŭ��
                {
                    if (chickenSprite != null) // chickenSprite�� �����Ǿ����� Ȯ��
                    {
                        inventory.AddItemToSlot(chickenSprite); // �κ��丮�� ���߰�� �߰�
                        Destroy(hit.collider.gameObject);       // Ŭ���� ���߰�� ����
                    }
                    else
                    {
                        Debug.LogWarning("chickenSprite�� �������� �ʾҽ��ϴ�!");
                    }
                }
                else if (hit.collider.CompareTag("Refrigerator")) // ����� �� ����/�ݱ�
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
                else if (hit.collider.CompareTag("Oven")) // ���� �� ����/�ݱ�
                {
                    Transform ovenDoor = hit.collider.transform.Find("OvenDoor");
                    if (ovenIsOpen)
                    {
                        CloseOvenDoor(ovenDoor.gameObject);
                    }
                    else
                    {
                        if (HasChickenInInventory()) // �߰�Ⱑ �κ��丮�� ���� ���� ���� ���� ����
                        {
                            OpenOvenDoor(ovenDoor.gameObject);
                            ShowOvenGamePanel(); // ���� �г� ǥ��
                            cookingSlider.StartCooking(); // ���� ���� ����

                            // �κ��丮���� ���߰�� ����
                            inventory.RemoveItemFromSlot(chickenSprite);
                        }
                    } 
                }
                else if (hit.collider.CompareTag("CookedChicken")) // ���� �߰�� Ŭ��
                {
                    // ���� �߰�� �Ǵ� ź �߰�⸦ �κ��丮�� �߰�
                    if (hit.collider.CompareTag("CookedChicken"))
                    {
                        inventory.AddItemToSlot(cookedChickenSprite);
                    }
                    else if (hit.collider.CompareTag("BurntChicken"))
                    {
                        inventory.AddItemToSlot(burntChickenSprite);
                    }

                    Destroy(hit.collider.gameObject); // Ŭ���� ���� ��� ����
                }
            }
        }
    }

    bool HasChickenInInventory()
    {
        // �κ��丮�� �߰�Ⱑ �ִ��� Ȯ���ϴ� �Լ� (���⼱ ������ ��������Ʈ�� �ִ��� üũ)
        return inventory.HasItem(chickenSprite);
    }

    void ShowOvenGamePanel()
    {
        if (ovenGamePanel != null)
        {
            ovenGamePanel.SetActive(true); // ���� �г� Ȱ��ȭ
        }
    }

    void HideOvenGamePanel()
    {
        if (ovenGamePanel != null)
        {
            ovenGamePanel.SetActive(false); // ���� �г� ��Ȱ��ȭ
        }
    }

    void OpenRefrigeratorDoor(GameObject refrigeratorDoor)
    {
        // ����� ���� ���� ���·� ȸ���ϵ��� ����
        refrigeratorDoor.transform.localRotation = Quaternion.Euler(0, -90, 0);
        refrigeratorIsOpen = true;
    }

    void CloseRefrigeratorDoor(GameObject refrigeratorDoor)
    {
        // ����� ���� �ݴ� ���·� ȸ��
        refrigeratorDoor.transform.localRotation = Quaternion.Euler(0, 0, 0);
        refrigeratorIsOpen = false;
    }

    void OpenOvenDoor(GameObject ovenDoor)
    {
        if (!ovenIsOpen) // ���� ���� ���� ���� �ʴٸ� ����
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