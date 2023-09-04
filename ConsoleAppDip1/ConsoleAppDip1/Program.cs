using System.Security.Cryptography.X509Certificates;
using static ConsoleAppDip1.DoTheThingProcess;

namespace ConsoleAppDip1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // wire up dependency

            // previous to IOC containers, or when an IOC is just overkill
            // one 'wired up' depdenecies "manually" using ...
            // 'poor' man's DI or "pure" DI
            // see https://blog.ploeh.dk/2014/06/10/pure-di/
            // I highly recommend this guys's book on DI
            IWebBrowerController bc = new WebBrowserController();
            var theProcesss = new DoTheThingProcess(bc);

            // or could use ... IOC Container
            // this is not expected to compile, this is an example
            // of some non-existent IOC to make a point
            var ioc = someMagicThingThatGetsMeAnIoc();
            ioc.Register<IWebBrowerController, WebBrowserController>();
            ioc.Register<DoTheThingProcess>();

            theProcess = ioc.GetService<DoTheThingProcess>();

            // now we're out of 'setup', time to run the code
            theProcesss.JustDoIt();
        }
    }


    public class DoTheThingProcess
    {
        private IWebBrowerController _bc;

        public DoTheThingProcess(IWebBrowerController bc)
        {
            _bc = bc;
        }

        public void JustDoIt()
        {
            GetReadyStep();
            DoIt();
            PatSelfOnBack();
        }

        private void PatSelfOnBack()
        {
            
        }

        private void DoIt()
        {
            ReadIocDocs();
        }

        private void ReadIocDocs()
        {
            // use the browser to nav to the docs and let the user read
            _bc = new WebBrowserController();
            // ...
            // ...
            // ..
        }

        private void GetReadyStep()
        {
            
        }
    }

    public interface IWebBrowerController
    {

    }

    
    public class WebBrowserController : IWebBrowerController
    {

    }


}