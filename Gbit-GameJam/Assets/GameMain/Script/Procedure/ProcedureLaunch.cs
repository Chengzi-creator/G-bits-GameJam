using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

public class ProcedureLaunch : ProcedureBase
{
    protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        ChangeState<ProcedureSplash>(procedureOwner);
        Log.Debug("GameJam Launch");
    }
}