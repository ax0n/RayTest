using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RayHospital.Interfaces;
using RayHospital.Resources;

namespace RayHospital.App
{
  class Program
  {
    /// This is a test application for the RayHospital booking system.
    /// It will simulate a number of patient registrations happening on a number of days starting from today, and print the resulting consultations.
    /// In a real application, the BookConsultation() and Consultations() methods would probably be called from controllers in a REST API.

    static void Main(string[] args)
    {
      // TODO: Create implementation of IConsultationsManager and initialize using HospitalResources.
      IConsultationsManager consultationsManager = null;

      // Read hard-coded list of patients and book a consultation for each patient
      DateTime startDate = DateTime.Today;
      foreach (var registration in PatientRegistrations.Patients)
      {
        var registrationDate = startDate.AddDays(registration.OffsetDays);
        var patient = new PatientRegistrationModel { Name = registration.Name, Condition = registration.Condition, Topography = registration.Topography };
        var consultation = consultationsManager.BookConsultation(registrationDate, patient);
      }

      // Print the resulting consultations
      var consultations = consultationsManager.Consultations();
      Console.WriteLine(JsonConvert.SerializeObject(consultations, Formatting.Indented, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" }));
    }
  }
}
