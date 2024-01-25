using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config
{
    #region User

    public static readonly string user_tableName = "UserData";

    #endregion User

    #region GamePlayer

    public static readonly int gamePlayer_spawnerID = 1;
    public static readonly Define.Prefabs gamePlayer_prefabType = Define.Prefabs.Character;
    public static readonly int gamePlayer_characterID = 1;
    public static readonly int gamePlayer_levelUpCount = 1;
    public static readonly int gamePlayer_expValue = 0;
    public static readonly int gamePlayer_coinValue = 1000;
    public static readonly int gamePlayer_hpValue = 300;
    public static readonly int gamePlayer_weaponID = 0;

    #endregion GamePlayer

    #region Camera

    //public static readonly Vector3 camera_deltaPos = new Vector3(0.0f, 6.0f, -5.0f);
    public static readonly Vector3 camera_deltaPos = new Vector3(0.0f, 8.0f, -5.0f);

    #endregion Camera

    #region UI/GUI

    public static readonly string ui_uiStorageName = "@UIStorage";
    //public static readonly string gui_controller = "@Controller";

    #endregion UI/GUI
}