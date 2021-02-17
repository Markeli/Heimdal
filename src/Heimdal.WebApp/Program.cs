using System.Threading.Tasks;

namespace Heimdal
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            var boostrapper = new HeimdalBootstrapper();
            return boostrapper.RunAsync(args);
        }
    }
}