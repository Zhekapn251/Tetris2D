using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinGame : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private TMP_Text score;
    [SerializeField] private Fireworks fireworks;
    [SerializeField] private Image filledStarsAmount;
    
    private float _starsamount;

    
    private void Start()
    {
        EventsManager.OneWinMenuBtnClicked += HideWinGame;
    }
    public void ShowWinGame()
    {
        board.AllowStepping(false);
        gameObject.SetActive(true);
        score.text = board.score.ToString();
        GetWinnerRate();
        ShowStars();
        ClearWinnerRate();
    }

    private void ClearWinnerRate()
    {
        _starsamount = 0f;
    }

    private void ShowStars()
    {
        filledStarsAmount.fillAmount = _starsamount;
    }

    private void GetWinnerRate()
    {
        _starsamount = board.CalculateNumberOfStars();
    }
    public void HideWinGame()
    {
        gameObject.SetActive(false);
        fireworks.FireWorksDisable();
        board.StartGameRoutinesWithoutSaving();
        board.AllowStepping(true);
    }
    public void MenuOnWinGame()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        EventsManager.OneWinMenuBtnClicked -= HideWinGame;
    }

}
