using System;
using System.Collections;
using UnityEngine;

[Obsolete("Managers ���� : �Ϲ� Ŭ�������� ����� �� �����ϴ�. Managers�� �̿��� �ּ���.")]
public class LogInManager : BaseManager
{
    public enum LogInProcessType
    {
        None,
        InitLogIn,
        UpdateNickname,
        AccountAuth,
        UserLogIn,
        UserLogOut,
    }

    public enum AccountType
    {
        None,
        Guest,
        Google
    }

    public LogInProcessType currLogInProcessType { get; private set; } = LogInProcessType.None;
    public AccountType currAccountType { get; private set; } = AccountType.None;
    public bool isDone { get; private set; } = false;

    #region Override

    protected override void InitDataProcess() { }
    protected override void ResetDataProcess() { }

    #endregion Override

    public void InitLogInState()
    {
        currLogInProcessType = LogInProcessType.InitLogIn;
        currAccountType = AccountType.None;

        isDone = Managers.Backend.TokenLogin();
        if (isDone)
        {
            string nickname = Managers.Backend.GetNickname();
            isDone = CheckNickname(nickname);
            if (isDone)
            {
                currLogInProcessType = LogInProcessType.AccountAuth;
            }
            else
            {
                currLogInProcessType = LogInProcessType.UpdateNickname;
            }
        }
        else
        {
            currLogInProcessType = LogInProcessType.UserLogOut;
        }

        isDone = false;
    }

    bool CheckNickname(string _nickname)
    {
        bool nicknameAble = string.IsNullOrEmpty(_nickname) == false; //�г����� ���� ��� true

        return nicknameAble;
    }

    public bool SetSignUp(AccountType pAccountType)
    {
        AccountType selectAccountType = pAccountType;
        isDone = false;

        switch (selectAccountType)
        {
            case AccountType.Guest:
                {
                    isDone = Managers.Backend.GuestLogIn();
                    currAccountType = GetAccountType();
                }
                break;

            case AccountType.Google:
                {
                    if (Application.platform != RuntimePlatform.Android)
                    {
                        Debug.LogWarning("����� �� ���� ����Դϴ�.");
                        return isDone;
                    }

                    isDone = Managers.Backend.CheckFederationAccount();

                    if (isDone)
                    {
                        isDone = Managers.Backend.AuthorizeFederation();
                    }
                }
                break;

            default:
                Debug.LogError("���� ó�� �ʿ�");
                break;
        }

        return isDone;
    }

    public bool SetUpdateNickname(string _updateNickname)
    {
        isDone = Managers.Backend.CreateNickname(_updateNickname);

        return isDone;
    }

    public void SetUserLogIn()
    {
        currLogInProcessType = LogInProcessType.UserLogIn;
        currAccountType = GetAccountType();

        isDone = true;
    }

    public void SetUserLogOut()
    {
        switch (currAccountType)
        {
            case AccountType.Guest:
                {
                    isDone = Managers.Backend.SignOut();

                    if(isDone)
                    {
                        Managers.Backend.DeleteGuestInfo();
                    }
                }
                break;

            case AccountType.Google:
                {
                    isDone = Managers.Backend.LogOut();
                }
                break;

            default:
                Debug.LogError("���� ó�� �ʿ�");
                break;
        }
    }

    private AccountType GetAccountType()
    {
        string subscriptionType = Managers.Backend.GetSubscriptionType();
        switch (subscriptionType)
        {
            case "customSignUp":
                {
                    return AccountType.Guest;
                }

            case "google":
                {
                    return AccountType.Google;
                }

            case "":
                {
                    return AccountType.None;
                }

            default:
                {
                    Debug.LogError("�� �� ���� ���� Ÿ�� : " + subscriptionType);

                    return AccountType.None;
                }
        }
    }
}