using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
