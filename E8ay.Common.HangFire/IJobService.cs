using E8ay.Common.HangFire.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire
{
    public interface IJobService
    {
        void PublishEvent<T>(Event<T> e);
    }
}
