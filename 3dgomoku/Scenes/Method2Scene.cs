using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // SceneManager를 사용하기 위해 필요

public class Method2Scene : MonoBehaviour
{
    public void LoadNewScene()
    {
        // 새로운 씬을 불러옵니다. 씬의 이름을 "NewScene"이라고 가정했습니다.
        SceneManager.LoadScene("Method2");
    }
}