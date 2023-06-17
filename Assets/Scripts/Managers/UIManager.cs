using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject _acceptMoveUI;

    [SerializeField]
    TextMeshProUGUI _rollText;

    [SerializeField]
    GameObject _rollDiceButton;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnRolledDice.AddListener(ShowRollResult);
        GameManager.Instance.OnActivateRoute.AddListener(() => ShowAcceptMoveUI(false));
        GameManager.Instance.OnNewTurnStart.AddListener(OnNewTurnStart);
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
        _rollDiceButton.SetActive(false);
        _rollText.gameObject.SetActive(true);
        _rollText.text = result.ToString();
        _rollText.transform.DOPunchScale(Vector3.one * 1.3f, .3f);
        ShowAcceptMoveUI(true);
    }

    public void OnNewTurnStart()
    {
        _rollDiceButton.SetActive(true);
        _rollText.gameObject.SetActive(false);
    }
}
