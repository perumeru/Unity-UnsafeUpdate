using UnityEngine;
using Util.UpdateManager;

public class Chara : CustomMonoBehaviour
{
    public uint Unsafe_UpdateCallCount;
    public uint Unsafe_LateUpdateCallCount;
    public uint Unsafe_FixedUpdateCallCount;
    // Start is called before the first frame update
    protected override void Initialize(params Updatetype[] Lt)
    {
        base.Initialize(Updatetype.UnsafeUpdate, Updatetype.UnsafeFixedUpdate, Updatetype.UnsafeLateUpdate);
    }

    public override void Unsafe_LateUpdate()
    {
       
    }

    public override void Unsafe_Update()
    {
        Unsafe_FixedUpdateCallCount = Using_Unsafe_FixedUpdate_index;
        Unsafe_LateUpdateCallCount = Using_Unsafe_LateUpdate_index;
        Unsafe_UpdateCallCount = Using_Unsafe_Update_index;
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameLoops.UnsafeRemoveUpdatable(this, Updatetype.UnsafeLateUpdate);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameLoops.UnsafeUpdatable(this, Updatetype.UnsafeLateUpdate);
        }
    }
}
