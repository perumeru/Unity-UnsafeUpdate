using System.Text;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField, Multiline(10)]
    public string DebugLog;
    StringBuilder stringBuilder = new StringBuilder();

    float lastCollectNum = 0;
    private const int byte_bitmegabyte = 1048576;
    private const int megabyte = 1000000;
    float lastCollect = 0;
    float delta = 0;
    float lastDeltaTime = 0;
    int allocRate = 0;
    int lastAllocMemory = 0;
    float lastAllocSet = -9999;
    int allocMem = 0;
    int collectAlloc = 0;
    int peakAlloc = 0;

    [SerializeField]
    float fpsAverage = 0.0f;
    float fpsCounter = 0.0f;
    float frameCounter = 0.0f;
    // Update is called once per frame
    private void LateUpdate()
    {
        int collCount = System.GC.CollectionCount(0);

        if (lastCollectNum != collCount)
        {
            lastCollectNum = collCount;
            delta = Time.realtimeSinceStartup - lastCollect;
            lastCollect = Time.realtimeSinceStartup;
            lastDeltaTime = Time.deltaTime;
            collectAlloc = allocMem;
        }

        allocMem = (int)System.GC.GetTotalMemory(false);

        peakAlloc = allocMem > peakAlloc ? allocMem : peakAlloc;

        if (Time.realtimeSinceStartup - lastAllocSet > 0.5F)
        {
            int diff = allocMem - lastAllocMemory;
            lastAllocMemory = allocMem;
            lastAllocSet = Time.realtimeSinceStartup;

            if (diff >= 0)
            {
                allocRate = diff;
            }
        }

        stringBuilder.Append(
            "���݊��蓖�Ă��Ă��郁����" +
            (allocMem / megabyte).ToString("0") +
            ("mb\n")
            );

        stringBuilder.Append(
            "���蓖�ă������̃s�[�N" +
            (peakAlloc / megabyte).ToString("0") +
            ("mb(last collect ") +
            (collectAlloc / megabyte).ToString("0") +
            ("mb)\n")
            );

        stringBuilder.Append(
             "�������̎��W�p�x(0.5/s)" +
             (allocRate / megabyte).ToString("0.0") +
             ("mb\n")
             );

        stringBuilder.Append(
            "�K�x�R���̕p�x " +
            delta.ToString("0.00") +
            ("s\n")
            );

        stringBuilder.Append(
            "�Ō�̎��W�Ƃ̍���" +
            lastDeltaTime.ToString("0.000") +
            ("s (") + (1F / lastDeltaTime).ToString("0.0)(fps)\n")
            );

        stringBuilder.Append(
            "�g�p�����q�[�v�T�C�Y/���C���������̗e�� " +
            (UnityEngine.Profiling.Profiler.usedHeapSizeLong / byte_bitmegabyte + " / " + SystemInfo.systemMemorySize.ToString("0")) +
            ("MB\n")
            );

        stringBuilder.Append(
        "GCcount->" + lastCollectNum + "\n");

        DebugLog = stringBuilder.ToString();
        stringBuilder.Clear();

        var fps = 1F / Time.deltaTime;
        fpsCounter += fps;
        frameCounter++;

        if(frameCounter > fps)
        {
            fpsAverage = fpsCounter / frameCounter;
            frameCounter = fpsCounter = 0.0f;
        }
    }
}