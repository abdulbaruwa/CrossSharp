using System.Reactive.Concurrency;
using CrossPuzzleClient.ViewModels;

namespace CrossPuzzleClient.Observables
{

    public class SchedulerProvider : ISchedulerProvider
    {
        public IScheduler CurrentThread
        {
            get {return Scheduler.CurrentThread; } 
        }
        public IScheduler Dispatcher
        {
            get { return CoreDispatcherScheduler.Current; }
        }
        public IScheduler Immediate { get { return Scheduler.Immediate; } }

        public IScheduler NewThread
        {
            get { return NewThreadScheduler.Default; }
        }

        public IScheduler ThreadPool
        {
            get { return Scheduler.Default; }
        }
    }
}