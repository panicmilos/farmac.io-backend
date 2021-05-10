using Hangfire.Dashboard;

namespace Farmacio_API.Filters
{
    public class DashboardFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}