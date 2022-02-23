using UnityEngine;
using Util.UpdateManager;

///����Update�p�N���X
public class CustomMonoBehaviour : MonoBehaviour, UnsafeUpdate, UnsafeLateUpdate, UnsafeFixedUpdate
{
    //interface�Œ�`���Ă��Ȃ�get,set�Ɋւ��Ă͎��R�ɒǉ��\
    public uint Using_Unsafe_Update_index { get; private set; }
    public uint Using_Unsafe_LateUpdate_index { get; private set; }
    public uint Using_Unsafe_FixedUpdate_index { get; private set; }

    //�����I�Ɏ������鎖�ŁA�����I��private�ȏ�Ԃ������ł���
    void UnsafeUpdateIndex.SetIndex(in Updatetype type, in uint index)
    {
        switch (type)
        {
            case Updatetype.UnsafeUpdate: Using_Unsafe_Update_index = index; break;
            case Updatetype.UnsafeLateUpdate: Using_Unsafe_LateUpdate_index = index; break;
            case Updatetype.UnsafeFixedUpdate: Using_Unsafe_FixedUpdate_index = index; break;
            default: break;
        }
    }
    //�I�u�W�F�N�g�̃A�N�e�B�u����Initialize���ĂԁBparams�Ŏ������鎖�ň�������ɏo����
    protected virtual void OnEnable() => Initialize();
    protected virtual void Initialize(params Updatetype[] Lt) => GameLoops.Initialize(this, Lt);
    //�I�u�W�F�N�g�̔�A�N�e�B�u����UnsafeAllDestoryed���ĂԁB�����Update���~�܂�
    protected virtual void OnDisable() => GameLoops.UnsafeAllDestoryed(this);

    //�Ԉ����Update�����̏ꍇ�͏C�����s���uPlease change the 'Updatetype'�v���o�͂���B
    private void NeedToBeFixed(in Updatetype type)
    {
        GameLoops.UnsafeRemoveUpdatable(this, type);
        switch (type)
        {
            case Updatetype.UnsafeUpdate: Debug.LogWarning(name + " Warning! : Remove 'Updatetype.UnsafeUpdate' from 'Initialize base'."); break;
            case Updatetype.UnsafeLateUpdate: Debug.LogWarning(name + " Warning! : Remove 'Updatetype.UnsafeLateUpdate' from 'Initialize base'."); break;
            case Updatetype.UnsafeFixedUpdate: Debug.LogWarning(name + " Warning! : Remove 'Updatetype.UnsafeFixedUpdate' from 'Initialize base'."); break;
            default: Debug.LogError(name + " Error! : Remove 'Initialize base'."); break;
        }
    }
    //override���Ďg���BUnsafe_Update = Update
    public virtual void Unsafe_Update() => NeedToBeFixed(Updatetype.UnsafeUpdate);
    //override���Ďg���BUnsafe_FixedUpdate = FixedUpdate
    public virtual void Unsafe_FixedUpdate() => NeedToBeFixed(Updatetype.UnsafeFixedUpdate);
    //override���Ďg���BUnsafe_LateUpdate = LateUpdate
    public virtual void Unsafe_LateUpdate() => NeedToBeFixed(Updatetype.UnsafeLateUpdate);
}