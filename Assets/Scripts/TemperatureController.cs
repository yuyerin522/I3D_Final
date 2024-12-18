using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureController : MonoBehaviour
{
    public TextMesh currentTemText;         // 온도를 표시하는 TextMesh
    public List<GameObject> wallsAndFloors; // 여러 벽과 바닥 오브젝트를 관리하는 리스트
    public Material iceMaterial;            // 얼음 텍스처
    public Material defaultMaterial;        // 기본 텍스처

    private int temperature = 30;           // 초기 온도 값

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

        // 온도가 0 이하로 가면 집이 얼음으로 변함
        if (temperature <= 0)
        {
            foreach (GameObject obj in wallsAndFloors)
            {
                obj.GetComponent<Renderer>().material = iceMaterial;
            }
        }
        else
        {
            foreach (GameObject obj in wallsAndFloors)
            {
                obj.GetComponent<Renderer>().material = defaultMaterial;
            }
        }
    }

    void UpdateTemperatureText()
    {
        currentTemText.text = temperature.ToString(); // 온도를 텍스트에 반영
        UpdateTextColor();
    }

    void UpdateTextColor()
    {
        if (temperature < 30)
        {
            currentTemText.color = Color.blue; // 온도 30 이하: 파란색
        }
        else if (temperature >= 30 && temperature <= 36)
        {
            currentTemText.color = Color.yellow; // 온도 30~36: 노란색
        }
        else
        {
            currentTemText.color = Color.red; // 온도 36 초과: 빨간색
        }
    }
}
