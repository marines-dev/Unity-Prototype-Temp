using System;
using UnityEngine;
using BackEnd;

[Obsolete("Managers ���� : �Ϲ� Ŭ�������� ����� �� �����ϴ�. Managers�� �̿��� �ּ���.")]
public class BackendManager : BaseManager
{
    //
    BackendReturnObject bro = null;

    // Data
    private string gameDataRowInDate = string.Empty;

    #region Override

    protected override void InitDataProcess() { }
    protected override void ResetDataProcess()
    {
        gameDataRowInDate = string.Empty;
    }

    #endregion Override

    #region Init

    public void InitBackendSDK()
    {
        bro = Backend.Initialize(true);

        if (bro.IsSuccess())
        {
            Debug.Log($"Success: InitBackendSDK - {bro}");
        }
        else
        {
            Debug.LogError($"Failed : InitBackendSDK - {bro}");
        }
    }

    #endregion Init

    #region LogIn

    // LogIn
    public bool TokenLogin()
    {
        //string id = Backend.BMember.GetGuestID();
        //Debug.Log("���� ��⿡ ����� ���̵� :" + id);

        //bro = Backend.BMember.CheckUserInBackend("federationToken", FederationType.Google);
        //Debug.Log("federationToken : " + bro);

        bro = Backend.BMember.LoginWithTheBackendToken();

        if (bro.IsSuccess())
        {
            Debug.Log($"Success: TokenLogin - {bro}");
            return true;
        }
        else
        {
            Debug.Log($"Failed : TokenLogin - {bro}");
            return false;
        }
    }

    public bool GuestLogIn()
    {
        bro = Backend.BMember.GuestLogin();
        if (bro.IsSuccess())
        {
            Debug.Log($"Success: GuestLogIn - {bro}");
            return true;
        }
        else
        {
            Managers.Backend.DeleteGuestInfo();

            Debug.LogError($"Failed : GuestLogIn - {bro}");
            return false;
        }
    }

    // �ڳ� ������ ȹ���� ���� ��ū���� ȸ������ �Ǵ� �α���
    public bool AuthorizeFederation()
    {
        bool isSuccess = false;

#if UNITY_ANDROID
        bro = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
        isSuccess = bro.IsSuccess();
        if (bro.IsSuccess())
        {
            Debug.Log($"Success: AuthorizeFederation - {bro}");
        }
        else
        {
            Debug.LogError($"Failed : AuthorizeFederation - {bro}");
        }
#endif

        return isSuccess;
    }

    // LogOut
    public bool CheckFederationAccount()
    {
        bool isSuccess = false;

#if UNITY_ANDROID
        bro = Backend.BMember.CheckUserInBackend(GetTokens(), FederationType.Google);
        isSuccess = bro.IsSuccess();
        if (isSuccess)
        {
            Debug.Log($"Success: CheckFederationAccount - {bro}");
        }
        else
        {
            Debug.LogError($"Failed : CheckFederationAccount - {bro}");

            OpenSignUpPopupObject(selectAccoutType);
        }
#endif

        return isSuccess;
    }

    public bool LogOut()
    {
        bro = Backend.BMember.Logout();
        bool isSuccess = bro.IsSuccess();
        if (bro.IsSuccess())
        {
            Debug.Log($"Success: FederationLogOut - {bro}");
        }
        else
        {
            Debug.LogError($"Failed : FederationLogOut - {bro}");
        }

        return isSuccess;
    }

    public bool SignOut()
    {
        bro = Backend.BMember.WithdrawAccount();
        bool isSuccess = bro.IsSuccess();
        if (bro.IsSuccess())
        {
            Debug.Log($"Success: SignOut - {bro}");
        }
        else
        {
            Debug.LogError($"Failed : SignOut - {bro}");
        }

        return isSuccess;
    }

    public void DeleteGuestInfo()
    {
        Backend.BMember.DeleteGuestInfo();
    }

    // Nickname
    public bool CreateNickname(string _nickname)
    {
        Debug.Log("Input Nickname : " + _nickname);

        bro = Backend.BMember.CreateNickname(_nickname);
        bool isSuccess = bro.IsSuccess();
        if (isSuccess)
        {
            Debug.Log($"Success: CreateNickname - {bro}");
        }
        else
        {
            Debug.LogError($"Failed : CreateNickname - {bro}");
        }

        return isSuccess;
    }

    public string GetInData()
    {
        return Backend.UserInDate;
    }

    public string GetNickname()
    {
        return Backend.UserNickName;
    }

    public string GetSubscriptionType()
    {
        bro = Backend.BMember.GetUserInfo();
        return bro.GetReturnValuetoJSON()["row"]["subscriptionType"].ToString();
    }

    #endregion LogIn

    #region Data

    public LitJson.JsonData LoadBackendData(string _table)
    {
        Debug.Log("���� ���� ��ȸ �Լ��� ȣ���մϴ�.");
        var bro = Backend.GameData.GetMyData(_table, new Where()); // _table = "USER_DATA"
        if (bro.IsSuccess())
        {
            Debug.Log("���� ���� ��ȸ�� �����߽��ϴ�. : " + bro);
            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json���� ���ϵ� �����͸� �޾ƿɴϴ�.  

            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            }
            else
            {
                gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //�ҷ��� ���������� �������Դϴ�.  
                return gameDataJson;
            }
        }
        else
        {
            Debug.LogError("���� ���� ��ȸ�� �����߽��ϴ�. : " + bro);
        }

        return null;
    }

    public void SaveBackendData(string _table, ref Param _param)
    {
        Debug.LogWarning("���� ������Ʈ ��Ͽ� �ش� �����͵��� �߰��մϴ�.");
        // �������� ������ ������ ��û�մϴ�.
        var bro = Backend.GameData.Insert(_table, _param);

        if (bro.IsSuccess())
        {
            Debug.Log("�������� ������ ���Կ� �����߽��ϴ�. : " + bro);

            //������ ���������� �������Դϴ�.  
            gameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Debug.LogError("�������� ������ ���Կ� �����߽��ϴ�. : " + bro);
        }
    }

    public void UpdateBackendData(string _table, ref Param _param)
    {
        BackendReturnObject bro = null;

        if (string.IsNullOrEmpty(gameDataRowInDate))
        {
            Debug.Log("�� ���� �ֽ� �������� ������ ������ ��û�մϴ�.");

            bro = Backend.GameData.Update(_table, new Where(), _param);
        }
        else
        {
            Debug.Log($"{gameDataRowInDate}�� �������� ������ ������ ��û�մϴ�.");

            bro = Backend.GameData.UpdateV2(_table, gameDataRowInDate, Backend.UserInDate, _param);
        }

        if (bro.IsSuccess())
        {
            Debug.Log("�������� ������ ������ �����߽��ϴ�. : " + bro);
        }
        else
        {
            Debug.LogError("�������� ������ ������ �����߽��ϴ�. : " + bro);
        }
    }

    #endregion Data
}

