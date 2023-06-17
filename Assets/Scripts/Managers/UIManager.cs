using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject _acceptMoveUI;

    [SerializeField]
    TextMeshProUGUI _rollText;

    [SerializeField] 
    public List<Sprite> dices;
    
    [SerializeField]
    public Image diceImage;

    [SerializeField]
    GameObject _rollDiceButton;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnRolledDice.AddListener(ShowRollResult);
        GameManager.Instance.OnActivateRoute.AddListener(() => ShowAcceptMoveUI(false));
        GameManager.Instance.OnNewTurnStart.AddListener(OnNewTurnStart);
        GameManager.Instance.OnContinueMove.AddListener(() => ShowAcceptMoveUI(true));
        OnNewTurnStart();
        /*GameManager.Instance.OnNewTurnStart.AddListener()*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowAcceptMoveUI(bool state)
    {
        _acceptMoveUI.SetActive(state);
    }

    public void ShowRollResult(int result)
    {
        diceImage.sprite = dices[result - 1];
        ShowAcceptMoveUI(true);
    }

    public void OnNewTurnStart()
    {
        diceImage.sprite = null;
    }
}
