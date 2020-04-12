using RayHospital.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;

namespace RayHospital.App
{
	internal class Program
	{
		private static void Main(string[] commandlineArguments)
		{
			var startDate = DateTime.Today;

			var patientRegistrations = new[]
			{
				new Registration<IPatient>(new Patient("Lucas", new Cancer(CancerTopography.HeadNeck)), startDate.AddDays(0)),
				new Registration<IPatient>(new Patient("Sandra", new Cancer(CancerTopography.HeadNeck)), startDate.AddDays(0)),
				new Registration<IPatient>(new Patient("Regina", new Cancer(CancerTopography.Breast)), startDate.AddDays(1)),
				new Registration<IPatient>(new Patient("Jane", new Flu()), startDate.AddDays(1)),
				new Registration<IPatient>(new Patient("Jack", new Flu()), startDate.AddDays(2)),
			};

			var rooms = new[]
			{
				new Room("RoomOne", TreatmentMachineCapability.None),
				new Room("RoomTwo", TreatmentMachineCapability.None),
				new Room("RoomThree", TreatmentMachineCapability.Advanced),
				new Room("RoomFour", TreatmentMachineCapability.Simple)
			};

			var doctors = new[]
			{
				new Doctor("John", new[] { DoctorRole.Oncologist }),
				new Doctor("Anna", new[] { DoctorRole.GeneralPractitioner }),
				new Doctor("Laura", new[] { DoctorRole.Oncologist, DoctorRole.GeneralPractitioner })
			};

			var existingConsultations = new List<Consultation>
			{
				new Consultation(new Patient("Ray", new Cancer(CancerTopography.HeadNeck)), doctors.First(), rooms.First(), startDate.AddDays(1))
			};

			var raySearchHospital = new Hospital(doctors, rooms, existingConsultations);

			void ProduceConsultation(IPatient patient, ITreater treater, ITreatmentLocation treatmentLocation, DateTime date)
			{
				var consultation = new Consultation(patient, treater, treatmentLocation, date);
				raySearchHospital.ScheduledScheduledConsultations.Add(consultation);
				Console.WriteLine($"Consultation scheduled for {consultation.Patient.Name} with {consultation.Treater.Name} in room {consultation.TreatmentLocation.Name} at {consultation.Date.ToShortDateString()}");
			}


			foreach(var patientRegistration in patientRegistrations)
			{
				ConsultationModule.ScheduleConsultation(ProduceConsultation, patientRegistration, raySearchHospital.ScheduledScheduledConsultations, raySearchHospital.Doctors, raySearchHospital.Rooms, startDate, startDate.AddYears(1));
			}
		}
	}
}
