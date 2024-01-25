using System;
using System.Collections.Generic;
using UnityEngine;

//public enum TableName
//{
//    Spawning,
//    Spawner,
//    Character,
//    Stat,
//}

[Obsolete("Managers 전용 : 일반 클래스에서 사용할 수 없습니다. Managers를 이용해 주세요.")]
public class TableManager : BaseManager
{
    #region Table

    Dictionary<string, Table.BaseTable> table_dic = new Dictionary<string, Table.BaseTable>();
    
    //TableData.Spawning spawning_pro = null;
    //public TableData.Spawning spawning
    //{
    //    get
    //    {
    //        if (spawning_pro == null)
    //        {
    //            spawning_pro = LoadTableㅣ<TableData.Spawning>();
    //        }
    //        return spawning_pro;
    //    }
    //}

    //TableData.Spawner spawner_pro = null;
    //public TableData.Spawner spawner
    //{
    //    get
    //    {
    //        if (spawner_pro == null)
    //        {
    //            spawner_pro = LoadTableㅣ<TableData.Spawner>();
    //        }
    //        return spawner_pro;
    //    }
    //}

    //TableData.Character character_pro = null;
    //public TableData.Character character
    //{
    //    get
    //    {
    //        if (character_pro == null)
    //        {
    //            character_pro = LoadTableㅣ<TableData.Character>();
    //        }
    //        return character_pro;
    //    }
    //}

    //TableData.Stat stat_pro = null;
    //public TableData.Stat stat
    //{
    //    get
    //    {
    //        if (stat_pro == null)
    //        {
    //            stat_pro = LoadTableㅣ<TableData.Stat>();
    //        }
    //        return stat_pro;
    //    }
    //}

    #endregion Table
    #region JSON

    // public Dictionary<int, Data.StatData> StatDict { get; private set; } = new Dictionary<int, Data.StatData>();

    #endregion JSON

    #region Override

    protected override void InitDataProcess() 
    {
        //spawning = new Table.Spawning();
        //spawner = new Table.Spawner();
        //character = new Table.Character();
        //stat = new Table.Stat();

        //StatDict = LoadJson<Data.Stat, int, Data.StatData>("StatData_Test").MakeDict();
    }

    protected override void ResetDataProcess()
    {
        DeleteTable<Table.Spawning>();
        DeleteTable<Table.Spawner>();
        DeleteTable<Table.Character>();
        DeleteTable<Table.Stat>();
    }

    #endregion Override

    //public void LoadTable(TableName pTableNameType)
    //{
    //    //TableName tableNameType = Util.GetStringToEnum<TableName>(typeof(T).Name);
    //    //Table_Test table = null;
    //    switch (pTableNameType)
    //    {
    //        case TableName.Spawning:
    //            {
    //                if(spawning_pro != null)
    //                {
    //                    Debug.Log($"Failed : ");
    //                    return;
    //                }
    //                spawning_pro = LoadTableㅣ<TableData.Spawning>();
    //            }
    //            break;
    //        case TableName.Spawner:
    //            {
    //                if (spawner_pro != null)
    //                {
    //                    Debug.Log($"Failed : ");
    //                    return;
    //                }
    //                spawner_pro = LoadTableㅣ<TableData.Spawner>();
    //            }
    //            break;
    //        case TableName.Character:
    //            {
    //                if (character_pro != null)
    //                {
    //                    Debug.Log($"Failed : ");
    //                    return;
    //                }
    //                character_pro = LoadTableㅣ<TableData.Character>();
    //            }
    //            break;
    //        case TableName.Stat:
    //            {
    //                if (stat_pro != null)
    //                {
    //                    Debug.Log($"Failed : ");
    //                    return;
    //                }
    //                stat_pro = LoadTableㅣ<TableData.Stat>();
    //            }
    //            break;
    //    }
    //}
    //public void DeleteTable(TableName pTableNameType)
    //{
    //    switch (pTableNameType)
    //    {
    //        case TableName.Spawning:
    //            {
    //                if (spawning_pro != null)
    //                {
    //                    spawning_pro = null;
    //                }
    //            }
    //            break;
    //        case TableName.Spawner:
    //            {
    //                if (spawner_pro != null)
    //                {
    //                    spawner_pro = null;
    //                }
    //            }
    //            break;
    //        case TableName.Character:
    //            {
    //                if (character_pro != null)
    //                {
    //                    character_pro = null;
    //                }
    //            }
    //            break;
    //        case TableName.Stat:
    //            {
    //                if (stat_pro != null)
    //                {
    //                    stat_pro = null;
    //                }
    //            }
    //            break;
    //    }
    //}

    public void LoadTable<T>() where T : Table.BaseTable, new()
    {
        if(ContainsTable<T>())
        {
            Debug.LogWarning("Failed : ");
            return;
        }

        // T 테이블 클래스에서 테이블 데이터를 자동 생성 합니다.
        string tableName = typeof(T).Name;
        T table = new T();

        table_dic.Add(tableName, table);
    }

    void DeleteTable<T>() where T : Table.BaseTable, new()
    {
        if (ContainsTable<T>() == false)
            return;

        // 클래스에서 테이블 데이터를 자동 삭제 합니다.
        string tableName = typeof(T).Name;
        table_dic.Remove(tableName);
    }

    bool ContainsTable<T>() where T : Table.BaseTable, new()
    {
        string tableName = typeof(T).Name;
        bool containsKey = table_dic != null && table_dic.ContainsKey(tableName);
        return containsKey;
    }

    public T GetTable<T>() where T : Table.BaseTable, new()
    {
        if (ContainsTable<T>() == false)
        {
            Debug.LogWarning("Failed : ");
            return default(T);
        }

        string tableName = typeof(T).Name;
        T talbe = table_dic[tableName] as T;
        return talbe;
    }
    //T[] LoadTableData<T>() // 테이블 내부로 이동
    //{
    //    string tableName = typeof(T).Name;
    //    string path = $"Data/Table/{tableName}";
    //    TextAsset textAsset = Managers.Resource.LoadResource<TextAsset>(path);

    //    T[] TableDatas = CSVSerializer.DeserializeData<T>(textAsset.text);

    //    return TableDatas;
    //}

    //TILoader LoadJson<TILoader, TKey, TValue>(string path) where TILoader : ILoader<TKey, TValue>
    //{
    //    TextAsset textAsset = Managers.Resource.LoadResource<TextAsset>($"Data/{path}");
    //    return JsonUtility.FromJson<TILoader>(textAsset.text);
    //}
}