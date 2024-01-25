using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : Object
{
    public bool isInitialized { get; private set; } = false;

    static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                InitInstance();
            }

            return instance;
        }
    }

    static void InitInstance()
    {
        if (instance == null)
        {
            System.Type type = typeof(T);
            string instance_name = "@" + type.Name;

            GameObject go = GameObject.Find(instance_name);
            if (go == null)
            {
                go = new GameObject { name = instance_name };
                instance = go.AddComponent(type) as T;
            }

            DontDestroyOnLoad(instance);

            Debug.Log($"<{type.Name}>�� �����Ǿ����ϴ�.");
        }
    }

    public void Initialized()
    {
        if (isInitialized)
        {
            Debug.LogWarning("�̹� �ʱ�ȭ�� �Ϸ��߽��ϴ�.");
            return;
        }

        isInitialized = true;

        InitializedProcess();
    }

    protected abstract void InitializedProcess();

    public void Clear()
    {
        ClearProcess();

        isInitialized = false;

        if (instance != null)
        {
            Destroy(instance);
            instance = null;

            System.Type type = typeof(T);
            Debug.Log(string.Format("MonoSingletonManager<{0}>�� �����Ǿ����ϴ�.", type.Name));
            Debug.LogWarning("Debug �׽�Ʈ : Destroy �׽�Ʈ �ʿ�");
        }

    }

    protected abstract void ClearProcess();

    public void OnDestroy()
    {
        Clear();
    }
}

