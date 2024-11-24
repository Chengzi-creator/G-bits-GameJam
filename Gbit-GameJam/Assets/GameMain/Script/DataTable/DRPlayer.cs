using UnityGameFramework.Runtime;

public class DRPlayer : DataRowBase
{
    private int m_Id = 0;
    public override int Id => m_Id;
    
    public float Speed { get; private set; }
    public float JumpHeight { get; private set; }
    public float AirDuration { get; private set; }
    public float AirSpeedRate { get; private set; }

    public float AttackWaitDuration { get; private set; }

    public float AttackDuration { get; private set; }
    
    public float AttackEixtDuration { get; private set; }

    
    public float DodgeLength { get; private set; }
    public float DodgeSpeed { get; private set; }

    public override bool ParseDataRow(string dataRowString, object userData)
    {
        string[] colString = dataRowString.Split('\t');
        int index = 1;
        
        m_Id = int.Parse(colString[index++]);
        Speed = float.Parse(colString[index++]);
        JumpHeight = float.Parse(colString[index++]);
        AirDuration = float.Parse(colString[index++]);
        AirSpeedRate = float.Parse(colString[index++]);
        
        AttackWaitDuration = float.Parse(colString[index++]);
        AttackDuration = float.Parse(colString[index++]);
        AttackEixtDuration = float.Parse(colString[index++]);
        
        
        DodgeLength = float.Parse(colString[index++]);
        DodgeSpeed = float.Parse(colString[index++]);
        
        return true;
    }
}