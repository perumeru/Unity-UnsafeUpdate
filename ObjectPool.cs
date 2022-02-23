using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Util.UpdateManager;

public class ObjectPool : CustomMonoBehaviour
{
    [SerializeField]
    GameObject poolbase;
    [SerializeField]
    List<GameObject> pool;
    [SerializeField]
    List<int> countpool;
    [SerializeField]
    int counter = 0;
    [SerializeField]
    int poolsize;
    protected override void Initialize(params Updatetype[] Lt)
    {
        base.Initialize(Updatetype.UnsafeUpdate);
        if (poolbase == null)

            poolbase.SetActive(false);

        pool = new List<GameObject>();
        countpool = new List<int>();
        for (int i = 0; i < poolsize; i++)
        {
            poolbase.name = i.ToString();
            pool.Add(Instantiate(poolbase, transform));
            countpool.Add(i);
        }
        //ƒVƒƒƒbƒtƒ‹‚·‚é
        countpool = countpool.OrderBy(i => System.Guid.NewGuid()).ToList();

    }
    public override void Unsafe_Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (counter < poolsize)
            {
                Destroy(pool[countpool[counter]]);
                countpool.Remove(countpool[counter]);
                counter++;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            pool.Add(Instantiate(poolbase, transform));
            countpool.Add(poolsize);
            countpool = countpool.OrderBy(i => System.Guid.NewGuid()).ToList();
            poolsize++;
        }
    }
}
