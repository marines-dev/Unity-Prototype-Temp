using UnityEngine;

class LoadingUI : BaseUI
{
    enum Control
    {
        /// <sammary>
        /// LoadingUI
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

    #region LoadingUI

    #endregion LoadingUI
}
