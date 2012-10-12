using System.Reactive.Concurrency;

namespace CrossPuzzleClient.ViewModels
{
    public interface ISchedulerProvider
    {

        IScheduler CurrentThread { get; }
        IScheduler Dispatcher { get; }
        IScheduler Immediate { get; }
        IScheduler NewThread { get; }
        IScheduler ThreadPool { get; }
        //IScheduler TaskPool { get; } 
    }



}