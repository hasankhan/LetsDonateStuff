using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBackgrounder;
using System.Threading.Tasks;

namespace LetsDonateStuff.Jobs
{
    public abstract class DelayedJob: Job
    {
        bool firstRun;
        DateTime createdOn;

        public DelayedJob(string name, TimeSpan interval) : this(name, interval, TimeSpan.MaxValue) { }
        public DelayedJob(string name, TimeSpan interval, TimeSpan timeout):base(name, interval, timeout) 
        {
            createdOn = DateTime.Now;
            firstRun = true;
        }

        public override Task Execute()
        {
            return new Task(OnExecuting);
        }

        void OnExecuting()
        {
            if (!firstRun || (DateTime.Now - createdOn) >= Interval)
                OnExecute();

            firstRun = false;
        }

        protected abstract void OnExecute();
    }
}