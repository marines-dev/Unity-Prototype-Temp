using System;
using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public interface ITargetHandler
{
    public Vector3 transPosition { get; set; }
    public Quaternion transRotation { get; set; }

    public Define.BaseState baseStateType { get; }

    public void OnEnableTargetOutline();
    public void OnDisableTargetOutline();

    public void OnDamage(int pValue);
}

public abstract class BaseCharacter : MonoBehaviour, ITargetHandler
{
    // Character
    int characterID_pro = 0;
    protected int characterID
    {
        get
        {
            if (characterID_pro <= 0)
            {
                Debug.LogWarning("");
            }
            return characterID_pro;
        }
        private set
        {
            if (value <= 0)
            {
                Debug.LogWarning("Failed : ");
                return;
            }
            characterID_pro = value;
        }
    }

    // Stat
    protected struct Stat
    {
        public int maxHp;
        public int currentHp;
        public int attack;
        public int defense;
        public float moveSpeed;
        public int maxExp;
        //public int exp = 0;
    }
    protected Stat stat_pro;
    protected Stat stat
    {
        get
        {
            return stat_pro;
        } 
    }

    // State
    public Define.BaseState baseStateType { get; private set; } = Define.BaseState.None;
    protected Define.UpperState upperStateType { get; private set; } = Define.UpperState.None;
    bool isBaseStateProcess = false;
    bool isUpperStateProcess = false;

    // Transform
    public Vector3 transPosition
    {
        get
        {
            Vector3 pos = transform.localPosition;
            return pos;
        }
        set
        {
            if (value == null)
                value = Vector3.zero;

            transform.localPosition = value;
        }
    }
    public Quaternion transRotation
    {
        get
        {
            Quaternion rot = transform.localRotation;
            return rot;
        }
        set
        {
            if (value == null)
                value = Quaternion.identity;

            transform.localRotation = value;
        }
    }

    // Test
    protected string baseLayerName = "Base Layer";
    protected string upperLayerName = "Upper Layer";
    string upperReadyAnimationClip1 = "Humanoid_Ready_Hand";
    string upperReadyAnimationClip2 = "Humanoid_Ready_Gun";
    string upperAttackAnimationClip1 = "Humanoid_Attack_Hand";
    string upperAttackAnimationClip2 = "Humanoid_Attack_Gun";
    protected float hitTime = 0.5f;

    Collider collider = null;

    NavMeshAgent navMeshAgent_pro = null;
    public NavMeshAgent navMeshAgent
    {
        get
        {
            if (navMeshAgent_pro == null)
            {
                navMeshAgent_pro = transform.gameObject.GetOrAddComponent<NavMeshAgent>();
            }
            return navMeshAgent_pro;
        }
    }

    Animator_Util animatorOverride_pro = null;
    protected Animator_Util animatorOverride
    {
        get
        {
            if (animatorOverride_pro == null)
            {
                animatorOverride_pro = gameObject.GetOrAddComponent<Animator_Util>();
            }
            return animatorOverride_pro;
        }
    }

    [Obsolete("임시")] Shader_Util shader_pro = null;
    protected Shader_Util shader
    {
        get
        {
            if (shader_pro == null)
            {
                shader_pro = gameObject.GetOrAddComponent<Shader_Util>();
            }
            return shader_pro;
        }
    }

    protected Weapon weapon { get; private set; } = null;
    HPBarUI hPBarUI = null;

    IEnumerator fixedUpdateHPBarCoroutine = null;

    IEnumerator baseStateProcessCoroutine = null;
    IEnumerator baseStateProcessRoutine = null;
    IEnumerator upperStateProcessCoroutine = null;
    IEnumerator upperStateProcessRoutine = null;

    void Start()
    {
        collider = gameObject.GetComponentInChildren<CapsuleCollider>();
        if (collider == null)
        {
            Renderer renderer = GetComponentInChildren<Renderer>();
            if(renderer != null)
            {
                collider = renderer.gameObject.GetOrAddComponent<MeshCollider>();
            }
        }

        navMeshAgent_pro = gameObject.GetOrAddComponent<NavMeshAgent>();
        navMeshAgent_pro.radius = 0.3f;
        animatorOverride_pro = gameObject.GetOrAddComponent<Animator_Util>();
        shader_pro = gameObject.GetOrAddComponent<Shader_Util>();
    }

    //void OnDestroy()
    //{
    //}

    #region Virtual

    /// <summary>
    /// 스폰 시 Base.SpawnCharacter() 함수에서 캐릭터를 초기화합니다.
    /// </summary>
    protected abstract void InitSpawnCharacter();
    //public abstract void ResetCharacter();
    protected abstract IEnumerator BaseDieStateProcecssCoroutine();
    protected abstract IEnumerator BaseIdleStateProcecssCoroutine();
    protected abstract IEnumerator BaseRunStateProcecssCoroutine();
    protected abstract IEnumerator UpperReadyStateProcecssCoroutine();
    protected abstract IEnumerator UpperAttackStateProcecssCoroutine();
    protected abstract void OnHitEvent();
    //protected abstract void SetDead();
    //protected abstract void SetIdle();
    //protected abstract void SetRun();
    //protected abstract void SetSkill();
    //protected abstract void SetRespawn();

    #endregion Virtual

    [Obsolete("테스트 중")]
    public void SpawnCharacter(int pCharacterID)
    {
        //
        characterID = pCharacterID;

        Table.Character.Data characterData = Managers.Table.GetTable<Table.Character>().GetTableData(characterID);
        SetStat(characterData.level);

        transPosition = Vector3.zero;
        transRotation = Quaternion.identity;

        shader.SetMateriasColorAlpha(1f, false);
        animatorOverride.SetAnimatorController(characterData.animatorController, characterData.animatorAvatar);
        animatorOverride.SwapAnimationClip(upperReadyAnimationClip1, upperReadyAnimationClip2);
        animatorOverride.SwapAnimationClip(upperAttackAnimationClip1, upperAttackAnimationClip2);

        // Weapon
        if (characterData.weaponID != 0)
        {
            if (weapon == null)
                CreateWeapon(characterData.weaponID);

            weapon.InitWeapon();
            SetWeaponPos(Vector3.zero, Quaternion.identity, false);
        }

        // Stat
        if (hPBarUI == null)
            CreateHPBarUI();

        // State
        baseStateType = Define.BaseState.None; //초기화
        upperStateType = Define.UpperState.None;
        isBaseStateProcess = false;
        isUpperStateProcess = false;


        // 자식 클래스에서 초기화할 함수입니다.
        InitSpawnCharacter(); // 임시(순서)

        //
        SetBaseStateType(Define.BaseState.Idle);
        SetUpperStateType(Define.UpperState.Ready);
        SetHPBarUI();
    }

    protected void DespawnCharacter()
    {
        Managers.Spawn.DespawnCharacter(gameObject);
    }

    void SetWeaponPos(Vector3 pPos, Quaternion pRot, bool pEnable)
    {
        if (weapon == null)
            return;

        weapon.SetPosition(pPos);
        weapon.SetRotation(pRot);
        weapon.SetEnable(pEnable);
    }

    void SetHPBarUI()
    {
        hPBarUI.OpenUI();
        FixedUpdateHPBarProcess();
    }

    [Obsolete("레벨업 스탯 구현 필요")]
    protected void SetStat(int pLevel)
    {
        Table.Character.Data characterData = Managers.Table.GetTable<Table.Character>().GetTableData(characterID);
        Table.Stat.Data statData = Managers.Table.GetTable<Table.Stat>().GetTableData(characterData.statID);

        stat_pro.maxHp = statData.maxHp;
        stat_pro.currentHp = statData.maxHp;
        stat_pro.attack = statData.attack;
        stat_pro.defense = statData.defense;
        stat_pro.moveSpeed = statData.moveSpeed;
        stat_pro.maxExp = statData.maxExp;
    }

    protected void SetHP(int pHpValue)
    {
        if (pHpValue < 0)
            stat_pro.currentHp = 0;
        else if (pHpValue > stat_pro.maxHp)
            stat_pro.currentHp = stat_pro.maxHp;
        else
        {
            stat_pro.currentHp = pHpValue;
        }
    }

    protected void SetMateriasColorAlpha(float pAlpha)
    {
        shader.SetMateriasColorAlpha(pAlpha, true);
    }

    protected bool CalculateNavMeshPath(Vector3 _randPos)
    {
        NavMeshPath path = new NavMeshPath();
        return navMeshAgent.CalculatePath(_randPos, path);
    }

    protected void SetBaseStateType(Define.BaseState pBaseStateType)
    {
        if (baseStateType == Define.BaseState.Die)
        {
            Debug.Log("");
            return;
        }

        if (pBaseStateType == Define.BaseState.None)
        {
            Debug.Log("");
            return;
        }

        if (pBaseStateType == baseStateType)
        {
            Debug.Log("");
            return;
        }

        //
        baseStateType = pBaseStateType;
        //animatorOverride.speed = 1; //임시

        SetBaseStateProcess();
    }

    protected void SetUpperStateType(Define.UpperState pUpperStateType)
    {
        if (baseStateType == Define.BaseState.Die)
        {
            Debug.Log("");
            return;
        }

        if (pUpperStateType == Define.UpperState.None)
        {
            Debug.Log("");
            return;
        }

        if (pUpperStateType == upperStateType)
        {
            Debug.Log("");
            return;
        }

        //
        upperStateType = pUpperStateType;
        //animatorOverride.speed = 1; //임시

        SetUpperStateProcess();
    }

    public void OnDamage(int pValue)
    {
        int damage = Mathf.Max(0, pValue - stat_pro.defense);
        int hp = stat_pro.currentHp - pValue;
        SetHP(hp);

        //
        if (stat_pro.currentHp <= 0)
        {
            Debug.Log($"{gameObject.name} : Dead");
            ClearFixedUpdateHPBar();
            //
            SetUpperStateType(Define.UpperState.Ready);
            SetBaseStateType(Define.BaseState.Die);
        }
    }

    public void OnEnableTargetOutline()
    {
        shader.OnEnableOutline();
    }

    public void OnDisableTargetOutline()
    {
        shader.OnDisableOutline();
    }

    void FixedUpdateHPBarProcess()
    {
        ClearFixedUpdateHPBarPorcess();

        fixedUpdateHPBarCoroutine = FixedUpdateHPBarCoroutine();
        StartCoroutine(fixedUpdateHPBarCoroutine);
    }

    void ClearFixedUpdateHPBar()
    {
        //
        ClearFixedUpdateHPBarPorcess();
        if (hPBarUI != null)
            hPBarUI.CloseUI();
    }

    void ClearFixedUpdateHPBarPorcess()
    {
        if (fixedUpdateHPBarCoroutine != null)
        {
            StopCoroutine(fixedUpdateHPBarCoroutine);
            fixedUpdateHPBarCoroutine = null;
        }
    }

    IEnumerator FixedUpdateHPBarCoroutine()
    {
        if (hPBarUI == null)
        {
            Debug.LogWarning("");
            yield break;
        }

        yield return null;

        hPBarUI.SetHPBar(transform, stat_pro.currentHp, stat_pro.maxHp);

        while (baseStateType != Define.BaseState.Die)
        {
            hPBarUI.SetHPBar(transform, stat_pro.currentHp, stat_pro.maxHp);

            yield return new WaitForFixedUpdate();
        }

        hPBarUI.SetHPBar(transform, 0, stat_pro.maxHp);
    }

    void SetBaseStateProcess()
    {
        ClearBaseStateProcess();

        switch (baseStateType)
        {
            case Define.BaseState.Die:
                {
                    baseStateProcessRoutine = BaseDieStateProcecssCoroutine();
                }
                break;
            case Define.BaseState.Idle:
                {
                    //// WeaponPos
                    //Vector3 pos = new Vector3(-0.0319f, 0.0585f, 0.1015f);
                    //Quaternion rot = Quaternion.Euler(0f, 0f, 0f);
                    //SetWeaponPos(pos, rot, true);

                    baseStateProcessRoutine = BaseIdleStateProcecssCoroutine();
                }
                break;
            case Define.BaseState.Run:
                {
                    //// WeaponPos
                    //Vector3 pos = new Vector3(-0.139f, 0.025f, 0.043f);
                    //Quaternion rot = Quaternion.Euler(17.513f, -57.14f, -4.325f);
                    //SetWeaponPos(pos, rot, true);

                    baseStateProcessRoutine = BaseRunStateProcecssCoroutine();
                }
                break;
        }

        baseStateProcessCoroutine = BaseStateProcessCoroutine();
        StartCoroutine(baseStateProcessCoroutine);
    }

    void ClearBaseStateProcess()
    {
        // Coroutine
        if (baseStateProcessCoroutine != null)
        {
            StopCoroutine(baseStateProcessCoroutine);
            baseStateProcessCoroutine = null;
        }

        // Routine
        if (baseStateProcessRoutine != null)
        {
            StopCoroutine(baseStateProcessRoutine);
            baseStateProcessRoutine = null;
        }

        isBaseStateProcess = false;
    }

    IEnumerator BaseStateProcessCoroutine()
    {
        //
        isBaseStateProcess = true;
        yield return null;

        if (baseStateProcessRoutine != null)
            yield return baseStateProcessRoutine;

        isBaseStateProcess = false;
    }

    void SetUpperStateProcess()
    {
        ClearUpperStateProcess();

        switch (upperStateType)
        {
            case Define.UpperState.Ready:
                {
                    // WeaponPos
                    Vector3 pos = new Vector3(-0f, 0f, -0.3f);
                    Quaternion rot = Quaternion.Euler(0f, 0f, 180f);
                    SetWeaponPos(pos, rot, true);

                    upperStateProcessRoutine = UpperReadyStateProcecssCoroutine();
                }
                break;
            case Define.UpperState.Attack:
                {
                    // WeaponPos
                    Vector3 pos = new Vector3(0.14f, 0.14f, -0.35f);
                    Quaternion rot = Quaternion.Euler(19f, -37f, 166f);
                    SetWeaponPos(pos, rot, true);

                    upperStateProcessRoutine = UpperAttackStateProcecssCoroutine();
                }
                break;
        }

        upperStateProcessCoroutine = UpperStateProcessCoroutine();
        StartCoroutine(upperStateProcessCoroutine);
    }

    void ClearUpperStateProcess()
    {
        // Coroutine
        if (upperStateProcessCoroutine != null)
        {
            StopCoroutine(upperStateProcessCoroutine);
            upperStateProcessCoroutine = null;
        }

        // Routine
        if (upperStateProcessRoutine != null)
        {
            StopCoroutine(upperStateProcessRoutine);
            upperStateProcessRoutine = null;
        }

        isUpperStateProcess = false;
    }

    IEnumerator UpperStateProcessCoroutine()
    {
        //
        isUpperStateProcess = true;
        yield return null;

        if (upperStateProcessRoutine != null)
            yield return upperStateProcessRoutine;

        isUpperStateProcess = false;
    }

    #region Load

    void CreateWeapon(int pWeaponID)
    {
        if (pWeaponID == 0)
            return;

        DestroyWeapon();

        Transform[] children = GetComponentsInChildren<Transform>();
        Transform weponTrans = Array.Find(children, x => x.name == "Hand_L");
        if (weponTrans == null)
        {
            Debug.LogWarning("Failed : ");
            return;
        }

        string tempPath = $"Prefabs/Weapon/SM_Wep_Watergun_02";
        GameObject go = Managers.Resource.InstantiateResource(tempPath, weponTrans);
        if(go == null)
        {
            Debug.LogWarning("Failed : ");
            return;
        }

        weapon = go.GetOrAddComponent<Weapon>();
    }

    void DestroyWeapon()
    {
        if (weapon != null)
        {
            Managers.Resource.DestroyGameObject(weapon.gameObject);
            weapon = null;
        }
    }

    void CreateHPBarUI()
    {
        DestroyHPBarUI();

        hPBarUI = Managers.UI.CreateWorldSpaceUI<HPBarUI>(transform);
        hPBarUI.OpenUI();
    }

    void DestroyHPBarUI()
    {
        if (hPBarUI != null)
        {
            Managers.Resource.DestroyGameObject(hPBarUI.gameObject);
            hPBarUI = null;
        }
    }

    #endregion Load
}
