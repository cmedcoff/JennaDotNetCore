using CrossDotNetPlatformatBizLogic;

namespace LoggingConsole2FromWorkerServiceTemplate;

public class BizLogicConsumer
{
    private readonly BizLogic _bizLogic;

    public BizLogicConsumer(BizLogic bizLogic)
    {
        _bizLogic = bizLogic;
    }
}