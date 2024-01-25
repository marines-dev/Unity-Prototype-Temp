using System;
using System.Collections;
using UnityEngine;

[Obsolete("Managers ���� : �Ϲ� Ŭ�������� ����� �� �����ϴ�. Managers�� �̿��� �ּ���.")]
public class GUIManager : BaseManager
{
    Controller controller_go = null;
    //Controller controller_go
    //{
    //    get
    //    {
    //        if (controller_go_pro == null)
    //        {
    //            CreateController();
    //        }
    //        return controller_go_pro;
    //    }
    //}

    #region Override

    protected override void InitDataProcess() { }
    protected override void ResetDataProcess()
    {
        ExitJoystickController();
        DestroyController();
    }

    #endregion Override

    #region WorldScene

    public void SetWorldSceneController()
    {
        if(Managers.Game.playerCtrl == null)
        {
            Debug.LogWarning("Failed : ");
            return;
        }   
        
        CreateController();
        //SetJoystickHandler();
    }

    [Obsolete("�÷����� �з�")]
    //public void SetJoystickHandler()
    //{
    //    //
    //    IJoystickHandler JoystickHandler = controller_go;
    //}

    public void StartJoystickController()
    {
        if (Managers.Game.IsGamePlay == false)
        {
            Debug.Log($"Failed : {typeof(Controller).Name}�� ������ �� �����ϴ�.");
            return;
        }

        IJoystickHandler JoystickHandler = controller_go;
        JoystickHandler.StartController();
    }

    public void ExitJoystickController()
    {
        if(controller_go == null)
        {
            Debug.Log($"Failed : ������ {typeof(Controller).Name}�� �����ϴ�.");
            return;
        }

        IJoystickHandler JoystickHandler = controller_go;
        JoystickHandler.ExitController();
    }

    #endregion WorldScene

    #region Load

    void CreateController()
    {
        if (controller_go != null)
        {
            Debug.Log("");
            return;
        }

        //DestroyController();
        string name = $"@{typeof(Controller).Name}";
        controller_go = Managers.Resource.CreateComponentObject<Controller>(name, null);
    }

    void DestroyController()
    {
        if (controller_go == null)
        {
            Debug.Log("");
            return;
        }

        Managers.Resource.DestroyGameObject(controller_go.gameObject);
        controller_go = null;
    }

    #endregion Load
}
