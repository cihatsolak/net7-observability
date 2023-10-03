namespace OrderAPI.Services;

public class RedisService
{
    private readonly ConnectionMultiplexer _connectionMultiplexer;

    public RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = (ConnectionMultiplexer)connectionMultiplexer;
    }

    public IDatabase GetDatabase(int dbNo)
    {
        return _connectionMultiplexer.GetDatabase(dbNo);
    }
}
