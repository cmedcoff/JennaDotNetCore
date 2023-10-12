using CrossDotNetPlatformatBizLogic;

namespace FullFrameworkConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            BizLogic bl = new BizLogic(new FullFrameworkMyLogger());
            bl.SomeImportantLogic();
        }
    }

    public class FullFrameworkMyLogger : IMyLogger
    {
        public void Log(string message)
        {
            System.Diagnostics.Debug.Write(message);
        }
    }
}