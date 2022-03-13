using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Repository.Imp
{
    public interface ITimeEntryRepository
    {
        

        int CreateTimeEntry(TimeEntryViewModel timeentry);
        int SaveTimeEntry(TimeEntryViewModel timeentry);
        int DeleteTimeEntry(int timeentryID);
        TimeEntryViewModel getTimeEntry(int? timeentryID);
        List<TimeEntryViewModel> getAllTimeEntries();
        List<TimeEntryViewModel> getAllTEs(int? id, string repdate);
        List<MobileTimeEntryViewModel> getAllTimeEntry(int userID);
        List<TimeEntryViewModel> getFilterTEs(FilterViewModel filter);


    }
}