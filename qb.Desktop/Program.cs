using osu.Framework;

namespace qb.Desktop
{
    class Program
    {
        static void Main(string[] args)
        {
            using var host = Host.GetSuitableHost("qb");
            host.Run(new DesktopGame());
        }
    }
}
