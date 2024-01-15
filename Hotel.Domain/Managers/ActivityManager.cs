using Hotel.Domain.Exceptions;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;
using System;
using System.Collections.Generic;

namespace Hotel.Domain.Managers
{
    public class ActivityManager
    {
        private IActivityRepository _activityRepository;

        public ActivityManager(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public IReadOnlyList<Activity> GetActivities(string filter)
        {
            try
            {
                return _activityRepository.GetActivities(filter);
            }
            catch (Exception ex)
            {
                throw new ActivityManagerException("GetActivities", ex);
            }
        }

        public void AddActivity(Activity activity)
        {
            try
            {
                ValidateActivityData(activity);
                _activityRepository.AddActivity(activity);
            }
            catch (Exception ex)
            {
                throw new ActivityManagerException("AddActivity", ex);
            }
        }

        public void UpdateActivity(Activity activity)
        {
            try
            {
                ValidateActivityData(activity);
                _activityRepository.UpdateActivity(activity);
            }
            catch (Exception ex)
            {
                throw new ActivityManagerException("UpdateActivity", ex);
            }
        }

        public void DeleteActivity(int activityId)
        {
            try
            {
                // Soft-delete logic or additional checks can be added here
                _activityRepository.DeleteActivity(activityId);
            }
            catch (Exception ex)
            {
                throw new ActivityManagerException("DeleteActivity", ex);
            }
        }

        private void ValidateActivityData(Activity activity)
        {
            // Additional validation logic can be added here
            if (activity.Date < DateTime.Now)
            {
                throw new ActivityManagerException("Activity date cannot be in the past.");
            }

            // Add more validation as needed
        }
    }
}
