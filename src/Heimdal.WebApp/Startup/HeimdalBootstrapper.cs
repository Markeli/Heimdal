using Curiosity.Hosting.Web;
using Heimdal.CLI;
using Heimdal.Configuration;

namespace Heimdal
{
    public class HeimdalBootstrapper : CuriosityWebAppBootstrapper<HeimdalCLIArguments, HeimdalConfiguration, Startup>
    {
    }
}