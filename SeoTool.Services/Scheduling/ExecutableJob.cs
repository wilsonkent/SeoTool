using Quartz;
using SeoTool.Services.Interface;
using System.Threading.Tasks;

namespace SeoTool.Services.Scheduling
{
    public class ExecutableJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.MergedJobDataMap;
            var executableModel = (IExecutableModel)dataMap["ExecutableModel"];

            await executableModel.ExecuteTask();
        }
    }
}
