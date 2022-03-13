using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Interface
{
    public class TimeEntryServices : ITimeEntryServices
    {
        private readonly ITimeEntryRepository timeentryRepository;
        public TimeEntryServices(ITimeEntryRepository _timeentryRepository)
        {
            timeentryRepository = _timeentryRepository;
        }

        public List<TimeEntryViewModel> getAllTimeEntries()
        {
            return timeentryRepository.getAllTimeEntries();
        }


        public int CreateTimeEntry(TimeEntryViewModel timeentry)
        {
            return timeentryRepository.CreateTimeEntry(timeentry);
        }

        public int SaveTimeEntry(TimeEntryViewModel timeentry)
        {
            return timeentryRepository.SaveTimeEntry(timeentry);
        }
        public int DeleteTimeEntry(int timeentryID)
        {
            return timeentryRepository.DeleteTimeEntry(timeentryID);
        }

        public TimeEntryViewModel getTimeEntry(int? timeentryID)
        {

            return timeentryRepository.getTimeEntry(timeentryID);
        }
        public List<TimeEntryViewModel> getAllTEs(int? id, string repdate)
        {
            return timeentryRepository.getAllTEs(id, repdate);
        }
        public List<MobileTimeEntryViewModel> getAllTimeEntry(int userID)
        {
            return timeentryRepository.getAllTimeEntry(userID);
        }
        public List<TimeEntryViewModel> getFilterTEs(FilterViewModel filter)
        {
            return timeentryRepository.getFilterTEs(filter);
        }
    }
}