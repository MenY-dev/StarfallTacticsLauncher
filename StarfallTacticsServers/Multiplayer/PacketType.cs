namespace StarfallTactics.StarfallTacticsServers.Multiplayer
{
    public enum PacketType : int
    {
        None = 0,
        Auth = 1,
        AuthResponse = 2,
        Battle = 3,
        Chat = 4
    }
}
