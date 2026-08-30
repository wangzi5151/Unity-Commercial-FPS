namespace CommercialFPS
{
    public enum GameDifficulty
    {
        Easy,
        Normal,
        Hard,
        Nightmare,
        Hell
    }

    public enum GamePlayMode
    {
        EndlessWave,
        SurvivalStage,
        Sandbox,
        StoryCampaign
    }

    public enum GameEvent
    {
        OnPlayerHurt,
        OnPlayerDeath,
        OnEnemyKilled,
        OnBossSpawn,
        OnBossPhaseChange,
        OnWaveChanged,
        OnAirdropSpawn,
        OnWeatherChanged,
        OnVehicleExplode,
        OnAllyDeath,
        OnLandmineExplode,
        OnPickupLoot,
        OnMissionComplete,
        OnAchievementUnlock,
        OnWeaponModify,
        OnPlayerLevelUp
    }

    public enum EnemyState
    {
        Idle,
        Alert,
        Chase,
        Attack,
        Hurt,
        Dead
    }

    public enum MissionState
    {
        InProgress,
        Complete,
        ClaimReward,
        Failed
    }

    public enum BufferInputType
    {
        ReloadWeapon
    }
}
