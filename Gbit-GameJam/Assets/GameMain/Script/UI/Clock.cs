using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public float m_Timer;
    public int m_Minute;
    public int m_Second;
    public TMP_Text Text;

    void Awake()
    {
        m_Timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        m_Timer += Time.deltaTime;
        ShowTime();
    }

    public void ShowTime()
    {   
        m_Minute = (int)(m_Timer / 60f);
        m_Second = (int)(m_Timer - 60 * m_Minute);
        if (m_Timer >= 60f && m_Timer < 600f)
        {
            Text.text = $"0{m_Minute}:{m_Second}";
        }
        else if (m_Timer < 60f)
        {
            Text.text = $"00:{m_Second}";
        }
    }
}
