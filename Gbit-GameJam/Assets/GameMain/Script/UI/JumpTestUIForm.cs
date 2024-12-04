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

    public int increaseHPPerSecond = 2;
    public int punishHP = 5;

    private float timer = 0;

    private EntityPlayer player;

    private bool isClose;

    public RectTransform[] buttons; // 按钮数组

    public RectTransform QuestionPanel; // 问题面板

    private Vector2[] targetPositions; // 每个按钮的目标位置
    public float moveSpeed = 100f; // 按钮移动速度

    public float questionShowTime = 0.5f;
    public float questionChangeTime = 0.5f;
    public Vector2 initialPosition; // 初始位置
    public Vector2 initialSize; // 初始大小
    public Vector2 targetPosition; // 目标位置（相对于屏幕左下角）
    public Vector2 targetSize; // 目标大小（宽度和高度）
    private float elapsedTime = 0f; // 已经过去的时间
    private bool isQuestionShow = false;
    private bool isQuestionInit = false;

    
    private float punishTimer = 0f;

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        isClose = false;
        isQuestionInit = false;
        isQuestionShow = false;

        //初始化题目
        IDataTable<DRTestHub> dtTestHub = GameEntry.DataTable.GetDataTable<DRTestHub>();

        //随机取一条题目
        //根据时间选取
        VarInt32 second = GameEntry.DataNode.GetNode("UI").GetData<VarInt32>();
        int minute = second / 60;
        int randomIndex;
        if (minute < 5)
        {
            randomIndex = Random.Range(0, 21);

        }
        else
        {
            randomIndex = Random.Range(21, 41);
        }

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

        QuestionPanel.anchoredPosition = initialPosition;
        QuestionPanel.sizeDelta = initialSize;
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

        if (!isQuestionShow)
        {
            elapsedTime += elapseSeconds;
            if (elapsedTime >= questionShowTime)
            {
                elapsedTime = questionShowTime;
                isQuestionShow = true;
                elapsedTime = 0f;
            }
        }
        else if (!isQuestionInit)
        {
            elapsedTime += elapseSeconds;
            if (elapsedTime >= questionChangeTime)
            {
                elapsedTime = questionChangeTime;
                isQuestionInit = true;
            }

            float t = Mathf.Clamp01(elapsedTime / questionChangeTime);

            // 缓慢移动到目标位置
            QuestionPanel.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, t);
            // 缓慢缩放到目标大小
            QuestionPanel.sizeDelta = Vector2.Lerp(initialSize, targetSize, t);
        }

        punishTimer -= elapseSeconds;
        punishTimer = Mathf.Max(0, punishTimer);
    }

    private void OnAnswer1ButtonClick()
    {
        if(punishTimer > 0)
        {
            return;
        }
        
        if (CorrectAnswerIndex == 1)
        {
            GameEntry.Sound.PlaySound(AssetUtility.GetWAVAsset("correct"));
            if (!isClose)
            {
                GameEntry.UI.CloseUIForm(this);
            }
        }
        else
        {
            GameEntry.Sound.PlaySound(AssetUtility.GetWAVAsset("wrong"));
            if (player != null)
            {
                player.AddHP(punishHP);
                punishTimer = 0.5f;
            }
        }
    }

    private void OnAnswer2ButtonClick()
    {
        if(punishTimer > 0)
        {
            return;
        }
        if (CorrectAnswerIndex == 2)
        {
            GameEntry.Sound.PlaySound(AssetUtility.GetWAVAsset("correct"));
            if (!isClose)
            {
                GameEntry.UI.CloseUIForm(this);
            }
        }
        else
        {
            GameEntry.Sound.PlaySound(AssetUtility.GetWAVAsset("wrong"));

            if (player != null)
            {
                player.AddHP(punishHP);
                punishTimer = 0.5f;
            }
        }
    }

    private void OnAnswer3ButtonClick()
    {
        if(punishTimer > 0)
        {
            return;
        }
        
        if (CorrectAnswerIndex == 3)
        {
            GameEntry.Sound.PlaySound(AssetUtility.GetWAVAsset("correct"));

            if (!isClose)
            {
                GameEntry.UI.CloseUIForm(this);
            }
        }
        else
        {
            GameEntry.Sound.PlaySound(AssetUtility.GetWAVAsset("wrong"));

            if (player != null)
            {
                player.AddHP(punishHP);
                punishTimer = 0.5f;
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