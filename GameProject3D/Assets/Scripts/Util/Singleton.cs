using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T:class, new()
{
    private static T _instance = null;
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();

                System.Type type = typeof(T);
                string message = string.Format("SingletonManager<{0}>�� �����Ǿ����ϴ�.", type.ToString());
                Debug.Log(message);
            }

            return _instance;
        }
    }

    public Singleton()
    { }

    public void Release()
    {
        if(_instance != null)
        {
            _instance = null;

            System.Type type = typeof(T);
            string message = string.Format("SingletonManager<{0}>�� �����Ǿ����ϴ�.", type.ToString());
            Debug.Log(message);
        }
    }
}
