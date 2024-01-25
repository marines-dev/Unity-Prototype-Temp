using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


public abstract class BaseUI : MonoBehaviour
{
    GameObject      uiObj = null;

    List<Transform> list_uiTrans = new List<Transform>();
    Transform[]     array_useUITrans = null;

    IEnumerator openUICoroutine = null;


    public void Initialized()
    {
        uiObj = gameObject;

        if(openUICoroutine != null)
        {
            StopCoroutine(openUICoroutine);
            openUICoroutine = null;
        }

        InitControlList(uiObj);
        BindControls();
        BindEvents();
        InitUI();
    }

    private void InitControlList(GameObject _uiPanelObj)
    {
        if(list_uiTrans != null && list_uiTrans.Count != 0)
        {
            list_uiTrans.Clear();
        }

        ReculsiveControl(_uiPanelObj.transform);
    }

    private void ReculsiveControl(Transform _trans)
    {
        int findIndex = list_uiTrans.FindIndex(x => x == _trans);
        if (findIndex == -1)
        {
            list_uiTrans.Add(_trans);
        }
        else
        {
            list_uiTrans[findIndex] = _trans;
        }

        if (_trans.childCount != 0)
        {
            for (int i = 0; i < _trans.childCount; i++)
            {
                Transform childTrans = _trans.GetChild(i);

                ReculsiveControl(childTrans);
            }
        }
    }

    protected virtual void BindControls()
    {
    }

    protected virtual void BindEvents()
    {
    }

    protected void BindControl<T>() where T : Enum
    {
        Array    array_control   = System.Enum.GetValues(typeof(T));
        int      uiCount         = array_control.Length;
        array_useUITrans         = new Transform[uiCount];

        foreach (int control in array_control)
        {
            string      uiName      = System.Enum.GetName(typeof(T), control);
            Transform   childTrans  = list_uiTrans.Find(x => x.gameObject.name == uiName);
            int         uiIndex     = (int)control;

            array_useUITrans[uiIndex] = childTrans;
        }
    }
    
    protected void BindEventControl<T>(Enum _enum, UnityAction _action)
    {
        GameObject obj = GetControlObject(_enum);

        // Button
        if (typeof(T) == typeof(Button))
        {
            Button button = obj.GetComponent<Button>();

            if (button == null)
            {
                button = obj.AddComponent<Button>();
            }

            button.onClick.AddListener(_action);
        }
    }

    protected virtual void InitUI()
    {
    }

    public void OpenUI()
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }

        //UIManager_Test.instance.OpenUI<ScreenCoverUI>();
        //UIManager_Test.instance.GetUI<ScreenCoverUI>().SetScreeConverUI(ScreenCoverUI.ScreenCoverType.LoadingData);

        OpenUIProcess();

        //UIManager_Test.instance.CloseUI<ScreenCoverUI>();
    }

    protected abstract void OpenUIProcess();

    public void CloseUI()
    {
        if (gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }

        //UIManager_Test.instance.OpenUI<ScreenCoverUI>();
        //UIManager_Test.instance.GetUI<ScreenCoverUI>().SetScreeConverUI(ScreenCoverUI.ScreenCoverType.LoadingData);

        CloseUIProcess();

        //UIManager_Test.instance.CloseUI<ScreenCoverUI>();
    }

    protected abstract void CloseUIProcess();

    protected Transform GetControlTrans(Define _enum)
    {
        int uiIndex = Convert.ToInt32(_enum);
        Transform trans = array_useUITrans[uiIndex];

        if (trans == null)
        {
            string format = string.Format("{0}는 null입니다.", trans);
            Debug.LogError(format);
        }

        return trans;
    }

    protected GameObject GetControlObject(Enum _enum)
    {
        int uiIndex = Convert.ToInt32(_enum);
        Transform trans = array_useUITrans[uiIndex];

        if (trans == null)
        {
            string format = string.Format("{0}는 null입니다.", trans);
            Debug.LogError(format);
        }

        return trans.gameObject;
    }

    protected T GetControlComponent<T>(Enum _enum)
    {
        int uiIndex = Convert.ToInt32(_enum);
        Transform trans = array_useUITrans[uiIndex];

        if (trans == null)
        {
            string format = string.Format("{0}는 null입니다.", trans);
            Debug.LogError(format);
        }

        T component = trans.GetComponent<T>();
        if(component == null)
        {
            Debug.LogError(string.Format("{0}의 {1} 컴포넌트는 존재하지 않습니다.", component.ToString(), typeof(T).Name));
        }

        return component;
    }

    protected void SetActiveControl(Enum _enum, bool _enable)
    {
        GameObject obj = GetControlObject(_enum);

        obj.SetActive(_enable);
    }

    protected void SetImageControl(Enum _enum, string _spritePath, int _spriteIndex)
    {
        Sprite[] arr_sprite = Resources.LoadAll<Sprite>(_spritePath);
        if (arr_sprite == null)
        {
            string format = string.Format("{0} 경로의 리소스를 찾을 수 없습니다.", _spritePath);
            Debug.LogError(format);
            return;
        }

        GameObject obj = GetControlObject(_enum);
        Image image = obj.GetComponent<Image>();
        if (image == null)
        {
            image = obj.AddComponent<Image>();
        }

        image.sprite = arr_sprite[_spriteIndex];
        Debug.LogWarning("IndexTest필요");
    }

    protected void SetTextControl(Enum _enum, string _text)
    {
        GameObject obj = GetControlObject(_enum);
        TMP_Text text = obj.GetComponent<TMP_Text>();
        if (text == null)
        {
            text = obj.AddComponent<TMP_Text>();
        }

        text.text = _text;
    }
}
