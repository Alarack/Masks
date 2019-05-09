using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PooledObject : MonoBehaviour, IPoolable {

    public ObjectPoolManager.ObjectPoolType poolType;
    //public virtual string ObjectName { get { return string.IsNullOrEmpty(objectName) ? "" : objectName; } set { objectName = value; } }
    //protected string objectName;


    public virtual void OnGet()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnReturn()
    {
        gameObject.SetActive(false);
    }

}
