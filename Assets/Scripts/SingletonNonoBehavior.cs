﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;
    public MultipleInstancesHandlingType handleMultipleInstances;
    public bool persistant;

    public virtual void Start()
    {
        if (handleMultipleInstances != MultipleInstancesHandlingType.KeepAll && instance != null && instance != this)
        {
            if (handleMultipleInstances == MultipleInstancesHandlingType.DestroyNew)
            {
                Destroy(gameObject);
                return;
            }
            else
                Destroy(instance.gameObject);
        }
        instance = this as T;
        if (persistant)
            DontDestroyOnLoad(gameObject);
    }

    public static T GetInstance()
    {
        if (instance == null)
            instance = FindObjectOfType<T>();
        return instance;
    }

    public enum MultipleInstancesHandlingType
    {
        KeepAll,
        DestroyNew,
        DestroyOld
    }
}
