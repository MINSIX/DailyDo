using UnityEngine;
using UnityEngine.UI;

public class TurnText : MonoBehaviour
{
   
    public Text turnText;


    // Update is called once per frame
    void Update()
    {
    
        UpdateTurnText();
    }

    // Update the turn text based on the current player
    public void UpdateTurnText()
    {
        if (GameManager.Instance.currentPlayer == GameManager.PlayerTurn.Black)
        {
           turnText.text = "Now Player: Black";
        }
        else
        {
           turnText.text = "Now Player: White";
        }
    }
}
