using System.Collections;
using System.Collections.Generic;
// CubeSelector ��ũ��Ʈ
using UnityEngine;

public class Cubeselector : MonoBehaviour
{
    void Update()
    {
        // ���콺 �Է� �� ť�� ���� ���� ����
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Cube cube = hit.collider.GetComponent<Cube>();
                if (cube != null && !cube.IsOccupied)
                {
                    // ���õ� ť�� ���� ����
                    cube.GetComponent<Renderer>().material.color = Color.green;
                }
            }
        }
    }
}
