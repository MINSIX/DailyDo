using UnityEngine;
using UnityEngine.UI;

public class EndingText : MonoBehaviour
{
   
    public Text turnText;


    // Update is called once per frame
    void Start()
    {
    
          if (GameManager.Instance.currentPlayer == GameManager.PlayerTurn.Black)
        {
            turnText.text = "Black is Winner!!!";
        }
        else
        {
            turnText.text = "White is Winner!!!";
        }
    }

    // Update the turn text based on the current player
    
}
