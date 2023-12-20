using Microsoft.EntityFrameworkCore;
using TestFromGitToMongo.Models;
using MongoFramework;

namespace TestFromGitToMongo.Data
{
    //THIS class/file can be deleted
    //public class DataContext : MongoDbContext
    //{
    //    public DataContext(IMongoDbConnection connection) : base(connection) { 
    //    }

    //    public MongoDbSet<Bike> Bikes{ get; set; }

    //    protected override void OnConfigureMapping(MappingBuilder mappingBuilder)
    //    {
    //        mappingBuilder.Entity<Bike>()
    //          .HasProperty(m => m.Cost, b => b.HasElementName("MappedCost"))
    //          .ToCollection("MyCustomEntities");
    //    }

    //}
}
