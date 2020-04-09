using System;
using System.Collections.Generic;

namespace RayHospital.Interfaces
{
  public interface IConsultationsManager
  {
    ConsultationModel BookConsultation(DateTime registrationDate, PatientRegistrationModel patient);

    IEnumerable<ConsultationModel> Consultations();
  }
}