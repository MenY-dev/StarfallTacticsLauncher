namespace StarfallTactics.StarfallTacticsServers.Multiplayer
{
    public enum PacketType : int
    {
        None = 0,
        PlayerAuth = 1,
        PlayerAuthResponse = 2,
        AuthRequest = 3,
        Battle = 4,
        Chat = 5
    }
}
