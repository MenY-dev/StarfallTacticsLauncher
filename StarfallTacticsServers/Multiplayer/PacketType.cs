namespace StarfallTactics.StarfallTacticsServers.Multiplayer
{
    public enum PacketType : int
    {
        None = 0,
        PlayerAuth = 1,
        PlayerAuthResponse = 2,
        AuthRequest = 3,
        PlayerJoined = 4,
        PlayerDisconnected = 5,
        PlayerStatus = 6,
        PlayerStatusRequest = 7,
        PlayersInfo = 8,
        PlayersInfoRequest = 9,
        Battle = 31,
        Chat = 61,
        SystemMessage = 62
    }
}
