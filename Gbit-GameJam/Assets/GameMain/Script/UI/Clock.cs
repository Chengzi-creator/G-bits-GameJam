using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityGameFramework.Runtime;

public class Clock : MonoBehaviour
{

    public int m_Minute;
    public int m_Second;
    public TMP_Text Text;

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShowTime();
    }

    public void ShowTime()
    {   
        VarInt32 second = GameEntry.DataNode.GetNode("UI").GetData<VarInt32>();
        m_Minute = (int)(second / 60f);
        m_Second = (int)(second - 60 * m_Minute);
        if (second >= 60f && second < 600f)
        {
            Text.text = $"0{m_Minute}:{m_Second}";
        }
        else if (second < 60f && second >= 10f)
        {
            Text.text = $"00:{m_Second}";
        }
    }
}
