namespace CrossDotNetPlatformatBizLogic
{
    public class BizLogic
    {
        private readonly IMyLogger _logger;

        public BizLogic(IMyLogger logger)
        {
            _logger = logger;
        }

        public void SomeImportantLogic()
        {
            _logger.Log("Executing SomeImportantLogic!");
        }
    }


    public interface IMyLogger
    {
        void Log(string message);
    }

}
