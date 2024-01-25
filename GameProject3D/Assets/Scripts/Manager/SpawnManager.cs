using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObj : MonoBehaviour
{
    public int spawnerID = 0;
    public int orderedID = 0;
    public bool isSpawn = false;
}

[Obsolete("Managers ���� : �Ϲ� Ŭ�������� ����� �� �����ϴ�. Managers�� �̿��� �ּ���.")]
public class Spawner : MonoBehaviour
{
    //
    Table.Spawner.Data spawnerData = null;
    Define.Prefabs prefabType = Define.Prefabs.None;
    int prefabID = 0;

    //
    readonly Dictionary<int, PooledObj> pooledObj_dic = new Dictionary<int, PooledObj>(); // <spawningID, spawning>
    int orderedID_property = 0; // �ػ�� ����
    int orderedID_grant
    {
        get
        {
            ++orderedID_property;
            return orderedID_property;
        }
    }

    //
    int spawnCount = 0;
    int reserveCount = 0;

    //
    SpawningPool spawningPool = null;
    IEnumerator updatePoolingCoroutine = null;

    /// <summary>
    /// ������ ������Ʈ�� �ܺ� �Լ��� ó���� �� ��� �մϴ�.
    /// </summary>
    Action<PooledObj, int, int> spawnEventAction = null; 

    void Start()
    {
        spawningPool = gameObject.GetOrAddComponent<SpawningPool>();

        //spawnEventAction -= AddMonsterCount;
        //spawnEventAction += AddMonsterCount;
    }

    public void SetSpawner(int pSpawnerID, Define.Prefabs pPrefabType, int pPrefabID, Action<PooledObj, int, int> pSpawnEventAction = null)
    {
        spawningPool = gameObject.GetOrAddComponent<SpawningPool>();

        if (pSpawnerID <= 0 || pPrefabType == Define.Prefabs.None || pPrefabID <= 0)
        {
            Debug.LogWarning("");
            return;
        }

        spawnerData = Managers.Table.GetTable<Table.Spawner>().GetTableData(pSpawnerID);
        prefabType = pPrefabType;
        prefabID = pPrefabID;
        if (pSpawnEventAction != null)
            spawnEventAction = pSpawnEventAction;

        //
        spawningPool.SetSpawingPool(spawnerData.poolAmount, prefabType, prefabID);

        //
        if (spawnerData.isPooled)
            UpdatePooling();
    }

    void UpdatePooling()
    {
        //
        if (updatePoolingCoroutine != null)
        {
            StopCoroutine(updatePoolingCoroutine);
            updatePoolingCoroutine = null;
        }
        updatePoolingCoroutine = UpdatePoolingCoroutine();
        StartCoroutine(updatePoolingCoroutine);
    }

    IEnumerator UpdatePoolingCoroutine()
    {
        spawnCount = 0;
        reserveCount = 0;
        yield return null;

        while (true)
        {
            if (reserveCount + spawnCount < spawnerData.keepCount)
            {
                SpawnCharacter();
            }
            yield return null;
        }
    }

    public void SpawnCharacter()
    {
        IEnumerator reserveCharacterCoroutine = ReserveCharacterCoroutine();
        StartCoroutine(reserveCharacterCoroutine);
    }

    IEnumerator ReserveCharacterCoroutine()
    {
        // Reserve
        reserveCount++;
        yield return new WaitForSeconds(UnityEngine.Random.Range(spawnerData.minSpawnTime, spawnerData.maxSpawnTime));

        // Spawn
        PooledObj pooledObj = spawningPool.SpawnPooledObject(spawnerData.spawnPos, Quaternion.identity).GetOrAddComponent<PooledObj>();
        pooledObj.spawnerID = spawnerData.id;
        pooledObj.orderedID = orderedID_grant;
        pooledObj.isSpawn = true;
        pooledObj_dic.Add(pooledObj.orderedID, pooledObj);

        // SetSpawning
        if (spawnEventAction != null)
            spawnEventAction.Invoke(pooledObj, spawnerData.id, prefabID);

        //
        reserveCount--;
        spawnCount++;
    }

    public void DespawnCharacter(int pOrderedID)
    {
        PooledObj pooledObj = FindPooledObj(pOrderedID);
        pooledObj.isSpawn = false;

        spawningPool.DespawnPooledObject(pooledObj.gameObject);
        spawnCount--;

        //if (spawnEventAction != null)
        //spawnEventAction.Invoke(-1);
    }

    [Obsolete("���� �ʿ�")]
    PooledObj FindPooledObj(int pOrderedID)
    {
        if (pooledObj_dic.ContainsKey(pOrderedID) == false)
        {
            Debug.LogWarning("");
            return null;
        }

        PooledObj pooledObj = pooledObj_dic[pOrderedID];
        if (pooledObj == null)
        {
            Debug.LogWarning("");
            return null;
        }

        return pooledObj;
    }
    //void AddSpawnCount(int value) { spawnCount += value; }
    //void SetKeepCount(int count) { keepCount = count; }
}

public class SpawnManager : BaseManager
{
    Dictionary<int, Spawner> spawner_dic = new Dictionary<int, Spawner>();


    #region Override

    protected override void InitDataProcess() { }
    protected override void ResetDataProcess()
    {
        DestroyAllSpawner();
    }

    #endregion Override

    public void SetPlayerSpawner()
    {
        SetCharacterSpawner(Managers.User.SpawnerID, Managers.User.PrefabType, Managers.User.CharacterID, OnSpawnedPlayerAction);
    }

    public void SetMonsterSpawner(int pSpawningID)
    {
        Table.Spawning.Data spawningData = Managers.Table.GetTable<Table.Spawning>().GetTableData(pSpawningID);

        SetCharacterSpawner(spawningData.spawnerID, spawningData.prefabType, spawningData.prefabID, OnSpawnedMonsterAction);
    }

    void SetCharacterSpawner(int pSpawnerID, Define.Prefabs pPrefabType, int pPrefabID = 0, Action<PooledObj, int, int> pSpawnedCharacterAction = null)
    {
        Spawner spawner = CreateSpawner(pSpawnerID);
        spawner.SetSpawner(pSpawnerID, pPrefabType, pPrefabID, pSpawnedCharacterAction);
    }

    public void SpawnCharacter(int pSpawnerID )
    {
        Spawner spawner = FindSpawner(pSpawnerID);
        if (spawner == null)
        {
            Debug.LogWarning($"Failed : �������� ���� ������({pSpawnerID})�Դϴ�.\n�����ʸ� ������ �� ������ �õ����ּ���.");
            return;
        }

        spawner.SpawnCharacter();
    }

    public void DespawnCharacter(GameObject pDespawnedObj)
    {
        PooledObj pooledObj = GetPooledObj(pDespawnedObj);

        Spawner spawner = FindSpawner(pooledObj.spawnerID);
        spawner.DespawnCharacter(pooledObj.orderedID);
    }

    void OnSpawnedMonsterAction(PooledObj pPooledObj, int pSpawnerID, int pPrefabID)
    {
        if(pPooledObj == null)
        {
            Debug.LogWarning("");
            return;
        }

        Table.Spawner.Data spawnerData = Managers.Table.GetTable<Table.Spawner>().GetTableData(pSpawnerID);
        Table.Character.Data characterData = Managers.Table.GetTable<Table.Character>().GetTableData(pPrefabID);

        Type type = Type.GetType(characterData.characterType.ToString());
        BaseCharacter character = pPooledObj.gameObject.GetComponent(type) as BaseCharacter;
        if (character == null)
        {
            character = pPooledObj.gameObject.AddComponent(type) as BaseCharacter;
        }

        character.SpawnCharacter(characterData.id);

        // Monster
        Vector3 randDir = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(1, 3);
        randDir.y = 0;
        Vector3 randPos = spawnerData.spawnPos + randDir;

        character.transPosition = randPos;
        character.transRotation = Quaternion.identity;

        //while (true)
        //{
        //    randDir = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(0, spawnerData.spawnRadius);
        //    randDir.y = 0;
        //    randPos = spawnerData.spawnPos + randDir;

        //    // �� �� �ֳ�
        //    if (character.CalculateNavMeshPath(randPos))
        //        break;
        //}
        //character.transform.localPosition = randPos;
        //character.transform.localRotation = Quaternion.identity;
    }

    void OnSpawnedPlayerAction(PooledObj pPooledObj, int pSpawnerID, int pPrefabID)
    {
        if (pPooledObj == null)
        {
            Debug.LogWarning("");
            return;
        }

        Table.Spawner.Data spawnerData = Managers.Table.GetTable<Table.Spawner>().GetTableData(pSpawnerID);
        Table.Character.Data characterData = Managers.Table.GetTable<Table.Character>().GetTableData(pPrefabID);

        Type type = Type.GetType(characterData.characterType.ToString());
        BaseCharacter character = pPooledObj.gameObject.GetComponent(type) as BaseCharacter;
        if (character == null)
        {
            character = pPooledObj.gameObject.AddComponent(type) as BaseCharacter;
        }

        character.SpawnCharacter(characterData.id);

        // Player
        Vector3 randDir = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(0, spawnerData.spawnRadius);
        randDir.y = 0;
        Vector3 randPos = spawnerData.spawnPos + randDir;

        character.transPosition = randPos;
        character.transRotation = Quaternion.identity;

        // GameManager : �÷��̾� ���
        Managers.Game.SetPalyer(character as GamePlayer);
    }

    Spawner CreateSpawner(int pSpawnerID)
    {
        DestroySpawner(pSpawnerID);

        string go_name = $"@Spawner{pSpawnerID}";
        Spawner spawner = Managers.Resource.CreateComponentObject<Spawner>(go_name);

        spawner_dic.Add(pSpawnerID, spawner);
        return spawner;
    }

    void DestroySpawner(int pSpawnerID)
    {
        Spawner spawner = FindSpawner(pSpawnerID);
        if (spawner == null)
            return;
        //
        Managers.Resource.DestroyGameObject(spawner.gameObject);
        spawner_dic.Remove(pSpawnerID);
    }

    void DestroyAllSpawner()
    {
        foreach (KeyValuePair<int, Spawner> kv in spawner_dic)
        {
            DestroySpawner(kv.Key);
        }

        spawner_dic.Clear();
    }

    Spawner FindSpawner(int pSpawnerID)
    {
        if (spawner_dic.ContainsKey(pSpawnerID) == false)
        {
            Debug.LogWarning("");
            return null;
        }

        Spawner spawner = spawner_dic[pSpawnerID];
        if (spawner == null)
        {
            Debug.LogWarning("");
            return null;
        }

        return spawner;
    }

    PooledObj GetPooledObj(GameObject pDespawnedObj)
    {
        if (pDespawnedObj == null)
        {
            Debug.LogWarning("");
            return null;
        }

        PooledObj pooledObj = pDespawnedObj.GetComponent<PooledObj>();
        if (pooledObj == null)
        {
            Debug.LogWarning("");
            return null;
        };

        return pooledObj;
    }
}
