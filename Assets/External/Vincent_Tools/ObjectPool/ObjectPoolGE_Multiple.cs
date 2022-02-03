using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolData<T>
{
    public string poolName;
    public T prefab;
    public int poolSize;

    [HideInInspector]
    public List<T> m_freeList;
    [HideInInspector]
    public List<T> m_usedList;

    public void InitListSize() {
        m_freeList = new List<T>(poolSize);
        m_freeList = new List<T>(poolSize);
    }
}
public class ObjectPoolGE_Multiple<T> : MonoBehaviour,IObjectPoolGE<T> where T : MonoBehaviour
{

    public List<PoolData<T>> pools = new List<PoolData<T>>();

    public void InitPool_ObjectDeactived()
    {
        foreach (var item in pools)
        {
            item.InitListSize();

            for (int i = 0; i < item.poolSize; i++)
            {
                var pooledObject = Instantiate(item.prefab, transform);
                pooledObject.gameObject.SetActive(false);
                item.m_freeList.Add(pooledObject);
            }
        }
    }

    public void InitPool_ObjectActived()
    {
        foreach (var item in pools)
        {
            item.InitListSize();

            for (int i = 0; i < item.poolSize; i++)
            {
                var pooledObject = Instantiate(item.prefab, transform);
                pooledObject.gameObject.SetActive(true);
                item.m_freeList.Add(pooledObject);
            }
        }
    }
    /// <summary>
    /// Get Object from first Pool
    /// </summary>
    /// <returns></returns>
    public T GetObjectFromPool()
    {

        int numFree = pools[0].m_freeList.Count;
        if (numFree == 0)
            return null;

        var pooledObject = pools[0].m_freeList[numFree - 1];
        pools[0].m_freeList.RemoveAt(numFree - 1);
        pools[0].m_usedList.Add(pooledObject);
        return pooledObject;

        //foreach (var item in pools)
        //{
        //    int numFree = item.m_freeList.Count;
        //    if (numFree == 0)
        //        return null;

        //    var pooledObject = item.m_freeList[numFree - 1];
        //    item.m_freeList.RemoveAt(numFree - 1);
        //    item.m_usedList.Add(pooledObject);
        //    return pooledObject;
        //}
        //return null;
    }

    /// <summary>
    /// Get Object from selected pool by Index
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public T GetObjectFromPool(int _index)
    {

        int numFree = pools[_index].m_freeList.Count;
        if (numFree == 0)
            return null;

        var pooledObject = pools[_index].m_freeList[numFree - 1];
        pools[_index].m_freeList.RemoveAt(numFree - 1);
        pools[_index].m_usedList.Add(pooledObject);
        return pooledObject;
    }

    /// <summary>
    /// Return Object to First Pool
    /// </summary>
    /// <param name="_pooledObject"></param>
    public void ReturnObjectToPool_ObjectDeactived(T _pooledObject)
    {
        Debug.Assert(pools[0].m_usedList.Contains(_pooledObject));

        pools[0].m_usedList.Remove(_pooledObject);
        pools[0].m_freeList.Add(_pooledObject);

        var pooledObjectTransform = _pooledObject.transform;
        pooledObjectTransform.parent = transform;
        pooledObjectTransform.localPosition = Vector3.zero;
        if (_pooledObject.gameObject.activeSelf)
            _pooledObject.gameObject.SetActive(false);
    }

    /// <summary>
    /// Return Object to Pool by Index
    /// </summary>
    /// <param name="_pooledObject"></param>
    /// <param name="_index"></param>
    public void ReturnObjectToPool_ObjectDeactived(T _pooledObject,int _index)
    {
        Debug.Assert(pools[_index].m_usedList.Contains(_pooledObject));

        pools[_index].m_usedList.Remove(_pooledObject);
        pools[_index].m_freeList.Add(_pooledObject);

        var pooledObjectTransform = _pooledObject.transform;
        pooledObjectTransform.parent = transform;
        pooledObjectTransform.localPosition = Vector3.zero;
        if (_pooledObject.gameObject.activeSelf)
            _pooledObject.gameObject.SetActive(false);
    }

    /// <summary>
    /// Return Object to First Pool
    /// </summary>
    /// <param name="_pooledObject"></param>
    public void ReturnObjectToPool_ObjectActived(T _pooledObject)
    {
        Debug.Assert(pools[0].m_usedList.Contains(_pooledObject));

        pools[0].m_usedList.Remove(_pooledObject);
        pools[0].m_freeList.Add(_pooledObject);

        var pooledObjectTransform = _pooledObject.transform;
        pooledObjectTransform.parent = transform;
        pooledObjectTransform.localPosition = Vector3.zero;
        if (!_pooledObject.gameObject.activeSelf)
            _pooledObject.gameObject.SetActive(true);
    }

    /// <summary>
    /// Return Object to Pool by Index
    /// </summary>
    /// <param name="_pooledObject"></param>
    /// <param name="_index"></param>
    public void ReturnObjectToPool_ObjectActived(T _pooledObject,int _index)
    {
        Debug.Assert(pools[_index].m_usedList.Contains(_pooledObject));

        pools[_index].m_usedList.Remove(_pooledObject);
        pools[_index].m_freeList.Add(_pooledObject);

        var pooledObjectTransform = _pooledObject.transform;
        pooledObjectTransform.parent = transform;
        pooledObjectTransform.localPosition = Vector3.zero;
        if (!_pooledObject.gameObject.activeSelf)
            _pooledObject.gameObject.SetActive(true);
    }
}
