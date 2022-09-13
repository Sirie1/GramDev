using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ButtonLongHold : MonoBehaviour
{
    public int waintingTime;
    float timer;
    public GameObject statsBackground;
    public StatsDisplay statsDisplay;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Timer started");
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > waintingTime)
        {
            //Debug.Log("Timereached");
            statsBackground.SetActive(true);
            statsDisplay.gameObject.SetActive(true);
            timer = 0;
        }
    }
}
