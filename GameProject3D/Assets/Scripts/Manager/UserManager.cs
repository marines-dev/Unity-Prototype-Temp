using System;
using System.Text;
using UnityEngine;
using BackEnd;

namespace ServerData
{
    public class UserData
    {
        // Player (�� 7��)
        public int spawnerID = 0;
        public Define.Prefabs prefabType = Define.Prefabs.None;
        public int characterID = 0;
        public int levelValue = 0;
        public int expValue = 0;
        public int coinValue = 0;
        public int hpValue = 0;
        public int weaponID = 0;
    }
}

[Obsolete("Managers ���� : �Ϲ� Ŭ�������� ����� �� �����ϴ�. Managers�� �̿��� �ּ���.")]
public class UserManager : BaseManager
{
    // Backend
    string InData
    {
        get
        {
            return Managers.Backend.GetInData();
        }
    }
    string NickName
    {
        get
        {
            return Managers.Backend.GetNickname();
        }
    }

    // ServerData.UserData
    ServerData.UserData userData = new ServerData.UserData();
    public int SpawnerID
    {
        get
        {
            return userData.spawnerID;
        }
    }
    public Define.Prefabs PrefabType
    {
        get
        {
            return userData.prefabType;
        }
    }
    public int CharacterID
    {
        get
        {
            return userData.characterID;
        }
    }
    public int LevelValue
    {
        get
        {
            return userData.levelValue;
        }
    }
    public int ExpValue
    {
        get
        {
            return userData.expValue;
        }
    }
    public int CoinValue
    {
        get
        {
            return userData.coinValue;
        }
    }
    public int HpValue
    {
        get
        {
            return userData.hpValue;
        }
    }
    public int WeaponID
    {
        get
        {
            return userData.weaponID;
        }
    }

    #region Override

    protected override void InitDataProcess() 
    {
        if(userData == null)
            userData = new ServerData.UserData();
    }

    protected override void ResetDataProcess() { }

    #endregion Override

    // Test : ������ ����� // Debug.Log(ToString())
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();

        // User
        result.AppendLine($"inData : {InData.ToString()}");
        result.AppendLine($"nickName : {NickName.ToString()}");

        // Player
        result.AppendLine($"spawnerID : {userData.spawnerID.ToString()}");
        result.AppendLine($"characterID : {userData.characterID.ToString()}");
        result.AppendLine($"currentLevel : {userData.levelValue.ToString()}");
        result.AppendLine($"currentExp : {userData.expValue.ToString()}");
        result.AppendLine($"currentCoin : {userData.coinValue.ToString()}");
        result.AppendLine($"currentHP : {userData.hpValue.ToString()}");
        result.AppendLine($"weaponID : {userData.weaponID.ToString()}");

        //result.AppendLine($"inventory");
        //foreach (var itemKey in test_inventory.Keys)
        //{
        //    result.AppendLine($"| {itemKey} : {test_inventory[itemKey]}��");
        //}
        //result.AppendLine($"equipment");
        //foreach (var equip in test_equipment)
        //{
        //    result.AppendLine($"| {equip}");
        //}

        return result.ToString();
    }

    //ServerData.UserData GetUserData() 
    //{
    //    ServerData.UserData _userData = new ServerData.UserData();

    //    // GamePlayer
    //    _userData.spawnerID = userData.spawnerID;
    //    _userData.prefabType = userData.prefabType;
    //    _userData.characterID = userData.characterID;
    //    _userData.levelValue = userData.levelValue;
    //    _userData.expValue = userData.expValue;
    //    _userData.coinValue = userData.coinValue;
    //    _userData.hpValue = userData.hpValue;

    //    return _userData; 
    //}

    /// <summary>
    // ������ Hp ���� �˻��Ͽ� ���� Hp ���ȿ� �����մϴ�. ��ȯ�� ������ �ٽ� ������ּ���.
    /// </summary>
    public int SetUserHP(int pHpValue)
    {
        if (pHpValue == userData.hpValue)
        {
            Debug.LogWarning($"Faild : �����Ϸ��� Exp({pHpValue}) ���� ������ ���� Exp({userData.hpValue})�� �����Ƿ� ������ �� �����ϴ�.");
            return userData.hpValue;
        }

        Table.Character.Data characterData = Managers.Table.GetTable<Table.Character>().GetTableData(userData.characterID);
        Table.Stat.Data statData = Managers.Table.GetTable<Table.Stat>().GetTableData(characterData.statID);
        if (pHpValue > statData.maxHp)
        {
            Debug.Log($"Warning : HP({pHpValue}) �� �ʰ��� MaxHP({statData.maxHp})���� �����մϴ�.");
            userData.hpValue  = statData.maxHp;
        }
        else if(pHpValue < 0)
        {
            Debug.Log($"Warning : HP({pHpValue}) �� �ʰ��� 0���� �����մϴ�.");
            userData.hpValue = 0;
        }
        else
        {
            userData.hpValue = pHpValue;
        }

        //
        UpdateUserData();

        //
        Debug.Log($"������ ����� hpValue�� {userData.hpValue}�Դϴ�.");
        return userData.hpValue;
    }

    /// <summary>
    // ������ Exp ���� �˻��Ͽ� ���� Exp ���ȿ� �����մϴ�. ��ȯ�� ������ �ٽ� ������ּ���.
    /// </summary>
    public int SetUserExp(int pExpValue)
    {
        if (pExpValue <= userData.expValue)
        {
            Debug.LogWarning($"Faild : �����Ϸ��� Exp({pExpValue}) ���� ������ ���� Exp({userData.expValue})�� �۰ų� �����Ƿ� ������ �� �����ϴ�.");
            return userData.expValue;
        }

        Table.Character.Data characterData = Managers.Table.GetTable<Table.Character>().GetTableData(userData.characterID);
        Table.Stat.Data statData = Managers.Table.GetTable<Table.Stat>().GetTableData(characterData.statID);
        if (pExpValue > statData.maxExp)
        {
            Debug.Log($"Warning : exp({pExpValue}) �� �ʰ��� MaxExp({statData.maxExp})���� �����մϴ�.");
            Debug.Log($"Warning : ������ ������ ������ �ּ���!");
            userData.expValue = statData.maxExp;
        }
        else if (pExpValue < 0)
        {
            Debug.Log($"Warning : exp({pExpValue}) �� �ʰ��� 0���� �����մϴ�.");
            userData.expValue = 0;
        }
        else
        {
            userData.expValue = pExpValue;
        }

        //
        UpdateUserData();

        //
        Debug.Log($"������ ����� Exp�� {userData.expValue}�Դϴ�.");
        return userData.expValue;
    }

    [Obsolete("�ӽ�")]
    public int SetUserLevelUp()
    {
        Table.Character.Data characterData = Managers.Table.GetTable<Table.Character>().GetTableData(userData.characterID);
        Table.Stat.Data statData = Managers.Table.GetTable<Table.Stat>().GetTableData(characterData.statID);
        if (userData.expValue < statData.maxExp)
        {
            Debug.LogWarning($"Failed : ������ Exp({userData.expValue})�� MaxExp({statData.maxExp})���� �۾� LevelUp�� �� �����ϴ�.");
            return userData.levelValue;
        }

        userData.levelValue += 1;
        userData.expValue = 0;

        //
        UpdateUserData();

        //
        Debug.Log($"������ ����� levelValue�� {userData.levelValue}�Դϴ�.");
        Debug.Log($"������ expValue�� {userData.expValue}���� �ʱ�ȭ�Ǿ����ϴ�. ����� ������ ������ �ּ���.");
        return userData.levelValue;
    }

    [Obsolete("�ӽ�")]
    public void SetUserWeaponID(int pWeaponID)
    {
        if (pWeaponID < 0) // Table ���� �˻� �ʿ�
        {
            Debug.LogWarning("Failed : ");
            return;
        }

        userData.weaponID = pWeaponID;

        //
        UpdateUserData();
    }

    //[Obsolete("�ӽ�")]
    //public void SetUserCoin(int pValue)
    //{
    //    //
    //    int coinValue = userData.coinValue + pValue;
    //    if (coinValue < 0)
    //    {
    //        return;
    //    }

    //    userData.coinValue = coinValue;

    //    //
    //    UpdateUserData();
    //}

    #region ServerLoad

    [Obsolete("�ӽ�")]
    public void CreateUserData()
    {
        // ServerData.UserData
        userData.spawnerID = Config.gamePlayer_spawnerID;
        userData.prefabType = Config.gamePlayer_prefabType;
        userData.characterID = Config.gamePlayer_characterID;
        userData.levelValue = Config.gamePlayer_levelUpCount;
        userData.expValue = Config.gamePlayer_expValue;
        userData.coinValue = Config.gamePlayer_coinValue;
        userData.hpValue = Config.gamePlayer_hpValue;
        userData.weaponID = Config.gamePlayer_weaponID;

        //
        SaveUserData();
    }

    [Obsolete("�׽�Ʈ �ʿ� : ���� ���� ��� Ȯ��")]
    public void SaveUserData() // Backend �����Ϳ� �����մϴ�.
    {
        if (userData == null)
        {
            Debug.LogError("Failed : �������� �ٿ�ްų� ���� ������ �����Ͱ� �������� �ʽ��ϴ�. Init Ȥ�� Load�� ���� �����͸� �������ּ���.");
            return;
        }

        Param param = new Param();

        // ServerData.UserData
        param.Add("spawnerID", userData.spawnerID.ToString());
        param.Add("prefabType", userData.prefabType.ToString());
        param.Add("characterID", userData.characterID.ToString());
        param.Add("levelUpCount", userData.levelValue.ToString());
        param.Add("expValue", userData.expValue.ToString());
        param.Add("coinValue", userData.coinValue.ToString());
        param.Add("hpValue", userData.hpValue.ToString());
        param.Add("weaponID", userData.weaponID.ToString());

        // Test
        //userData.info = "ģ�ߴ� ������ ȯ���Դϴ�.";
        //userData.equipment.Add("������ ����");
        //userData.inventory.Add("��������", 1);
        //Managers.Backend.SaveBackendData<string>("level", test_info);
        //Managers.Backend.SaveBackendData<Dictionary<string, int>>("level", test_inventory);
        //Managers.Backend.SaveBackendData<List<string>>("level", test_equipment);

        // SaveBackend
        Managers.Backend.SaveBackendData(Config.user_tableName, ref param);
    }

    [Obsolete("�׽�Ʈ �ʿ� : ���� ���� ��� Ȯ��")]
    public void UpdateUserData() // Backend �����Ϳ� �����մϴ�.
    {
        if (userData == null)
        {
            Debug.LogError("�������� �ٿ�ްų� ���� ������ �����Ͱ� �������� �ʽ��ϴ�. Init Ȥ�� Load�� ���� �����͸� �������ּ���.");
            return;
        }

        Param param = new Param();

        // ServerData.UserData
        param.Add("spawnerID", userData.spawnerID.ToString());
        param.Add("prefabType", userData.prefabType.ToString());
        param.Add("characterID", userData.characterID.ToString());
        param.Add("levelUpCount", userData.levelValue.ToString());
        param.Add("expValue", userData.expValue.ToString());
        param.Add("coinValue", userData.coinValue.ToString());
        param.Add("hpValue", userData.hpValue.ToString());
        param.Add("weaponID", userData.weaponID.ToString());

        // UpdateBackend
        Managers.Backend.UpdateBackendData(Config.user_tableName, ref param);
    }

    public bool LoadUserData() // Backend �����͸� �ҷ��ɴϴ�.
    {
        LitJson.JsonData gameDataJson = Managers.Backend.LoadBackendData(Config.user_tableName);
        if (gameDataJson == null)
        {
            Debug.LogWarning($"���� �����Ͱ� �������� �ʽ��ϴ�.");
            return false;
        }

        userData.spawnerID = int.Parse(gameDataJson[0]["spawnerID"].ToString());
        userData.prefabType = (Define.Prefabs)Enum.Parse(typeof(Define.Prefabs), gameDataJson[0]["prefabType"].ToString());
        userData.characterID = int.Parse(gameDataJson[0]["characterID"].ToString());
        userData.levelValue = int.Parse(gameDataJson[0]["levelUpCount"].ToString());
        userData.expValue = int.Parse(gameDataJson[0]["expValue"].ToString());
        userData.coinValue = int.Parse(gameDataJson[0]["coinValue"].ToString());
        userData.hpValue = int.Parse(gameDataJson[0]["hpValue"].ToString());
        userData.weaponID = int.Parse(gameDataJson[0]["weaponID"].ToString());

        //Test
        //userData.info = gameDataJson[0]["info"].ToString();
        //foreach (string itemKey in gameDataJson[0]["inventory"].Keys)
        //{
        //    userData.inventory.Add(itemKey, int.Parse(gameDataJson[0]["inventory"][itemKey].ToString()));
        //}
        //foreach (LitJson.JsonData equip in gameDataJson[0]["equipment"])
        //{
        //    userData.equipment.Add(equip.ToString());
        //}

        //
        Debug.Log(ToString());
        return true;
    }

    #endregion ServerLoad
}

