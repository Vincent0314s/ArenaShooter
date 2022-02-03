using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPoolGE<T>
{
    public void InitPool_ObjectDeactived();
    public void InitPool_ObjectActived();

    public T GetObjectFromPool();
    public void ReturnObjectToPool_ObjectDeactived(T _pooledObject);
    public void ReturnObjectToPool_ObjectActived(T _pooledObject);
}
