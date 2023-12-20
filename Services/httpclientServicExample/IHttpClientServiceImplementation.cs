using System.Threading.Tasks;
using TestFromGitToMongo.Models;

namespace TestFromGitToMongo.Services.httpServicExample
{
	public interface IHttpClientServiceImplementation
	{
        
        List<Bike> Bikes { get; set; }
        Task GetBikes();
        Task GetBikesWithStream();

        Task Execute();

    }
}
