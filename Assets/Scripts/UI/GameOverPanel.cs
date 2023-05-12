using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public GameObject Board;
    public TextMeshProUGUI WinnerText;

    public void SetWinnerText(PawnColor winnerPawnColor)
    {
        WinnerText.text = winnerPawnColor.ToString().ToUpper() + " WINS";
        WinnerText.color = winnerPawnColor == PawnColor.White ? Color.white : Color.black;
    }

    public void DisableBoard()
    {
        Board.SetActive(false);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}