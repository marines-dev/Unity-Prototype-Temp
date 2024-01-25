using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

class TitleUI : BaseUI
{
    enum Controls
    {
        /// <sammary>
        /// TitleUI
        /// </sammary>

        // UIPosition

        // Object
        TitleUI_Object_InitDataProcess, TitleUI_Object_LoginProcess, TitleUI_Object_LoadDataProcess,
        TitleUI_Object_Logout, TitleUI_Object_Login,

        // Button
        TitleUI_Button_Guest, TitleUI_Button_Google, TitleUI_Button_GameStart,

        // Slider
        TitleUI_Slider_LoadData,

        // Image

        // Text

        /// <sammary>
        /// TitleUI_Panel_PatchPopup
        /// </sammary>
        TitleUI_Panel_PatchPopup,

        // UIPosition

        // Object

        // Button
        PatchPopup_Button_Confirm, PatchPopup_Button_Cancel,

        // Image

        // Text


        /// <sammary>
        /// TitleUI_Panel_SignUpPopup
        /// </sammary>
        TitleUI_Panel_SignUpPopup,

        // UIPosition

        // Object
        SignUpPopup_Object_Guest, SignUpPopup_Object_Google,

        // Button
        SignUpPopup_Button_GuestConfirm, SignUpPopup_Button_GuestCancel, SignUpPopup_Button_GoogleConfirm, SignUpPopup_Button_GoogleCancel,

        // Image

        // Text

        /// <sammary>
        /// TitleUI_Panel_NicknamePopup
        /// </sammary>
        TitleUI_Panel_NicknamePopup,

        // UIPosition

        // Object

        // Button
        NicknamePopup_Button_Confirm,

        // InputField
        NicknamePopup_InputField_Nickname,

        // Image

        // Text

        /// <sammary>
        /// TitleUI_Panel_GuestLogoutPopup
        /// </sammary>
        TitleUI_Panel_GuestLogoutPopup,

        // UIPosition

        // Object

        // Button
        GuestLogoutPopup_Button_Confirm, GuestLogoutPopup_Button_Cancel,

        // Image

        // Text

        /// <sammary>
        /// TitleUI_Panel_DebugTestPopup
        /// </sammary>
        TitleUI_Panel_DebugTestPopup,

        // UIPosition

        // Object
        DebugTestPopup_Object_SelectAble, DebugTestPopup_Object_SelectDesable,
        DebugTestPopup_Object_DestPopup, DebugTestPopup_Object_TestLogout,

        // Button
        DebugTestPopup_Button_Select, DebugTestPopup_Button_TestLogout,

        // Image

        // Text
        DebugTestPopup_Text_Desc,
    }

    public LogInManager.AccountType selectAccountType { get; private set; } = LogInManager.AccountType.None;
    public string inputNickname { get; private set; } = string.Empty;
    bool debugSelectAble = true;

    public Action onLogInState { get; private set; } = null;

    protected override void BindControls()
    {
        base.BindControls();

        // ControlUI를 연결합니다.
        BindControl<Controls>();
    }

    // Event를 연결합니다.
    protected override void BindEvents()
    {
        base.BindEvents();

        BindEventControl<Button>(Controls.TitleUI_Button_Guest, OnClick_TitleUI_Button_Guest);
        BindEventControl<Button>(Controls.TitleUI_Button_Google, OnClick_TitleUI_Button_Google);
        BindEventControl<Button>(Controls.TitleUI_Button_GameStart, OnClick_TitleUI_Button_GameStart);
        BindEventControl<Button>(Controls.PatchPopup_Button_Confirm, OnClick_PatchPopup_Button_Confirm);
        BindEventControl<Button>(Controls.PatchPopup_Button_Cancel, OnClick_PatchPopup_Button_Cancel);
        BindEventControl<Button>(Controls.SignUpPopup_Button_GuestConfirm, OnClick_SignUpPopup_Button_GuestConfirm);
        BindEventControl<Button>(Controls.SignUpPopup_Button_GuestCancel, OnClick_SignUpPopup_Button_GuestCancel);
        BindEventControl<Button>(Controls.SignUpPopup_Button_GoogleConfirm, OnClick_SignUpPopup_Button_GoogleConfirm);
        BindEventControl<Button>(Controls.SignUpPopup_Button_GoogleCancel, OnClick_SignUpPopup_Button_GoogleCancel);
        BindEventControl<Button>(Controls.NicknamePopup_Button_Confirm, OnClick_NicknamePopup_Button_Confirm);
        BindEventControl<Button>(Controls.GuestLogoutPopup_Button_Confirm, OnClick_GuestLogoutPopup_Button_Confirm);
        BindEventControl<Button>(Controls.GuestLogoutPopup_Button_Cancel, OnClick_GuestLogoutPopup_Button_Cancel);
        BindEventControl<Button>(Controls.DebugTestPopup_Button_Select, OnClick_DebugTestPopup_Button_Select);
        BindEventControl<Button>(Controls.DebugTestPopup_Button_TestLogout, OnClick_DebugTestPopup_Button_TestLogout);
    }

    // UIPanel을 생성할 때 초기화하는 함수입니다.
    protected override void InitUI()
    {
        base.InitUI();
    }

    // Open할 때 실행할 프로세스입니다.
    protected override void OpenUIProcess()
    {
        selectAccountType = LogInManager.AccountType.None;
        debugSelectAble = true;
        onLogInState = null;

        // Close
        Close_TitleUI_Panel_SignUpPopup();
        Close_TitleUI_Panel_NicknamePopup();
        Close_TitleUI_Panel_GuestLogoutPopup();
        Close_TitleUI_Panel_DebugTestPopup();

        UpdateTitleUI(TitleScene.TitleProcessType.Init);
        Open_TitleUI_Panel_DebugTestPopup();
    }

    // Close할 때 실행할 프로세스입니다.
    protected override void CloseUIProcess()
    {
        //yield return null;
    }

    #region Button

    void OnClick_TitleUI_Button_Guest()
    {
        Open_TitleUI_Panel_SignUpPopup(LogInManager.AccountType.Guest);
    }

    void OnClick_TitleUI_Button_Google()
    {
        Debug.LogWarning("개발 진행 중 입니다.");

        //selectAccountType = LoginManager.AccountType.Guest;

        //if (onLogInState != null)
        //    onLogInState();
    }

    void OnClick_TitleUI_Button_GameStart()
    {
        if (onLogInState != null)
            onLogInState();
    }

    void OnClick_PatchPopup_Button_Confirm()
    {
        Debug.LogWarning("개발 진행 중 입니다.");

    }

    void OnClick_PatchPopup_Button_Cancel()
    {
        Debug.LogWarning("개발 진행 중 입니다.");

    }

    void OnClick_SignUpPopup_Button_GuestConfirm()
    {
        selectAccountType = LogInManager.AccountType.Guest;

        if (onLogInState != null)
            onLogInState();

        Close_TitleUI_Panel_SignUpPopup();
    }

    void OnClick_SignUpPopup_Button_GuestCancel()
    {
        //selectAccountType = LoginManager.AccountType.None;

        Close_TitleUI_Panel_SignUpPopup();
    }

    void OnClick_SignUpPopup_Button_GoogleConfirm()
    {
        Close_TitleUI_Panel_SignUpPopup();
    }

    void OnClick_SignUpPopup_Button_GoogleCancel()
    {
        //selectAccountType = LoginManager.AccountType.None;

        Close_TitleUI_Panel_SignUpPopup();
    }

    void OnClick_NicknamePopup_Button_Confirm()
    {
        Debug.LogWarning("UI 구조 수정 필요");
        TMP_InputField inputField = GetControlObject(Controls.NicknamePopup_InputField_Nickname).GetComponent<TMP_InputField>();
        if (inputField == null)
            return;

        inputNickname = inputField.text;

        if (onLogInState != null)
            onLogInState();

        Close_TitleUI_Panel_NicknamePopup();
    }

    void OnClick_GuestLogoutPopup_Button_Confirm()
    {
        onLogInState.Invoke();

        Close_TitleUI_Panel_GuestLogoutPopup();
    }

    void OnClick_GuestLogoutPopup_Button_Cancel()
    {
        Close_TitleUI_Panel_GuestLogoutPopup();
    }

    void OnClick_DebugTestPopup_Button_Select()
    {
        debugSelectAble = !debugSelectAble;

        Update_TitleUI_Panel_DebugTestPopup(debugSelectAble);
    }

    void OnClick_DebugTestPopup_Button_TestLogout()
    {
        switch (Managers.LogIn.currAccountType)
        {
            case LogInManager.AccountType.Guest:
                {
                    Open_TitleUI_Panel_GuestLogoutPopup();
                }
                break;

            case LogInManager.AccountType.Google:
                {
                    Debug.LogWarning("개발 진행 중 입니다.");
                    //LoginManager.Instance.SetLogOut(LoginManager.AccountType.Google);
                }
                break;

            default:
                Debug.LogError("예외 처리 필요");
                break;
        }
    }

    #endregion Button

    #region TitleUI

    public void Set_TitleUI(TitleScene.TitleProcessType _titleProcessType)
    {
        UpdateTitleUI(_titleProcessType);

        if (_titleProcessType == TitleScene.TitleProcessType.LogIn)
        {
            switch (Managers.LogIn.currLogInProcessType)
            {
                //case LoginManager.LoginProcessType.UserLogOut:
                //    {

                //    }
                //    break;

                //case LoginManager.LoginProcessType.UserLogIn:
                //    {
                //    }
                //    break;

                case LogInManager.LogInProcessType.UpdateNickname:
                    {
                        Open_TitleUI_Panel_NicknamePopup();
                    }
                    break;
            }
        }
    }

    public void Set_OnLogInState(Action _onLogInState)
    {
        onLogInState = _onLogInState;
    }

    void UpdateTitleUI(TitleScene.TitleProcessType _titleProcessType)
    {
        SetActiveControl(Controls.TitleUI_Object_InitDataProcess, _titleProcessType == TitleScene.TitleProcessType.Init);
        SetActiveControl(Controls.TitleUI_Object_LoginProcess, _titleProcessType == TitleScene.TitleProcessType.LogIn);
        SetActiveControl(Controls.TitleUI_Object_LoadDataProcess, _titleProcessType == TitleScene.TitleProcessType.LoadUserData); //임시

        if(_titleProcessType == TitleScene.TitleProcessType.LogIn)
        {
            SetActiveControl(Controls.TitleUI_Object_Login, Managers.LogIn.currLogInProcessType == LogInManager.LogInProcessType.AccountAuth);
            SetActiveControl(Controls.TitleUI_Object_Logout, Managers.LogIn.currLogInProcessType == LogInManager.LogInProcessType.UserLogOut);
        }
    }

    #endregion TitleUI

    #region TitleUI_Panel_SignUpPopup

    void Open_TitleUI_Panel_SignUpPopup(LogInManager.AccountType _accountType)
    {
        SetActiveControl(Controls.TitleUI_Panel_SignUpPopup, true);

        selectAccountType = LogInManager.AccountType.None;
        Update_TitleUI_Panel_SignUpPopup(_accountType);
    }

    void Close_TitleUI_Panel_SignUpPopup()
    {
        SetActiveControl(Controls.TitleUI_Panel_SignUpPopup, false);
    }

    void Update_TitleUI_Panel_SignUpPopup(LogInManager.AccountType _accountType)
    {
        SetActiveControl(Controls.SignUpPopup_Object_Guest, _accountType == LogInManager.AccountType.Guest);
        SetActiveControl(Controls.SignUpPopup_Object_Google, _accountType == LogInManager.AccountType.Google);
    }

    #endregion TitleUI_Panel_SignUpPopup

    #region TitleUI_Panel_NicknamePopup

    void Open_TitleUI_Panel_NicknamePopup()
    {
        SetActiveControl(Controls.TitleUI_Panel_NicknamePopup, true);

        Update_TitleUI_Panel_NicknamePopup();
    }

    void Close_TitleUI_Panel_NicknamePopup()
    {
        SetActiveControl(Controls.TitleUI_Panel_NicknamePopup, false);
    }

    void Update_TitleUI_Panel_NicknamePopup()
    {
        TMP_InputField inputField = GetControlComponent<TMP_InputField>(Controls.NicknamePopup_InputField_Nickname);
        if (inputField == null)
            return;

        inputField.text = string.Empty;
        inputNickname = inputField.text;
    }

    #endregion TitleUI_Panel_NicknamePopup

    #region TitleUI_Panel_GuestLogoutPopup

    void Open_TitleUI_Panel_GuestLogoutPopup()
    {
        SetActiveControl(Controls.TitleUI_Panel_GuestLogoutPopup, true);

        Update_TitleUI_Panel_GuestLogoutPopup();
    }

    void Close_TitleUI_Panel_GuestLogoutPopup()
    {
        SetActiveControl(Controls.TitleUI_Panel_GuestLogoutPopup, false);
    }

    void Update_TitleUI_Panel_GuestLogoutPopup()
    {

    }

    #endregion TitleUI_Panel_GuestLogoutPopup

    #region TitleUI_Panel_DebugTestPopup

    void Open_TitleUI_Panel_DebugTestPopup()
    {
        SetActiveControl(Controls.TitleUI_Panel_DebugTestPopup, true);

        debugSelectAble = true;
        Update_TitleUI_Panel_DebugTestPopup(debugSelectAble);
    }

    void Close_TitleUI_Panel_DebugTestPopup()
    {
        SetActiveControl(Controls.TitleUI_Panel_DebugTestPopup, false);
    }

    void Update_TitleUI_Panel_DebugTestPopup(bool _debugSelectAble)
    {
        // Select
        SetActiveControl(Controls.DebugTestPopup_Object_SelectAble, _debugSelectAble == false);
        SetActiveControl(Controls.DebugTestPopup_Object_SelectDesable, _debugSelectAble);
        SetActiveControl(Controls.DebugTestPopup_Object_DestPopup, _debugSelectAble);
        Set_DebugLog();
    }

    [Obsolete("테스트")]
    public void Set_DebugLog(string _logMessage = "")
    {
        if (debugSelectAble == false)
            return;

        SetTextControl(Controls.DebugTestPopup_Text_Desc, _logMessage);
        SetActiveControl(Controls.DebugTestPopup_Object_TestLogout, Managers.LogIn.currLogInProcessType == LogInManager.LogInProcessType.UserLogIn);

        string format = string.Format("[DebugLog]\n{0}", _logMessage);
        Debug.Log(format);
    }

    #endregion TitleUI_Panel_DebugTestPopup
}
