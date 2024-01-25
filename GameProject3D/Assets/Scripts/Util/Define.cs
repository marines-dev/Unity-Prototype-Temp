using System;

public class Define
{
    public enum Scene
    {
        // ※씬 이름과 동일해야 합니다.
        None,
        InitScene,
        LoadScene,
        TitleScene,
        //LobbyScene,
        WorldScene,
        Login,
        //SampleScene,
    }

    public enum Prefabs
    {
        None,
        Character,
        Weapon,
    }

    public enum Character
    {
        None,
        GamePlayer,
        GameMonster,
        NPC,
    }

    //public enum PrefabName
    //{
    //    None,
    //    Player_Male,
    //    Monster,
    //}

    public enum BaseState
    {
        None,
        Die,
        Idle,
        Run,
        //Attack,
    }

    public enum UpperState
    {
        None,
        Ready,
        Attack,
    }

    public enum WeaponType
    {
        None,
        Hand,
        Sword,
        Gun,
    }

    public enum Layer
    {
        Monster = 8,
        Ground = 9,
        Block = 10,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }

    public enum CameraMode
    {
        None,
        QuarterView,
    }

    //static public readonly Scene startScene = Scene.InitScene;
    static public readonly Type startScene = typeof(InitScene);
}
