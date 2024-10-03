using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.WorkerService.Service
{
    public class ServiceSettings
    {
        private int _TimeDelaySeconds;

        public string PathLog { get; set; }
        public TimeSpan DelayTimeSpan { get; set; }
        public int TimeDelaySeconds
        {
            get { return this._TimeDelaySeconds; }
            set
            {
                this._TimeDelaySeconds = value;
                this.DelayTimeSpan = TimeSpan.FromSeconds(value);
            }
        }
    }
}
