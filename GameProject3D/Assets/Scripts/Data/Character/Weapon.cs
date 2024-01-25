using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    ParticleSystem shotSFX = null;

    //
    //public enum State
    //{
    //    Ready, // �߻� �غ��
    //    Empty, // źâ�� ��
    //    Reloading // ������ ��
    //}
    //public State state { get; private set; } // ���� ���� ����

    //public Transform fireTransform; // �Ѿ��� �߻�� ��ġ

    ////public ParticleSystem muzzleFlashEffect; // �ѱ� ȭ�� ȿ��
    ////public ParticleSystem shellEjectEffect; // ź�� ���� ȿ��

    //private LineRenderer bulletLineRenderer; // �Ѿ� ������ �׸��� ���� ������

    ////private AudioSource gunAudioPlayer; // �� �Ҹ� �����
    ////public AudioClip shotClip; // �߻� �Ҹ�
    ////public AudioClip reloadClip; // ������ �Ҹ�

    //public float damage = 25; // ���ݷ�
    //private float fireDistance = 50f; // �����Ÿ�

    //public int ammoRemain = 100; // ���� ��ü ź��
    //public int magCapacity = 25; // źâ �뷮
    //public int magAmmo; // ���� źâ�� �����ִ� ź��


    //public float timeBetFire = 0.12f; // �Ѿ� �߻� ����
    //public float reloadTime = 1.8f; // ������ �ҿ� �ð�
    //private float lastFireTime; // ���� ���������� �߻��� ����


    //private void Awake()
    //{
    //    // ����� ������Ʈ���� ������ ��������
    //    //gunAudioPlayer = GetComponent<AudioSource>();
    //    bulletLineRenderer = GetComponent<LineRenderer>();

    //    // ����� ���� �ΰ��� ����
    //    bulletLineRenderer.positionCount = 2;
    //    // ���� �������� ��Ȱ��ȭ
    //    bulletLineRenderer.enabled = false;
    //}

    void Start()
    {
        BoxCollider collider = gameObject.GetOrAddComponent<BoxCollider>();
        collider.isTrigger = true;

        //Debug.Log("shotSFX : " + shotSFX.gameObject.name);
    }

    //private void OnEnable()
    //{
    //    // ���� źâ�� ����ä���
    //    magAmmo = magCapacity;
    //    // ���� ���� ���¸� ���� �� �غ� �� ���·� ����
    //    state = State.Ready;
    //    // ���������� ���� �� ������ �ʱ�ȭ
    //    lastFireTime = 0;
    //}

    public void InitWeapon()
    {
        // SFX
        {
            if (shotSFX == null)
                CreateShotSFX();

            if(shotSFX != null)
            {
                shotSFX.transform.parent = transform;
                shotSFX.transform.localPosition = new Vector3(0f, 0.14f, 0.6f);
                shotSFX.transform.localRotation = Quaternion.identity;
                shotSFX.Stop();
            }
        }
    }

    public void SetParent(Transform pParent = null)
    {
        transform.parent = pParent;
    }

    public void SetPosition(Vector3 pPos)
    {
        transform.localPosition = pPos;
    }

    public void SetRotation(Quaternion pRot)
    {
        transform.localRotation = pRot;
    }

    public void SetEnable(bool pEnable)
    {
        gameObject.SetActive(pEnable);
    }

    public void PlaySFX()
    {
        if (shotSFX == null)
            return;

        shotSFX.Stop();
        shotSFX.Play();
    }

    //// �߻� �õ�
    //public void Fire()
    //{
    //    // ���� ���°� �߻� ������ ����
    //    // && ������ �� �߻� �������� timeBetFire �̻��� �ð��� ����
    //    if (state == State.Ready
    //        && Time.time >= lastFireTime + timeBetFire)
    //    {
    //        // ������ �� �߻� ������ ����
    //        lastFireTime = Time.time;
    //        // ���� �߻� ó�� ����
    //        Shot();
    //    }
    //}

    //// ���� �߻� ó��
    //private void Shot()
    //{
    //    // ����ĳ��Ʈ�� ���� �浹 ������ �����ϴ� �����̳�
    //    RaycastHit hit;
    //    // �Ѿ��� ���� ���� ������ ����
    //    Vector3 hitPosition = Vector3.zero;

    //    // ����ĳ��Ʈ(��������, ����, �浹 ���� �����̳�, �����Ÿ�)
    //    if (Physics.Raycast(fireTransform.position,
    //        fireTransform.forward, out hit, fireDistance))
    //    {
    //        // ���̰� � ��ü�� �浹�� ���

    //        // �浹�� �������κ��� IDamageable ������Ʈ�� �������� �õ�
    //        IDamageable target =
    //            hit.collider.GetComponent<IDamageable>();

    //        // �������� ���� IDamageable ������Ʈ�� �������µ� �����ߴٸ�
    //        if (target != null)
    //        {
    //            // ������ OnDamage �Լ��� ������Ѽ� ���濡�� ������ �ֱ�
    //            target.OnDamage(damage, hit.point, hit.normal);
    //        }

    //        // ���̰� �浹�� ��ġ ����
    //        hitPosition = hit.point;
    //    }
    //    else
    //    {
    //        // ���̰� �ٸ� ��ü�� �浹���� �ʾҴٸ�
    //        // �Ѿ��� �ִ� �����Ÿ����� ���ư������� ��ġ�� �浹 ��ġ�� ���
    //        hitPosition = fireTransform.position +
    //                      fireTransform.forward * fireDistance;
    //    }

    //    // �߻� ����Ʈ ��� ����
    //    StartCoroutine(ShotEffect(hitPosition));

    //    // ���� źȯ�� ���� -1
    //    magAmmo--;
    //    if (magAmmo <= 0)
    //    {
    //        // źâ�� ���� ź���� ���ٸ�, ���� ���� ���¸� Empty���� ����
    //        state = State.Empty;
    //    }
    //}

    //// �߻� ����Ʈ�� �Ҹ��� ����ϰ� �Ѿ� ������ �׸���
    //private IEnumerator ShotEffect(Vector3 hitPosition)
    //{
    //    // �ѱ� ȭ�� ȿ�� ���
    //    muzzleFlashEffect.Play();
    //    // ź�� ���� ȿ�� ���
    //    shellEjectEffect.Play();

    //    // �Ѱ� �Ҹ� ���
    //    gunAudioPlayer.PlayOneShot(shotClip);

    //    // ���� �������� �ѱ��� ��ġ
    //    bulletLineRenderer.SetPosition(0, fireTransform.position);
    //    // ���� ������ �Է����� ���� �浹 ��ġ
    //    bulletLineRenderer.SetPosition(1, hitPosition);
    //    // ���� �������� Ȱ��ȭ�Ͽ� �Ѿ� ������ �׸���
    //    bulletLineRenderer.enabled = true;

    //    // 0.03�� ���� ��� ó���� ���
    //    yield return new WaitForSeconds(0.03f);

    //    // ���� �������� ��Ȱ��ȭ�Ͽ� �Ѿ� ������ �����
    //    bulletLineRenderer.enabled = false;
    //}

    //// ������ �õ�
    //public bool Reload()
    //{
    //    if (state == State.Reloading ||
    //        ammoRemain <= 0 || magAmmo >= magCapacity)
    //    {
    //        // �̹� ������ ���̰ų�, ���� �Ѿ��� ���ų�
    //        // źâ�� �Ѿ��� �̹� ������ ��� ������ �Ҽ� ����
    //        return false;
    //    }

    //    // ������ ó�� ����
    //    StartCoroutine(ReloadRoutine());
    //    return true;
    //}

    //// ���� ������ ó���� ����
    //private IEnumerator ReloadRoutine()
    //{
    //    // ���� ���¸� ������ �� ���·� ��ȯ
    //    state = State.Reloading;
    //    // ������ �Ҹ� ���
    //    gunAudioPlayer.PlayOneShot(reloadClip);

    //    // ������ �ҿ� �ð� ��ŭ ó���� ����
    //    yield return new WaitForSeconds(reloadTime);

    //    // źâ�� ä�� ź���� ����Ѵ�
    //    int ammoToFill = magCapacity - magAmmo;

    //    // źâ�� ä������ ź���� ���� ź�ຸ�� ���ٸ�,
    //    // ä������ ź�� ���� ���� ź�� ���� ���� ���δ�
    //    if (ammoRemain < ammoToFill)
    //    {
    //        ammoToFill = ammoRemain;
    //    }

    //    // źâ�� ä���
    //    magAmmo += ammoToFill;
    //    // ���� ź�࿡��, źâ�� ä�ŭ ź���� �A��
    //    ammoRemain -= ammoToFill;

    //    // ���� ���� ���¸� �߻� �غ�� ���·� ����
    //    state = State.Ready;
    //}

    //void OnTriggerEnter(Collider collider)
    //{
    //    target = collider.gameObject;
    //    Debug.Log("�浹 ����!" + collider.name);
    //}

    //void OnTriggerStay(Collider collider)
    //{
    //    target = collider.gameObject;
    //    Debug.Log("�浹 ��!" + collider.name);
    //}

    //void OnTriggerExit(Collider collider)
    //{
    //    Debug.Log("�浹 ��!" + collider.name);
    //}

    #region Load

    void CreateShotSFX()
    {
        DestroyShotSFX();

        string path = $"Prefabs/SFX/Shot";
        shotSFX = Managers.Resource.InstantiateResource(path, transform).GetOrAddComponent<ParticleSystem>();

        shotSFX.transform.parent = transform;
        shotSFX.transform.localPosition = new Vector3(0.0007f, 0.0491f, 0.5542f);
        shotSFX.transform.localRotation = Quaternion.identity;
        shotSFX.Stop();
    }

    void DestroyShotSFX()
    {
        if (shotSFX != null && shotSFX.gameObject != null)
        {
            Managers.Resource.DestroyGameObject(shotSFX.gameObject);
        }
        shotSFX = null;
    }

    #endregion Load
}