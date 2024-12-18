using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureController : MonoBehaviour
{
    public TextMesh currentTemText;         // �µ�ǥ��
    public List<GameObject> wallsAndFloors; // ���̶� �ٴ�
    public Material iceMaterial;            // ���� �ؽ�ó
    public Material defaultMaterial;        // �⺻ �ؽ�ó
    public GameObject penguinPrefab;        // ���

    private int temperature = 27;           // �ʱ� �µ� ��
    private bool penguinsSpawned = false;   // ��� ���� ����
    private List<GameObject> spawnedPenguins = new List<GameObject>(); 

    void Start()
    {
        UpdateTemperatureText(); // �ʱ� �ؽ�Ʈ ������Ʈ
    }

    void Update()
    {
        // ���콺 Ŭ�� Ȯ��
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("UP"))
                {
                    ChangeTemperature(3); // UP �ϸ� �µ� 3�� ����
                }
                else if (hit.collider.CompareTag("DOWN"))
                {
                    ChangeTemperature(-3); // DOWN �ϸ� �µ� 3�� ����
                }
            }
        }
    }

    void ChangeTemperature(int delta)
    {
        temperature += delta;
        UpdateTemperatureText();

        if (temperature <= 0)
        {
            // �µ��� 0 ���Ϸ� ���� ���� �������� ����
            foreach (GameObject obj in wallsAndFloors)
            {
                obj.GetComponent<Renderer>().material = iceMaterial;
            }

            if (!penguinsSpawned)
            {
                SpawnPenguins();
                penguinsSpawned = true;
            }
        }
        else
        {
            foreach (GameObject obj in wallsAndFloors)
            {
                obj.GetComponent<Renderer>().material = defaultMaterial;
            }

            if (penguinsSpawned)
            {
                DestroyPenguins();
                penguinsSpawned = false;
            }
        }
    }

    void UpdateTemperatureText()
    {
        currentTemText.text = temperature.ToString();
        UpdateTextColor();
    }

    void UpdateTextColor()
    {
        if (temperature < 30)
        {
            currentTemText.color = Color.blue;              // �µ� 30 ����: �Ķ���
        }
        else if (temperature >= 30 && temperature <= 36)
        {
            currentTemText.color = Color.yellow;            // �µ� 30~36: �����
        }
        else
        {
            currentTemText.color = Color.red;               // �µ� 36 �ʰ�: ������
        }
    }

    void SpawnPenguins()
    {
        for (int i = 0; i < 10; i++)
        {
            // ���� ��ġ ���
            float x = Random.Range(10f, -10f);           
            float z = Random.Range(-5f, 15f);            
            Vector3 spawnPosition = new Vector3(x, 0f, z);

            // ���� ȸ���� ���
            float randomYRotation = Random.Range(0f, 360f);
            Quaternion spawnRotation = Quaternion.Euler(0f, randomYRotation, 0f);

            // ��� ������ ���� �� ����Ʈ�� �߰�
            GameObject penguin = Instantiate(penguinPrefab, spawnPosition, spawnRotation);
            spawnedPenguins.Add(penguin);
        }
    }

    void DestroyPenguins()
    {
        foreach (GameObject penguin in spawnedPenguins)
        {
            Destroy(penguin);
        }
        spawnedPenguins.Clear();
    }
}
