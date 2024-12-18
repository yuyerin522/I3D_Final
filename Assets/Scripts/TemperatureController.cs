using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureController : MonoBehaviour
{
    public TextMesh currentTemText;         // �µ��� ǥ���ϴ� TextMesh
    public List<GameObject> wallsAndFloors; // ���� ���� �ٴ� ������Ʈ�� �����ϴ� ����Ʈ
    public Material iceMaterial;            // ���� �ؽ�ó
    public Material defaultMaterial;        // �⺻ �ؽ�ó

    private int temperature = 30;           // �ʱ� �µ� ��

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

        // �µ��� 0 ���Ϸ� ���� ���� �������� ����
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
        currentTemText.text = temperature.ToString(); // �µ��� �ؽ�Ʈ�� �ݿ�
        UpdateTextColor();
    }

    void UpdateTextColor()
    {
        if (temperature < 30)
        {
            currentTemText.color = Color.blue; // �µ� 30 ����: �Ķ���
        }
        else if (temperature >= 30 && temperature <= 36)
        {
            currentTemText.color = Color.yellow; // �µ� 30~36: �����
        }
        else
        {
            currentTemText.color = Color.red; // �µ� 36 �ʰ�: ������
        }
    }
}
