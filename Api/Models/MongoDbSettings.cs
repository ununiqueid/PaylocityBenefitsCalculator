using MongoDB.Driver;

namespace Api.Models;

public class MongoDbSettings
{
    public ServerApi ServerApi { get; set; } = new ServerApi(ServerApiVersion.V1);
    public string DatabaseName { get; set; }
    public string ConnectionString { get; set; }
    public string CollectionName { get; set; }
}
