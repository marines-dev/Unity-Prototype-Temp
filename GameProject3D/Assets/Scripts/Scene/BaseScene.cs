using System;
using System.Collections;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    static bool doInitSceneLoad = false;
    bool doLoadProcess = false;

    IEnumerator loadingProcessCoroutine = null;
    protected IEnumerator loadingProcessRoutine = null;


    void Awake()
    {
        doLoadProcess = false;

        if (doInitSceneLoad == false && Managers.Scene.currentSceneType != Define.Scene.InitScene)
        {
            Debug.LogWarning(string.Format($"���� �ʱ�ȭ�� ���� {typeof(InitScene).Name} ������ �̵��մϴ�."));

            //
            //Managers.Scene.LoadScene<InitScene>();
            string sceneName = Define.Scene.InitScene.ToString();
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            return;
        }

        LoadProcess();
    }

    void OnDestroy()
    {
        CloseScene();
    }

    #region Virtual

    /// <summary>
    /// ������ �ε��� �����͵��� ������� �����ϴ� �Լ� �Դϴ�.
    /// </summary>
    protected abstract IEnumerator LoadingProcessRoutine();

    /// <summary>
    /// �� �ε� �� BaseScene�� ���� ���� �Դϴ�.
    /// </summary>
    protected abstract void OpenScene();

    /// <summary>
    /// �� ���� ������ �ʱ�ȭ �մϴ�.
    /// </summary>
    protected abstract void CloseScene();

    #endregion Virtual

    [Obsolete("InitScene ���� : InitScene���� �ʱ�ȭ �� ȣ���մϴ�.")]
    protected void CompleteInitSceneLoading()
    {
        if (Managers.Scene.currentSceneType != Define.Scene.InitScene)
        {
            Debug.LogWarning("");
            return;
        }

        doInitSceneLoad = true;
    }

    [Obsolete("SceneManager ���� : BaseScene�� LoadingProcess�� �Ϸ��ϸ� SceneManager���� ȣ���ϴ� �Լ��Դϴ�.")] 
    public bool DoLoadProcess()
    {
        string baseSceneName = this.GetType().Name;
        string loadingSceneName = Managers.Scene.loadingSceneType.ToString();
        if (Managers.Scene.isIoading == false || baseSceneName != loadingSceneName)
        {
            Debug.LogWarning($"Failed : {baseSceneName}�� �ε��� �����ϰ� ���� �ʽ��ϴ�.");
            return false;
        }

        return doLoadProcess;
    }

    void LoadProcess()
    {
        // loadingProcessRoutine
        if (loadingProcessRoutine != null)
        {
            StopCoroutine(loadingProcessRoutine);
            loadingProcessRoutine = null;
        }

        // initProcessCoroutine
        if (loadingProcessCoroutine != null)
        {
            StopCoroutine(loadingProcessCoroutine);
            loadingProcessCoroutine = null;
        }

        loadingProcessCoroutine = LoadingProcessCoroutine();
        StartCoroutine(loadingProcessCoroutine);

        Debug.Log($"Success : {this.GetType().Name}�� LoadProcess �Ϸ�");
    }

    /// <summary>
    /// �� ���� ������ �ε带 �����ϴ� �Լ� �Դϴ�.
    /// </summary>
    IEnumerator LoadingProcessCoroutine()
    {
        loadingProcessRoutine = LoadingProcessRoutine();
        yield return loadingProcessRoutine;

        // Complete
        // �ص����� �ε� �� �ʱ�ȭ �Ϸ� �� ȣ���ϴ� �Լ� �Դϴ�.
        //yield return new WaitForEndOfFrame();
        doLoadProcess = true;
        OpenScene();
    }
}
