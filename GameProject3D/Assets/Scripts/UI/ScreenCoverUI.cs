using System.Collections;
using UnityEngine;
using UnityEngine.UI;

class ScreenCoverUI : BaseUI
{
    enum Control
    {
        /// <sammary>
        /// ScreenCoverUI
        /// </sammary>

        // UIPosition

        // Object
        SampleUI_Object_TuchBlocking, SampleUI_Object_LoadingData,

        // Button

        // Image

        // Text

    }

    public enum ScreenCoverType
    {
        TuchBlocking,
        LoadingData,
    }

    protected override void BindControls()
    {
        base.BindControls();

        // Control�� �����մϴ�.
        BindControl<Control>();
    }

    // Event�� �����մϴ�.
    protected override void BindEvents()
    {
        base.BindEvents();
        //BindEventControl<Button>(Control., OnClick_);
    }

    // UIPanel�� ������ �� �ʱ�ȭ�ϴ� �Լ��Դϴ�.
    protected override void InitUI()
    {
        base.InitUI();
    }

    // Open�� �� ������ ���μ����Դϴ�.
    protected override void OpenUIProcess()
    {
        //yield return null;
        
        //
        UpdateScreenCoverUI(ScreenCoverType.TuchBlocking);
    }

    // Close�� �� ������ ���μ����Դϴ�.
    protected override void CloseUIProcess()
    {
        //yield return null;

        //
    }

    //private void Update()
    //{
    //}

    #region Button

    //void OnClick_()
    //{
    //    Debug.LogWarning("���� ���� �� �Դϴ�.");
    //}

    #endregion Button

    #region ScreenCoverUI
    
    public void SetScreeConverUI(ScreenCoverType _screenCoverType)
    {
        UpdateScreenCoverUI(_screenCoverType);
    }

    void UpdateScreenCoverUI(ScreenCoverType _screenCoverType)
    {
        SetActiveControl(Control.SampleUI_Object_TuchBlocking, _screenCoverType == ScreenCoverType.TuchBlocking);
        SetActiveControl(Control.SampleUI_Object_LoadingData, _screenCoverType == ScreenCoverType.LoadingData);
    }

    #endregion ScreenCoverUI

    #region SampleUI_Panel_

    //void Open_SampleUI_Panel_()
    //{
    //    SetActiveControl(Control.SampleUI_Panel_, true);
    //}

    //void Close_SampleUI_Panel_()
    //{
    //    SetActiveControl(Control.SampleUI_Panel_, false);
    //}

    #endregion SampleUI_Panel_
}
