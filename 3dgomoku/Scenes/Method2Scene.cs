using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // SceneManager�� ����ϱ� ���� �ʿ�

public class Method2Scene : MonoBehaviour
{
    public void LoadNewScene()
    {
        // ���ο� ���� �ҷ��ɴϴ�. ���� �̸��� "NewScene"�̶�� �����߽��ϴ�.
        SceneManager.LoadScene("Method2");
    }
}