using UnityEngine;
using Util.UpdateManager;

///自作Update用クラス
public class CustomMonoBehaviour : MonoBehaviour, UnsafeUpdate, UnsafeLateUpdate, UnsafeFixedUpdate
{
    //interfaceで定義していないget,setに関しては自由に追加可能
    public uint Using_Unsafe_Update_index { get; private set; }
    public uint Using_Unsafe_LateUpdate_index { get; private set; }
    public uint Using_Unsafe_FixedUpdate_index { get; private set; }

    //明示的に実装する事で、実質的にprivateな状態を実装できる
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
    //オブジェクトのアクティブ時にInitializeを呼ぶ。paramsで実装する事で引数を空に出来る
    protected virtual void OnEnable() => Initialize();
    protected virtual void Initialize(params Updatetype[] Lt) => GameLoops.Initialize(this, Lt);
    //オブジェクトの非アクティブ時にUnsafeAllDestoryedを呼ぶ。これでUpdateが止まる
    protected virtual void OnDisable() => GameLoops.UnsafeAllDestoryed(this);

    //間違ったUpdate実装の場合は修正を行い「Please change the 'Updatetype'」を出力する。
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
    //overrideして使う。Unsafe_Update = Update
    public virtual void Unsafe_Update() => NeedToBeFixed(Updatetype.UnsafeUpdate);
    //overrideして使う。Unsafe_FixedUpdate = FixedUpdate
    public virtual void Unsafe_FixedUpdate() => NeedToBeFixed(Updatetype.UnsafeFixedUpdate);
    //overrideして使う。Unsafe_LateUpdate = LateUpdate
    public virtual void Unsafe_LateUpdate() => NeedToBeFixed(Updatetype.UnsafeLateUpdate);
}