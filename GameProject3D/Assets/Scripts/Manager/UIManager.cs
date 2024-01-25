using System;
using System.Collections.Generic;
using UnityEngine;
// MainUI /  PopupUI 구분 처리 필요

[Obsolete("Managers 전용 : 일반 클래스에서 사용할 수 없습니다. Managers를 이용해 주세요.")]
public class UIManager : BaseManager
{
    List<BaseUI> list_BaseUI = new List<BaseUI>();

    Canvas canvas_go_pro = null;
    public Canvas canvas_go
    {
        get
        {
            if (canvas_go_pro == null)
            {
                LoadCanvas();
            }
            return canvas_go_pro;
        }
    }

    [Obsolete("테스트 중 : Canvas 생성 시 자동 생성 확인 필요")]
    UnityEngine.EventSystems.EventSystem eventSystem_go_pro = null;
    public UnityEngine.EventSystems.EventSystem eventSystem_go
    {
        get
        {
            if (eventSystem_go_pro == null)
            {
                LoadEventSystem();
            }

            return eventSystem_go_pro;
        }
    }

    GameObject uiStorage_go_pro;
    GameObject uiIStorage_go
    {
        get
        {
            if (uiStorage_go_pro == null)
            {
                LoadUIStorage();
            }

            return uiStorage_go_pro;
        }
    }

    #region Override

    protected override void InitDataProcess()
    {
        LoadCanvas();
        LoadEventSystem();
        LoadUIStorage();
    }

    protected override void ResetDataProcess()
    {
    }

    #endregion Override

    #region BaseUI

    // StaticBaseUI
    public void ResisteredBaseUI()
    {
        if (uiIStorage_go.transform.childCount != 0)
        {
            for (int i = 0; i < uiIStorage_go.transform.childCount; ++i)
            {
                Transform childTrans = uiIStorage_go.transform.GetChild(i);

                SetBaseUI(childTrans.gameObject);
            }
        }
    }

    public void LoadUI<TBaseUI>() where TBaseUI : BaseUI
    {
        BaseUI baseUI = FindBaseUI<TBaseUI>();
        string baseUI_name = typeof(TBaseUI).Name;
        if (baseUI != null)
        {
            Debug.LogWarning(string.Format("{0} 타입의 BaseUI는 이미 존재합니다.", baseUI_name));
            return;
        }

        string path = $"Prefabs/UI/{baseUI_name}";
        GameObject resource = Managers.Resource.InstantiateResource(path, uiIStorage_go.transform);
        SetBaseUI(resource.gameObject);
    }

    public T CreateWorldSpaceUI<T>(Transform parent = null, string name = null) where T : BaseUI
    {
        Debug.Log("HPBarUI가 생성되었습니다.");
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.InstantiateResource($"Prefabs/UI/WorldSpace/{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        BaseUI uiBase = go.GetOrAddComponent<T>();

        uiBase.Initialized();
        uiBase.CloseUI();

        return Util.GetOrAddComponent<T>(go);
    }

    public void SetBaseUI<T>(T _ui_obj) where T : BaseUI
    {
        if (_ui_obj.transform.parent != uiIStorage_go.transform)
        {
            _ui_obj.transform.SetParent(uiIStorage_go.transform);
        }

        Type type = typeof(T);
        if (_ui_obj.name != type.Name)
        {
            _ui_obj.name = type.Name;
        }

        T ui = _ui_obj.gameObject.GetComponent<T>();
        if (ui == null)
        {
            ui = _ui_obj.gameObject.AddComponent<T>();
        }

        BaseUI uiBase = null;
        int uiBaseIndex = list_BaseUI.FindIndex(x => x.name == ui.name);
        if (uiBaseIndex == -1)
        {
            uiBase = ui;
            list_BaseUI.Add(uiBase);
        }
        else
        {
            uiBase = ui;
            list_BaseUI[uiBaseIndex] = uiBase;
        }

        uiBase.Initialized();

        CloseBaseUI<T>();
    }

    public void SetBaseUI(GameObject _ui_obj)
    {
        if (_ui_obj.transform.parent != uiIStorage_go.transform)
        {
            _ui_obj.transform.SetParent(uiIStorage_go.transform);
        }

        Type type = Type.GetType(_ui_obj.name);
        if (type == null)
        {
            Debug.LogError(string.Format("{0}는 존재하지 않는 UI 이름입니다.", _ui_obj.name));
            return;
        }

        BaseUI uiBase = _ui_obj.GetComponent(type) as BaseUI;
        if (uiBase == null)
        {
            uiBase = _ui_obj.AddComponent(type) as BaseUI;
        }

        int uiBaseIndex = list_BaseUI.FindIndex(x => x.name == uiBase.name);
        if (uiBaseIndex == -1)
        {
            list_BaseUI.Add(uiBase);
        }
        else
        {
            list_BaseUI[uiBaseIndex] = uiBase;
        }

        uiBase.Initialized();

        CloseBaseUI(uiBase.name);
    }

    public T GetBaseUI<T>() where T : BaseUI
    {
        T baseUI = FindBaseUI<T>().GetComponent<T>();

        if (baseUI == null)
        {
            Debug.LogWarning($"Failed: {typeof(T).Name}의 BaseUI는 등록되어 있지 않습니다. 먼저 {typeof(T).Name}를 로드해 주세요.");
            return null;
        }

        return baseUI;
    }

    public BaseUI GetBaseUI(string _uiName)
    {
        Type type = Type.GetType(_uiName);
        BaseUI baseUI = FindBaseUI(_uiName).GetComponent(type) as BaseUI;

        if (baseUI == null)
        {
            Debug.LogWarning("얻기 실패 : UIPanel을 찾을 수 없습니다.");
            return null;
        }

        return baseUI;
    }

    BaseUI FindBaseUI<T>() where T : BaseUI
    {
        Type type = typeof(T);
        BaseUI uiBase = list_BaseUI.Find(x => x.name == type.Name);

        return uiBase;
    }

    BaseUI FindBaseUI(string _uiName)
    {
        BaseUI uiBase = list_BaseUI.Find(x => x.name == _uiName);

        return uiBase;
    }

    public void OpenBaseUI<T>() where T : BaseUI
    {
        OpenBaseUI(typeof(T).Name);
    }

    public void OpenBaseUI(string _uiName)
    {
        Debug.LogWarning("uiID로 변경 필요!!");

        BaseUI uiBase = FindBaseUI(_uiName);

        if (uiBase == null)
        {
            Debug.LogWarning(string.Format("Failed : 열기 위한 {0}의 UI를 찾을 수 없습니다.", _uiName));
            return;
        }

        uiBase.OpenUI();
    }

    public void CloseBaseUI<T>() where T : BaseUI
    {
        CloseBaseUI(typeof(T).Name);
    }

    public void CloseBaseUI(string _uiName)
    {
        Debug.LogWarning("uiID로 변경 필요!!");
        BaseUI uiBase = FindBaseUI(_uiName);

        if (uiBase == null)
        {
            Debug.LogWarning("Failed : 닫을 UI를 찾을 수 없습니다.");
            return;
        }

        uiBase.CloseUI();
    }

    public void OpenBaseUIAll()
    {
        if (list_BaseUI.Count == 0)
        {
            Debug.LogWarning("열기 위한 UI가 없습니다.");
            return;
        }

        foreach (var uiBase in list_BaseUI)
        {
            if (uiBase.gameObject != null)
            {
                uiBase.OpenUI();
            }
        }
    }

    public void CloseBaseUIAll()
    {
        if (list_BaseUI.Count == 0)
        {
            Debug.LogWarning("Failed : 닫기 위한 UI가 없습니다.");
            return;
        }

        foreach (var uiBase in list_BaseUI)
        {
            if (uiBase != null && uiBase.gameObject != null)
            {
                uiBase.CloseUI();
            }
        }
    }

    #endregion BaseUI

    #region Load

    void LoadCanvas()
    {
        if (canvas_go_pro != null)
            return;

        canvas_go_pro = GameObject.FindObjectOfType<Canvas>();
        if (canvas_go_pro == null)
        {
            canvas_go_pro = Managers.Resource.InstantiateResource("Prefabs/UI/Canvas").GetComponent<Canvas>();

        }

        string go_name = $"@{typeof(Canvas).Name}";
        canvas_go_pro.gameObject.name = go_name;
        canvas_go_pro.gameObject.SetActive(true);

        //
        DontDestroyOnLoad(canvas_go_pro);
    }

    void LoadEventSystem()
    {
        if (eventSystem_go_pro != null)
            return;

        eventSystem_go_pro = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        if (eventSystem_go_pro == null)
        {
            eventSystem_go_pro = Managers.Resource.InstantiateResource("Prefabs/UI/EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>();
        }

        string go_name = $"@{typeof(UnityEngine.EventSystems.EventSystem).Name}";
        eventSystem_go_pro.gameObject.name = go_name;
        eventSystem_go_pro.gameObject.SetActive(true);

        //
        DontDestroyOnLoad(eventSystem_go_pro);
    }

    [Obsolete("테스트 중")]
    void LoadUIStorage()
    {
        if (uiStorage_go_pro != null)
            return;

        uiStorage_go_pro = canvas_go.transform.Find(Config.ui_uiStorageName).gameObject;
        if (uiStorage_go_pro == null)
        {
            uiStorage_go_pro = Managers.Resource.CreateGameObject(Config.ui_uiStorageName, canvas_go.transform);
        }

        //
        DontDestroyOnLoad(uiStorage_go_pro);
    }

    #endregion Load
}
