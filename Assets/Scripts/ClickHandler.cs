using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    [Header("Script")]
    public Inventory inventory;           // �κ��丮 ��ũ��Ʈ ����
    public CookingSlider cookingSlider;   // CookingSlider ��ũ��Ʈ ����
    public DoorController doorController; // DoorController ��ũ��Ʈ ����
    public RecordPlayer recordPlayer;     // RecordPlayer ��ũ��Ʈ ����
    public TemperatureController temperatureController; // TemperatureController ��ũ��Ʈ ����

    [Header("Sprite")]
    public Sprite chickenSprite;          // �߰�� ��������Ʈ
    public Sprite cookedChickenSprite;    // ���� �߰�� ��������Ʈ
    public Sprite burntChickenSprite;     // ź �߰�� ��������Ʈ
    public Sprite ballItemSprite;         // �౸�� ��������Ʈ
    public Sprite collarItemSprite;       // ���� ��������Ʈ

    [Header("Object")]
    public GameObject dogPrefab;          // ������ ������
    public GameObject handStick;          // �÷��̾� �տ� ��Ÿ�� �����
    public GameObject help;               // ���� UI
    public GameObject[] itemPrefabs;      // ������ ������ ������ �迭
    public GameObject door;               // ��

    [Header("SpawnPoint")]
    public Transform spawnPoint;          // �⺻ ������ ���� ��ġ
    public Transform itemSpawnPoint;      // �౸��/���� Ŭ�� �� ������ ���� ��ġ
    public Transform[] spawnPoints;       // ���� �� ������ ���� ���� ��ġ �迭

    private bool isShaking = false;       // ����� ��鸲 ����
    private int clickCount = 0;           // Ŭ�� Ƚ��

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

    // ���߰��
    private void HandleChickenClick(Collider hitCollider)
    {
        // ����� ���� ���� ���� ���� �߰�� ���� �� ����
        if (chickenSprite != null && doorController.refrigeratorIsOpen)
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
        // ���� ���� ���� ���� ���� ���� �߰�⸦ ���� �� ����
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

    // ħ�� Ŭ��
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

    // ������ Ŭ��
    private void HandleDogClick(Collider hitCollider)
    {
            StartCoroutine(DogJumpAnimation(hitCollider.transform));
            inventory.RemoveItemFromSlot(cookedChickenSprite);
    }

    // ���彺ƽ
    private void HandleWorldStickClick(Collider hitCollider)
    {
        Destroy(hitCollider.gameObject);
        ShowHandStick();
    }

    //���Ĺ�
    private void HandleUnderSofaClick()
    {
        if (handStick.activeSelf && !isShaking)
        {
            StartCoroutine(ShakeStick());
            HandleItemGeneration();
        }
    }

    //������
    private void HandleItemClick(Collider hitCollider, Sprite itemSprite)
    {
        inventory.AddItemToSlot(itemSprite);
        Destroy(hitCollider.gameObject);
    }

    //������ ����
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
        door.transform.localRotation = Quaternion.Euler(0, -80, 0); // �� ȸ��
    }

    // ���� ��������
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

    // ������ ��������
    private void HandleItemGeneration()
    {
        clickCount++;
        if (clickCount % 3 == 0) GenerateRandomItem();
    }

    // ������ ��������
    private void GenerateRandomItem()
    {
        if (itemPrefabs.Length > 0 && spawnPoints.Length > 0)
        {
            int randomItemIndex = Random.Range(0, itemPrefabs.Length);
            int randomSpawnIndex = Random.Range(0, spawnPoints.Length);

            Instantiate(itemPrefabs[randomItemIndex], spawnPoints[randomSpawnIndex].position, Random.rotation);
        }
    }

    // �� ��ƽ
    private void ShowHandStick()
    {
        if (handStick != null) handStick.SetActive(true);
    }

    // ���� ȭ�� �ڷ�ƾ
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