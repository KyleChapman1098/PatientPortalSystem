using System.Collections.Generic;

namespace PatientPortalSystem.Models.ViewModels{

    public class NurseDashboardViewModel
    {
        public List<AppointmentViewModel> Appointments { get; set; }
        public int PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
    }
}
