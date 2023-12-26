using System.Collections;
using System.Collections.Generic;
// CubeSelector 스크립트
using UnityEngine;

public class Cubeselector : MonoBehaviour
{
    void Update()
    {
        // 마우스 입력 및 큐브 선택 로직 구현
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Cube cube = hit.collider.GetComponent<Cube>();
                if (cube != null && !cube.IsOccupied)
                {
                    // 선택된 큐브 색상 변경
                    cube.GetComponent<Renderer>().material.color = Color.green;
                }
            }
        }
    }
}
