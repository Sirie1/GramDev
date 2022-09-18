using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] GameObject heroUnlockPopUp;
    private void OnEnable()
    {
        SetEndScreenText();
    }
    private void OnDisable()
    {
        heroUnlockPopUp.gameObject.SetActive(false);
    }
    void SetEndScreenText()
    {
        if (GameManager.Instance.PlayerWonMatch)
            resultText.SetText("Match won");
        else
            resultText.SetText("Match lost");
        if (GameManager.Instance.NewHero)
            heroUnlockPopUp.gameObject.SetActive(true);
    }

}
