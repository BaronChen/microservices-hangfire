using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire.EventBus
{
    public interface IEventHandler
    {
        void Handle<T>(Event<T> e);
    }
}
