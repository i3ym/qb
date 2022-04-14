using System.Linq;
using osu.Framework;

namespace qb.Desktop
{
    class Program
    {
        static void Main(string[] args)
        {
            using var host = Host.GetSuitableDesktopHost("qb");
            host.Run(new DesktopGame(args.Contains("test")));
        }
    }
}
