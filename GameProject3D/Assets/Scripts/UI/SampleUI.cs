using UnityEngine;

class SampleUI : BaseUI
{
    enum Control
    {
        /// <sammary>
        /// SampleUI
        /// </sammary>

        // UIPosition

        // Object

        // Button

        // Image

        // Text

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
    }

    // Close�� �� ������ ���μ����Դϴ�.
    protected override void CloseUIProcess()
    {
        //yield return null;

        //
    }

    //void Update()
    //{
    //}

    #region Button

    //void OnClick_()
    //{
    //    Debug.LogWarning("���� ���� �� �Դϴ�.");
    //}

    #endregion Button

    #region SampleUI

    #endregion SampleUI

    #region SampleUI_Panel_

    //void Open_SampleUI_Panel_()
    //{
    //    SetActiveControl(Control.SampleUI_Panel_, true);
    //}

    //void Close_SampleUI_Panel_()
    //{
    //    SetActiveControl(Control.SampleUI_Panel_, false);
    //}

    //void Update_SampleUI_Panel_()
    //{

    //}

    #endregion SampleUI_Panel_
}
