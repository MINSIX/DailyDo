using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // SceneManager�� ����ϱ� ���� �ʿ�

public class RuleScene : MonoBehaviour
{
    public void LoadNewScene()
    {
        // ���ο� ���� �ҷ��ɴϴ�. ���� �̸��� "NewScene"�̶�� �����߽��ϴ�.
        SceneManager.LoadScene("Method");
    }
}