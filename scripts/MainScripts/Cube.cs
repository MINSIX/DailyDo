
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cube : MonoBehaviour
{
    private bool isOccupied = false;
    private MeshRenderer meshRenderer;
    private bool isMouseOver = false;
    public AudioClip mySound;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        // Disable MeshRenderer for all child cubes
        EnableChildCubeRenderers();
    }

    // Disable MeshRenderer for all child cubes
    void DisableChildCubeRenderers()
    {
        foreach (Transform child in transform)
        {
            MeshRenderer childMeshRenderer = child.GetComponent<MeshRenderer>();
            if (childMeshRenderer != null)
            {
                childMeshRenderer.enabled = false;
            }
        }
    }

    // Enable MeshRenderer for all child cubes
    void EnableChildCubeRenderers()
    {
        foreach (Transform child in transform)
        {
            MeshRenderer childMeshRenderer = child.GetComponent<MeshRenderer>();
            if (childMeshRenderer != null)
            {
                childMeshRenderer.enabled = true;
            }
        }
    }

    public bool IsOccupied
    {
        get { return isOccupied; }
        set { isOccupied = value; }
    }

    void OnMouseEnter()
    {
        if (!isOccupied)
        {
            isMouseOver = true;
            meshRenderer.enabled = true;
            DisableChildCubeRenderers(); // Enable child cube renderers
            GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    void OnMouseExit()
    {
        if (!isOccupied)
        {
            meshRenderer.enabled = false;
            isMouseOver = false;
            EnableChildCubeRenderers(); // Disable child cube renderers
        }
    }

    void OnMouseDown()
    {
        if (!isOccupied && isMouseOver)
        {
            isOccupied = true;
            meshRenderer.enabled = true;
            GetComponent<Renderer>().material.color = (GameManager.Instance.currentPlayer == GameManager.PlayerTurn.Black) ? Color.black : Color.white;
            gameObject.tag = "RealBox";
            DisableChildCubeRenderers(); // Enable child cube renderers

            // Change the layer of the current cube
            gameObject.layer = LayerMask.NameToLayer("RealBox");

            // Change the tag and layer of all child cubes
            ChangeChildCubeTagsAndLayers("RealBox");
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.clip = mySound;
            audioSource.Play();
            int[,,] cubeGrid = GameManager.Instance.GetCubeGrid();

            // Get the position of the current cube in the cubeGrid
            Vector3Int cubePosition = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);

            // Update the cubeGrid at the corresponding position
            if(GameManager.Instance.currentPlayer == GameManager.PlayerTurn.Black)
            cubeGrid[cubePosition.x, cubePosition.y, cubePosition.z] = 1;
            else
                cubeGrid[cubePosition.x, cubePosition.y, cubePosition.z] = 2;

            // Change the tag of the current cube

            GameManager.Instance.currentPlayer = (GameManager.Instance.currentPlayer == GameManager.PlayerTurn.Black) ? GameManager.PlayerTurn.White : GameManager.PlayerTurn.Black;
          
            Wincheck();
            // Additional actions can be implemented here
           
        }
    }
    void Wincheck()
    {
        if (GameManager.Instance.CheckForWin())
        {
            if (GameManager.Instance.flag == 0)
            {
                GameManager.Instance.flag = 1;
                Debug.Log("Game Over! " + (GameManager.Instance.currentPlayer == GameManager.PlayerTurn.Black ? "Black" : "White") + " wins!");
                
            }

            else
            {
                // 게임 종료 시 어느 색이 이겼는지 콘솔에 출력
                
                SceneManager.LoadScene("Ending");
                gameObject.SetActive(false);

            }

        }
    }
    // Change the tag and layer of all child cubes recursively
    void ChangeChildCubeTagsAndLayers(string newTag)
    {
        int newLayer = LayerMask.NameToLayer(newTag);

        foreach (Transform child in transform)
        {
            child.tag = newTag;
            child.gameObject.layer = newLayer;

            ChangeChildCubeTagsAndLayersRecursive(child, newTag);
        }
    }

    // Change the tag and layer of all child cubes recursively
    void ChangeChildCubeTagsAndLayersRecursive(Transform parent, string newTag)
    {
        int newLayer = LayerMask.NameToLayer(newTag);

        foreach (Transform child in parent)
        {
            child.tag = newTag;
            child.gameObject.layer = newLayer;

            ChangeChildCubeTagsAndLayersRecursive(child, newTag);
        }
    }


    // Change the tag of all child cubes recursively
    void ChangeChildCubeTags(string newTag)
    {
        foreach (Transform child in transform)
        {
            child.tag = newTag;
            ChangeChildCubeTagsRecursive(child, newTag);
        }
    }

    // Change the tag of all child cubes recursively
    void ChangeChildCubeTagsRecursive(Transform parent, string newTag)
    {
        foreach (Transform child in parent)
        {
            child.tag = newTag;
            ChangeChildCubeTagsRecursive(child, newTag);
        }
    }

}
