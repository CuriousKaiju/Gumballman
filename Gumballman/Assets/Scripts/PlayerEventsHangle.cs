using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerEventsHangle : MonoBehaviour
{
    private int _coinsValue;
    private int _HPValue = 3;
    private int _keyValue;

    [SerializeField] private GameObject _losePopUP;
    [SerializeField] private Text _coinValueForLevel;
    [SerializeField] private Image[] _helthGUI; // 0  1  2
    [SerializeField] private Sprite _HpHave;
    [SerializeField] private Sprite _HpNoHave;

    [SerializeField] private TextMeshProUGUI _playerScoreTextMesh;
    void Awake()
    {
        GameEvents.ScoreChangeEvent += OnScorePointChangeHandler;
        GameEvents.HPChangeEvent += OnHpChangeHandler;
        GameEvents.GameLose += OpenLosePopUP;
    }
    private void OnDestroy()
    {
        GameEvents.ScoreChangeEvent -= OnScorePointChangeHandler;
        GameEvents.HPChangeEvent -= OnHpChangeHandler;
        GameEvents.GameLose -= OpenLosePopUP;
    }
    private void OnScorePointChangeHandler(int score)
    {
        _coinsValue += score;
        _playerScoreTextMesh.SetText(_coinsValue.ToString());
    }
    private void OnHpChangeHandler(int hp)
    {
        _HPValue -= hp;
        int indexOfHeart = _HPValue;
        _helthGUI[indexOfHeart].sprite = _HpNoHave;
        if (_HPValue == 0)
        {
            GameEvents.CallPlayerLose();
        }
    }
    private void OpenLosePopUP()
    {
        _coinValueForLevel.text = "+" + _coinsValue;
        _losePopUP.SetActive(true);
    }
}
