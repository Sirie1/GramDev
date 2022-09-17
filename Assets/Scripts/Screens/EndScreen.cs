using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] TextMeshProUGUI heroUnlockText;

    void Start()
    {

    }
    private void OnEnable()
    {
        SetEndScreenText();
    }
    private void OnDisable()
    {
        heroUnlockText.gameObject.SetActive(false);
    }
    void SetEndScreenText()
    {
        if (GameManager.Instance.playerWonMatch)
            resultText.SetText("Match won");
        else
            resultText.SetText("Match lost");
        heroUnlockText.gameObject.SetActive(false);
    }
    public void SetEndScreenWonHero()
    {
        heroUnlockText.gameObject.SetActive(true);

    }

}
