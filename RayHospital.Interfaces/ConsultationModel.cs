using System;

namespace RayHospital.Interfaces
{
  public class ConsultationModel
  {
    public DateTime Date { get; set; }

    public string DoctorName { get; set; }

    public string RoomName { get; set; }

    public string PatientName { get; set; }
  }
}