using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire.EventBus
{
    public interface IEventHandler<T>
    {
        void Handle(Event<T> e);
    }
}
