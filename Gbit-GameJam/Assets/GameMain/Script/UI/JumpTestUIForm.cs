using GameFramework.DataTable;
using GameMain;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class JumpTestUIForm : UIFormLogic
{
    public TMP_Text QuestionText;
    public TMP_Text Answer1Text;
    public TMP_Text Answer2Text;
    public TMP_Text Answer3Text;

    public Button Answer1Button;
    public Button Answer2Button;
    public Button Answer3Button;

    public int CorrectAnswerIndex;

    public int increaseHPPerSecond = 5;
    public int punishHP = 10;

    private float timer = 0;

    private EntityPlayer player;

    private bool isClose;
    
    public RectTransform[] buttons; // 按钮数组
    public float moveInterval = 2.0f; // 按钮移动的时间间隔

    private Vector2[] targetPositions; // 每个按钮的目标位置
    public float moveSpeed = 100f; // 按钮移动速度


    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        
        isClose = false;

        //初始化题目
        IDataTable<DRTestHub> dtTestHub = GameEntry.DataTable.GetDataTable<DRTestHub>();

        //随机取一条题目
        int randomIndex = Random.Range(0, dtTestHub.Count);

        QuestionText.text = dtTestHub[randomIndex].TestName;
        Answer1Text.text = dtTestHub[randomIndex].Answer1;
        Answer2Text.text = dtTestHub[randomIndex].Answer2;
        Answer3Text.text = dtTestHub[randomIndex].Answer3;
        CorrectAnswerIndex = dtTestHub[randomIndex].Correct;

        Answer1Button.onClick.AddListener(OnAnswer1ButtonClick);
        Answer2Button.onClick.AddListener(OnAnswer2ButtonClick);
        Answer3Button.onClick.AddListener(OnAnswer3ButtonClick);

        // 初始化目标位置数组
        targetPositions = new Vector2[buttons.Length];
        // 为每个按钮生成初始目标位置
        for (int i = 0; i < buttons.Length; i++)
        {
            targetPositions[i] = GetRandomPosition(buttons[i]);
        }
        
        if (!GameEntry.Entity.HasEntity(EntityPlayer.PlayerId))
        {
            Log.Error("Player entity not exist!");
            return;
        }
        player = GameEntry.Entity.GetEntity(EntityPlayer.PlayerId).Logic as EntityPlayer;
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
        isClose = true;
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        timer += elapseSeconds;
        if (timer >= 1)
        {
            timer = 0;
            if (player != null)
            {
                player.AddHP(increaseHPPerSecond);
                timer = 0f;
            }
        }

        if (!GameEntry.Entity.HasEntity(EntityPlayer.PlayerId))
        {
            Log.Error("Player entity not exist!");
            GameEntry.UI.CloseUIForm(this);
        }
        
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == null) continue;

            // 平滑移动按钮到目标位置
            buttons[i].anchoredPosition = Vector2.MoveTowards(
                buttons[i].anchoredPosition,
                targetPositions[i],
                moveSpeed * elapseSeconds
            );

            // 如果按钮到达目标位置，生成新的随机目标位置
            if (Vector2.Distance(buttons[i].anchoredPosition, targetPositions[i]) < 1f)
            {
                targetPositions[i] = GetRandomPosition(buttons[i]);
            }
        }
    }

    private void OnAnswer1ButtonClick()
    {
        if (CorrectAnswerIndex == 1)
        {
            if (!isClose)
            {
                GameEntry.UI.CloseUIForm(this);
            }
        }
        else
        {
            if (player != null)
            {
                player.AddHP(punishHP);
            }
        }
    }

    private void OnAnswer2ButtonClick()
    {
        if (CorrectAnswerIndex == 2)
        {
            if (!isClose)
            {
                GameEntry.UI.CloseUIForm(this);
            }
        }
        else
        {
            if (player != null)
            {
                player.AddHP(punishHP);
            }
        }
    }

    private void OnAnswer3ButtonClick()
    {
        if (CorrectAnswerIndex == 3)
        {
            if (!isClose)
            {
                GameEntry.UI.CloseUIForm(this);
            }
        }
        else
        {
            if (player != null)
            {
                player.AddHP(punishHP);
            }
        }
    }
    
    
    // 获取屏幕范围内的随机位置
    private Vector2 GetRandomPosition(RectTransform button)
    {
        Vector2 buttonSize = button.sizeDelta;

        float x = Random.Range(0 + buttonSize.x / 2, 1080 - buttonSize.x / 2);
        float y = Random.Range(0 + buttonSize.y / 2, 607 - buttonSize.y / 2);

        return new Vector2(x, y);
    }
}