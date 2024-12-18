using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureController : MonoBehaviour
{
    public TextMesh currentTemText;         // 온도표시
    public List<GameObject> wallsAndFloors; // 벽이랑 바닥
    public Material iceMaterial;            // 얼음 텍스처
    public Material defaultMaterial;        // 기본 텍스처
    public GameObject penguinPrefab;        // 펭귄

    private int temperature = 27;           // 초기 온도 값
    private bool penguinsSpawned = false;   // 펭귄 생성 여부
    private List<GameObject> spawnedPenguins = new List<GameObject>(); 

    void Start()
    {
        UpdateTemperatureText(); // 초기 텍스트 업데이트
    }

    void Update()
    {
        // 마우스 클릭 확인
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("UP"))
                {
                    ChangeTemperature(3); // UP 하면 온도 3씩 증가
                }
                else if (hit.collider.CompareTag("DOWN"))
                {
                    ChangeTemperature(-3); // DOWN 하면 온도 3씩 감소
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
            // 온도가 0 이하로 가면 집이 얼음으로 변함
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
            currentTemText.color = Color.blue;              // 온도 30 이하: 파란색
        }
        else if (temperature >= 30 && temperature <= 36)
        {
            currentTemText.color = Color.yellow;            // 온도 30~36: 노란색
        }
        else
        {
            currentTemText.color = Color.red;               // 온도 36 초과: 빨간색
        }
    }

    void SpawnPenguins()
    {
        for (int i = 0; i < 10; i++)
        {
            // 랜덤 위치 계산
            float x = Random.Range(10f, -10f);           
            float z = Random.Range(-5f, 15f);            
            Vector3 spawnPosition = new Vector3(x, 0f, z);

            // 랜덤 회전값 계산
            float randomYRotation = Random.Range(0f, 360f);
            Quaternion spawnRotation = Quaternion.Euler(0f, randomYRotation, 0f);

            // 펭귄 프리팹 생성 및 리스트에 추가
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
