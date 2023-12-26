
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject cubePrefab;
    
    public int gridSize = 10;
    public int[,,] cubeGrid; // 3D array to store the state of each cube
    public int flag = 0;

    void SpawnCubes()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    cubeGrid[x, y, z] = 0;
                    Vector3 spawnPosition = new Vector3(x, y, z);
                    Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    public int[,,] GetCubeGrid()
    {
        return cubeGrid;
    }
   
    
    public enum PlayerTurn
    {
        Black,
        White
    }

    public PlayerTurn currentPlayer;

    void Awake()
    {
        cubeGrid = new int[gridSize, gridSize, gridSize];
        SpawnCubes();
        Instance = this;
        currentPlayer = PlayerTurn.Black;
    }

    public bool CheckForWin()
    {
        // ������ ���� �ڵ�: 3���� ť�갡 �������� ���õǾ��� �� �¸��� ����
        for (int x = 0; x < gridSize - 4; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    if (cubeGrid[x, y, z] != 0 &&
                        cubeGrid[x, y, z] == cubeGrid[x + 1, y, z] &&
                        cubeGrid[x, y, z] == cubeGrid[x + 2, y, z] &&
                        cubeGrid[x, y, z] == cubeGrid[x + 3, y, z] &&
                        cubeGrid[x, y, z] == cubeGrid[x + 4, y, z])
                    {
                        return true;
                    }
                }
            }
        }

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize - 4; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    if (cubeGrid[x, y, z] != 0 &&
                        cubeGrid[x, y, z] == cubeGrid[x, y + 1, z] &&
                        cubeGrid[x, y, z] == cubeGrid[x, y + 2, z] &&
                        cubeGrid[x, y, z] == cubeGrid[x, y + 3, z] &&
                        cubeGrid[x, y, z] == cubeGrid[x, y + 4, z])
                    {
                        return true;
                    }
                }
            }
        }

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize - 4; z++)
                {
                    if (cubeGrid[x, y, z] != 0 &&
                        cubeGrid[x, y, z] == cubeGrid[x, y, z + 1] &&
                        cubeGrid[x, y, z] == cubeGrid[x, y, z + 2] &&
                        cubeGrid[x, y, z] == cubeGrid[x, y, z + 3] &&
                        cubeGrid[x, y, z] == cubeGrid[x, y, z + 4])
                    {
                        return true;
                    }
                }
            }
        }

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    if(x<gridSize-4&&y<gridSize-4&&z<gridSize-4)
                    if (cubeGrid[x, y, z] != 0 &&
                       cubeGrid[x, y, z] == cubeGrid[x + 1, y + 1, z + 1] &&
                       cubeGrid[x, y, z] == cubeGrid[x + 2, y + 2, z + 2] &&
                       cubeGrid[x, y, z] == cubeGrid[x + 3, y + 3, z + 3] &&
                       cubeGrid[x, y, z] == cubeGrid[x + 4, y + 4, z + 4])
                    {
                        return true;
                    }
                    if(y<gridSize-4&&z< gridSize-4)
                    if (cubeGrid[x, y, z] != 0 &&
                        cubeGrid[x, y, z] == cubeGrid[x , y + 1, z + 1] &&
                        cubeGrid[x, y, z] == cubeGrid[x , y + 2, z + 2] &&
                        cubeGrid[x, y, z] == cubeGrid[x , y + 3, z + 3] &&
                        cubeGrid[x, y, z] == cubeGrid[x , y + 4, z + 4])
                    {
                        return true;
                    }
                    if (x < gridSize - 4 && z < gridSize - 4)
                        if (cubeGrid[x, y, z] != 0 &&
                       cubeGrid[x, y, z] == cubeGrid[x + 1, y, z + 1] &&
                       cubeGrid[x, y, z] == cubeGrid[x + 2, y, z + 2] &&
                       cubeGrid[x, y, z] == cubeGrid[x + 3, y, z + 3] &&
                       cubeGrid[x, y, z] == cubeGrid[x + 4, y, z + 4])
                    {
                        return true;
                    }
                    if (y < gridSize - 4 && x < gridSize - 4)
                        if (cubeGrid[x, y, z] != 0 &&
                       cubeGrid[x, y, z] == cubeGrid[x + 1, y+1, z ] &&
                       cubeGrid[x, y, z] == cubeGrid[x + 2, y+2, z ] &&
                       cubeGrid[x, y, z] == cubeGrid[x + 3, y+3, z ] &&
                       cubeGrid[x, y, z] == cubeGrid[x + 4, y+4, z ])
                    {
                        return true;
                    }
                }
            }
        }
      

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    if (y >= 4&&z<gridSize-4&&x<gridSize-4) { 
                    // x ���� �����ϰ� y ���� �����ϰ� z ���� �����ϴ� ���
                    if (cubeGrid[x, y, z] != 0 &&
                        cubeGrid[x, y, z] == cubeGrid[x + 1, y - 1, z + 1] &&
                        cubeGrid[x, y, z] == cubeGrid[x + 2, y - 2, z + 2] &&
                        cubeGrid[x, y, z] == cubeGrid[x + 3, y - 3, z + 3] &&
                        cubeGrid[x, y, z] == cubeGrid[x + 4, y - 4, z + 4])
                    {
                        return true;
                    }
                    }
                    if (y >= 4 && z >= 4&&x<gridSize-4)
                    {
                        // x ���� �����ϰ� y ���� �����ϰ� z ���� �����ϴ� ���
                        if (cubeGrid[x, y, z] != 0 &&
                        cubeGrid[x, y, z] == cubeGrid[x + 1, y - 1, z - 1] &&
                        cubeGrid[x, y, z] == cubeGrid[x + 2, y - 2, z - 2] &&
                        cubeGrid[x, y, z] == cubeGrid[x + 3, y - 3, z - 3] &&
                        cubeGrid[x, y, z] == cubeGrid[x + 4, y - 4, z - 4])
                        {
                            return true;
                        }
                    }
                    // x ���� �����ϰ� y ���� �����ϰ� z ���� �����ϴ� ���
                    if (y >= 4 && x >= 4&&z<gridSize-4)
                        if (cubeGrid[x, y, z] != 0 &&
                        cubeGrid[x, y, z] == cubeGrid[x - 1, y - 1, z + 1] &&
                        cubeGrid[x, y, z] == cubeGrid[x - 2, y - 2, z + 2] &&
                        cubeGrid[x, y, z] == cubeGrid[x - 3, y - 3, z + 3] &&
                        cubeGrid[x, y, z] == cubeGrid[x - 4, y - 4, z + 4])
                    {
                        return true;
                    }
                    if (x >= 4 && z < gridSize - 4&&y<gridSize-4)
                        if (cubeGrid[x, y, z] != 0 &&
                        cubeGrid[x, y, z] == cubeGrid[x - 1, y + 1, z + 1] &&
                        cubeGrid[x, y, z] == cubeGrid[x - 2, y + 2, z + 2] &&
                        cubeGrid[x, y, z] == cubeGrid[x - 3, y + 3, z + 3] &&
                        cubeGrid[x, y, z] == cubeGrid[x - 4, y + 4, z + 4])
                        {
                            return true;
                        }

                }
            }
        }
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize ; z++)
                {
                    if(x<gridSize-4 && y<gridSize-4)
                    if (cubeGrid[x, y, z] != 0 &&
                        cubeGrid[x, y, z] == cubeGrid[x + 1, y + 1, z ] &&
                        cubeGrid[x, y, z] == cubeGrid[x + 2, y + 2, z ] &&
                        cubeGrid[x, y, z] == cubeGrid[x + 3, y + 3, z ] &&
                        cubeGrid[x, y, z] == cubeGrid[x + 4, y + 4, z ])
                    {
                        return true;
                    }
                    if (x >= 4 && y < gridSize - 4)
                        if (cubeGrid[x, y, z] != 0 &&
                       cubeGrid[x, y, z] == cubeGrid[x - 1, y + 1, z] &&
                       cubeGrid[x, y, z] == cubeGrid[x - 2, y + 2, z] &&
                       cubeGrid[x, y, z] == cubeGrid[x - 3, y + 3, z] &&
                       cubeGrid[x, y, z] == cubeGrid[x - 4, y + 4, z])
                    {
                        return true;
                    }
                    if (y >= 4 && x < gridSize - 4)
                        if (cubeGrid[x, y, z] != 0 &&
                        cubeGrid[x, y, z] == cubeGrid[x + 1, y - 1, z] &&
                        cubeGrid[x, y, z] == cubeGrid[x + 2, y - 2, z] &&
                        cubeGrid[x, y, z] == cubeGrid[x + 3, y - 3, z] &&
                        cubeGrid[x, y, z] == cubeGrid[x + 4, y - 4, z])
                    {
                        return true;
                    }
                    if(y < gridSize -4&&z<gridSize-4)
                    if (cubeGrid[x, y, z] != 0 &&
                       cubeGrid[x, y, z] == cubeGrid[x , y + 1, z+1] &&
                       cubeGrid[x, y, z] == cubeGrid[x , y + 2, z+1] &&
                       cubeGrid[x, y, z] == cubeGrid[x , y + 3, z+1] &&
                       cubeGrid[x, y, z] == cubeGrid[x , y + 4, z+1])
                    {
                        return true;
                    }
                    if (z >= 4 && y < gridSize - 4)
                        if (cubeGrid[x, y, z] != 0 &&
                       cubeGrid[x, y, z] == cubeGrid[x , y + 1, z - 1] &&
                       cubeGrid[x, y, z] == cubeGrid[x , y + 2, z - 2] &&
                       cubeGrid[x, y, z] == cubeGrid[x, y + 3, z - 3] &&
                       cubeGrid[x, y, z] == cubeGrid[x, y + 4, z - 4])
                        {
                            return true;
                        }
                    if (y >= 4 && z < gridSize - 4)
                        if (cubeGrid[x, y, z] != 0 &&
                        cubeGrid[x, y, z] == cubeGrid[x, y - 1, z + 1] &&
                        cubeGrid[x, y, z] == cubeGrid[x, y - 2, z + 2] &&
                        cubeGrid[x, y, z] == cubeGrid[x, y - 3, z + 3] &&
                        cubeGrid[x, y, z] == cubeGrid[x , y - 4, z + 4])
                        {
                            return true;
                        }
                    if(x<gridSize-4&&z<gridSize-4)
                    if (cubeGrid[x, y, z] != 0 &&
                      cubeGrid[x, y, z] == cubeGrid[x+1, y , z + 1] &&
                      cubeGrid[x, y, z] == cubeGrid[x+2, y , z + 1] &&
                      cubeGrid[x, y, z] == cubeGrid[x+3, y , z + 1] &&
                      cubeGrid[x, y, z] == cubeGrid[x+4, y , z + 1])
                    {
                        return true;
                    }
                    if (z >= 4&& x < gridSize - 4)
                        if (cubeGrid[x, y, z] != 0 &&
                       cubeGrid[x, y, z] == cubeGrid[x+1, y , z - 1] &&
                       cubeGrid[x, y, z] == cubeGrid[x+2, y , z - 2] &&
                       cubeGrid[x, y, z] == cubeGrid[x+3, y , z - 3] &&
                       cubeGrid[x, y, z] == cubeGrid[x+4, y , z - 4])
                        {
                            return true;
                        }
                    if (x >= 4 && z < gridSize - 4)
                        if (cubeGrid[x, y, z] != 0 &&
                        cubeGrid[x, y, z] == cubeGrid[x-1, y , z + 1] &&
                        cubeGrid[x, y, z] == cubeGrid[x-2, y , z + 2] &&
                        cubeGrid[x, y, z] == cubeGrid[x-3, y , z + 3] &&
                        cubeGrid[x, y, z] == cubeGrid[x-4, y , z + 4])
                        {
                            return true;
                        }
                }
            }
        }
        return false;
    }
  
}
