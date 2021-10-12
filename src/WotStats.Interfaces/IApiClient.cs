namespace WotStats.Interfaces
{
    using System.Threading.Tasks;
    using WotStats.Interfaces.Messages;

    public interface IApiClient
    {
        Realm Realm { get; }

        Task<Response<Player[]>> FindPlayersAsync(params string[] nicknames);
    }
}
