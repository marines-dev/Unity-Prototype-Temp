using System;
using UnityEngine;

/// <summary>
/// 1. ManagersŬ������ �ش� �Ŵ����� instance �߰��մϴ�.
/// 2. InitSceneŬ������ �ش� �Ŵ����� init() �߰��մϴ�.
/// 3. TitleSceneŬ�������� �ش� �Ŵ����� init() �߰��մϴ�.
/// </summary>

[Obsolete("Managers ���� : �Ϲ� Ŭ�������� ����� �� �����ϴ�. Managers�� �̿��� �ּ���.")]
public class SampleManager : BaseManager
{
    #region Override

    protected override void InitDataProcess() { }
    protected override void ResetDataProcess() { }

    #endregion Override
}
