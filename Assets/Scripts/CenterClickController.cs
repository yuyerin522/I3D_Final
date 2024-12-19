using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterClickController : MonoBehaviour
{
    public Image crosshair;              // ȭ�� �߾� ũ�ν����
    public ClickHandler clickHandler;    // ClickHandler ��ũ��Ʈ ����

    private Camera mainCamera;

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
        if (Input.GetMouseButtonDown(0)) // ���콺 Ŭ�� ��
        {
            // ȭ�� �߾ӿ��� Raycast ����
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, 100f)) // Ray�� ������Ʈ�� ��Ҵ��� Ȯ��
            {
                Debug.Log("Ŭ���� ������Ʈ: " + hit.collider.name);

                clickHandler.HandleClick(hit.collider);
            }
            else
            {
                Debug.Log("NO");
            }
        }
    }
}