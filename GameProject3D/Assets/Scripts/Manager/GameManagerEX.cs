using System;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("Managers ���� : �Ϲ� Ŭ�������� ����� �� �����ϴ�. Managers�� �̿��� �ּ���.")]
public class GameManagerEX : BaseManager
{
    //
    public bool IsGamePlay
    {
        get
        {
            bool isWorldScene = Managers.Scene.currentSceneType == Define.Scene.WorldScene;
            bool isSpawnPlayer = gamePlayer != null;
            bool isLivePlayer = gamePlayer != null && gamePlayer.baseStateType != Define.BaseState.Die;

            //
            bool isGamePlay = isWorldScene && isSpawnPlayer && isLivePlayer;
            if (isGamePlay)
            {
                return true;
            }
            else
            {
                Debug.Log("Failed : ������ �÷����� �� �����ϴ�.");
                return false;
            }

        }
    }
    bool isPaused = false; //���� Ȱ��ȭ ���� ���� ����

    //
    GamePlayer gamePlayer = null;
    public IControllHandler playerCtrl
    {
        get
        {
            if (!Managers.Game.IsGamePlay)
            {
                Debug.LogWarning("Failed : ");
                return null;
            }
            return gamePlayer;
        }
    }
    #region Override

    protected override void InitDataProcess() { }
    protected override void ResetDataProcess()
    {
        gamePlayer = null;
    }

    #endregion Override

    #region Player

    public void SetPalyer(GamePlayer pGamePlayer)
    {
        gamePlayer = pGamePlayer;
    }
    public void SetPlayerRespawn()
    {
        gamePlayer = null;

        // Respawn
        //Managers.Spawn.SpawnCharacter(Managers.User.SpawnerID);
        Managers.User.UpdateUserData();
        Managers.Scene.LoadScene<WorldScene>(); // WorldScene�� ��ε� �մϴ�.
    }

    #endregion Player

    public ITargetHandler GetTargetCharacter(GameObject pTarget)
    {
        if(pTarget == null)
        {
            Debug.LogWarning("Falied : ");
            return null;
        }

        BaseCharacter baseCharacter = pTarget.GetComponent<BaseCharacter>();
        if (baseCharacter == null)
        {
            Debug.LogWarning("Falied : ");
            return null;
        }

        return baseCharacter;
    }

    void OnApplicationPause(bool pause)
    {
        if (pause) // ���� ��Ȱ��ȭ �Ǿ��� �� ó��
        {
            isPaused = true;
            if (Managers.Scene.currentSceneType == Define.Scene.None)
                Managers.User.UpdateUserData();
        }
        else // ���� Ȱ��ȭ �Ǿ��� �� ó��
        {
            if (isPaused)
            {
                isPaused = false;
            }
        }
    }

    [Obsolete("�׽�Ʈ ��")]
    void OnApplicationQuit() // ���� ���� �� �� ó��
    {
        if (Managers.Scene.currentSceneType == Define.Scene.WorldScene)
            Managers.User.UpdateUserData(); //Managers.User.SaveUserData();
    }
}