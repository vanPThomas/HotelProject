using Hotel.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Interfaces
{
    public interface IActivityRepository
    {
        void AddActivity(Activity activity);
        void DeleteActivity(int activityId);
        IReadOnlyList<Activity> GetActivities(string filter);
        void UpdateActivity(Activity activity);
    }
}
