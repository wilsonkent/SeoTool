using System.Threading.Tasks;

namespace SeoTool.Services.Interface
{
    public interface ISchedulingService
    {
        Task StartSchedule(int secondsInterval, IExecutableModel model);
        Task StopSchedule();
    }
}