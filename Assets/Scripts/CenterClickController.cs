using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterClickController : MonoBehaviour
{
    public Image crosshair;              // 화면 중앙 크로스헤어
    public ClickHandler clickHandler;    // ClickHandler 스크립트 참조

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
        if (Input.GetMouseButtonDown(0)) // 마우스 클릭 시
        {
            // 화면 중앙에서 Raycast 실행
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, 100f)) // Ray가 오브젝트에 닿았는지 확인
            {
                Debug.Log("클릭된 오브젝트: " + hit.collider.name);

                clickHandler.HandleClick(hit.collider);
            }
            else
            {
                Debug.Log("NO");
            }
        }
    }
}