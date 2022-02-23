using UnityEngine;
using System;
namespace Util.UpdateManager
{
    public enum Updatetype
    {
        UnsafeUpdate,UnsafeLateUpdate,UnsafeFixedUpdate
    }
    interface UnsafeUpdateIndex
    {
        void SetIndex(in Updatetype type, in uint index);
    }
    interface UnsafeUpdate : UnsafeUpdateIndex
    {
        uint Using_Unsafe_Update_index { get; } void Unsafe_Update();
    }
    interface UnsafeLateUpdate : UnsafeUpdateIndex
    {
        uint Using_Unsafe_LateUpdate_index { get; } void Unsafe_LateUpdate();
    }
    interface UnsafeFixedUpdate : UnsafeUpdateIndex
    {
        uint Using_Unsafe_FixedUpdate_index { get; } void Unsafe_FixedUpdate();
    }
    public class GameLoops : MonoBehaviour
    {
        public const uint NULL = uint.MaxValue;
        private static GameLoops Instance
        {
            get
            {
                return instance ? instance : instance = FindObjectOfType<GameLoops>() ? FindObjectOfType<GameLoops>() : new GameObject("GameLoop").AddComponent<GameLoops>();
            }
        }
        private static GameLoops instance = default(GameLoops);

        struct UpdateInfo<T>
        {
            public T[] UnsafeUpdates;
            readonly Updatetype type;
            public uint updateCount;
            readonly uint resize;
            public void UnsafeAdd(in T updatable)
            {
                if (UnsafeUpdates.Length <= updateCount)
                {
                    T[] array2 = UnsafeUpdates;
                    UnsafeUpdates = new T[updateCount + resize];
                    Array.Copy(array2, UnsafeUpdates, updateCount);
                }
                if (UnsafeUpdates[updateCount] != null)
                {
                    for (uint UpdateArrayNum = 0; UpdateArrayNum < UnsafeUpdates.Length; UpdateArrayNum++)
                    {
                        if (UnsafeUpdates[UpdateArrayNum] == null)
                        {
                            updateCount = UpdateArrayNum;
                            break;
                        }
                    }
                }
                (updatable as UnsafeUpdateIndex).SetIndex(type, updateCount);
                UnsafeUpdates[updateCount] = updatable;
                updateCount++;
            }
            public void UnsafeRemove(in T updatable, uint index)
            {
                if (updateCount == 0) return;
                if(UnsafeUpdates[index] == null || !UnsafeUpdates[index].Equals(updatable))
                {
                    for (uint UpdateArrayNum = 0; UpdateArrayNum < UnsafeUpdates.Length; UpdateArrayNum++)
                    {
                        if (UnsafeUpdates[UpdateArrayNum].Equals(updatable))
                        {
                            index = UpdateArrayNum;
                            break;
                        }
                    }
                }
                updateCount--;
                UnsafeUpdates[index] = UnsafeUpdates[updateCount];
                (UnsafeUpdates[index] as UnsafeUpdateIndex).SetIndex(type​, index);
                UnsafeUpdates[updateCount] = default(T);
                Initialize(updatable);
            }
            public void Initialize(in T updatable)
            {
                (updatable as UnsafeUpdateIndex).SetIndex(type​, NULL);
            }
            public void Dispose()
            {
                updateCount = default(uint);
                Array.Clear(UnsafeUpdates, 0, UnsafeUpdates.Length);
                UnsafeUpdates = new T[resize];
            }
            public UpdateInfo(in uint arraysize, in Updatetype type)
            {
                updateCount = default(uint);
                this.type = type;
                resize = arraysize;
                UnsafeUpdates = new T[resize];
            }
        }
        private static UpdateInfo<UnsafeUpdate> unsafeUpdates = new UpdateInfo<UnsafeUpdate>(128, Updatetype.UnsafeUpdate);
        private static UpdateInfo<UnsafeLateUpdate> unsafeLateUpdates = new UpdateInfo<UnsafeLateUpdate>(32, Updatetype.UnsafeLateUpdate);
        private static UpdateInfo<UnsafeFixedUpdate> unsafeFixedUpdates = new UpdateInfo<UnsafeFixedUpdate>(64, Updatetype.UnsafeFixedUpdate);
        void Awake()
        {
            if (Instance != this) DestroyImmediate(this);
            else DontDestroyOnLoad(this.gameObject);
        }
        public bool _isQuitting { set; get; }
        public bool _isPauseing { set; get; }
        void Update()
        {
            for (uint UpdateArrayNum = 0; UpdateArrayNum < unsafeUpdates.updateCount; UpdateArrayNum++) unsafeUpdates.UnsafeUpdates[UpdateArrayNum].Unsafe_Update();
            for (uint UpdateArrayNum = 0; UpdateArrayNum < unsafeLateUpdates.updateCount; UpdateArrayNum++) unsafeLateUpdates.UnsafeUpdates[UpdateArrayNum].Unsafe_LateUpdate();
        }
        void FixedUpdate()
        {
            for (uint UpdateArrayNum = 0; UpdateArrayNum < unsafeFixedUpdates.updateCount; UpdateArrayNum++) unsafeFixedUpdates.UnsafeUpdates[UpdateArrayNum].Unsafe_FixedUpdate();
        }
        void OnApplicationQuit()
        {
            _isQuitting = true;
            unsafeUpdates.Dispose();
            unsafeLateUpdates.Dispose();
            unsafeFixedUpdates.Dispose();
        }
        void OnApplicationFocus(bool hasFocus)
        {
            _isPauseing = !hasFocus;
        }
        void OnApplicationPause(bool pauseStatus)
        {
            _isPauseing = pauseStatus;
        }

        public static bool UnsafeUpdatables<T>(in T updatable, params Updatetype[] Lt)
        {
            for (int i = 0; i != Lt.Length; i++)
                if (!UnsafeUpdatable(updatable, Lt[i])) return false;
            return true;
        }

        public static bool UnsafeUpdatable<T>(in T updatable, Updatetype Lt)
        {
            if (updatable == null || Instance == null || !typeof(UnsafeUpdateIndex).IsAssignableFrom(typeof(T))) return false;
            switch (Lt)
            {
                case Updatetype.UnsafeUpdate:
                    if ((updatable as UnsafeUpdate).Using_Unsafe_Update_index == NULL) unsafeUpdates.UnsafeAdd(updatable as UnsafeUpdate); break;
                case Updatetype.UnsafeLateUpdate:
                    if ((updatable as UnsafeLateUpdate).Using_Unsafe_LateUpdate_index == NULL) unsafeLateUpdates.UnsafeAdd(updatable as UnsafeLateUpdate); break;
                case Updatetype.UnsafeFixedUpdate:
                    if ((updatable as UnsafeFixedUpdate).Using_Unsafe_FixedUpdate_index == NULL) unsafeFixedUpdates.UnsafeAdd(updatable as UnsafeFixedUpdate); break;
                default: return false;
            }
            return true;
        }
        public static bool UnsafeRemoveUpdatable<T>(in T updatable, in Updatetype Lt)
        {
            if (updatable == null || !typeof(UnsafeUpdateIndex).IsAssignableFrom(typeof(T))) return false;
            switch (Lt)
            {
                case Updatetype.UnsafeUpdate:
                    if ((updatable as UnsafeUpdate).Using_Unsafe_Update_index != NULL) unsafeUpdates.UnsafeRemove(updatable as UnsafeUpdate, (updatable as UnsafeUpdate).Using_Unsafe_Update_index); break;
                case Updatetype.UnsafeLateUpdate:
                    if ((updatable as UnsafeLateUpdate).Using_Unsafe_LateUpdate_index != NULL) unsafeLateUpdates.UnsafeRemove(updatable as UnsafeLateUpdate, (updatable as UnsafeLateUpdate).Using_Unsafe_LateUpdate_index); break;
                case Updatetype.UnsafeFixedUpdate:
                    if ((updatable as UnsafeFixedUpdate).Using_Unsafe_FixedUpdate_index != NULL) unsafeFixedUpdates.UnsafeRemove(updatable as UnsafeFixedUpdate, (updatable as UnsafeFixedUpdate).Using_Unsafe_FixedUpdate_index); break;
                default: return false;
            }
            return true;
        }
        public static void Initialize<T>(in T updatable, params Updatetype[] Lt)
        {
            if (updatable == null || !typeof(UnsafeUpdateIndex).IsAssignableFrom(typeof(T))) return;
            unsafeUpdates.Initialize(updatable as UnsafeUpdate);
            unsafeLateUpdates.Initialize(updatable as UnsafeLateUpdate);
            unsafeFixedUpdates.Initialize(updatable as UnsafeFixedUpdate);
            if (!UnsafeUpdatables(updatable, Lt)) Debug.LogWarning("Needs improvement");
        }
        public static bool UnsafeAllDestoryed<T>(in T updatable)
        {
            if (updatable == null || !typeof(UnsafeUpdateIndex).IsAssignableFrom(typeof(T))) return false;
            if ((updatable as UnsafeUpdate).Using_Unsafe_Update_index != NULL)
                unsafeUpdates.UnsafeRemove(updatable as UnsafeUpdate, (updatable as UnsafeUpdate).Using_Unsafe_Update_index);
            if ((updatable as UnsafeLateUpdate).Using_Unsafe_LateUpdate_index != NULL)
                unsafeLateUpdates.UnsafeRemove(updatable as UnsafeLateUpdate, (updatable as UnsafeLateUpdate).Using_Unsafe_LateUpdate_index);
            if ((updatable as UnsafeFixedUpdate).Using_Unsafe_FixedUpdate_index != NULL)
                unsafeFixedUpdates.UnsafeRemove(updatable as UnsafeFixedUpdate, (updatable as UnsafeFixedUpdate).Using_Unsafe_FixedUpdate_index);
            return true;
        }
    }
}