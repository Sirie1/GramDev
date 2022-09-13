using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    void Start()
    {
        SetEndScreenText();
    }
    void SetEndScreenText()
    {
        if (GameManager.Instance.playerWonMatch)
            resultText.SetText("Match won");
        else
            resultText.SetText("Match lost");
    }

}
