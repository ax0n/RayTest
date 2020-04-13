using RayHospital.Interfaces;
using RayHospital.Lib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RayHospital.App
{
	internal class Program
	{
		private static void Main(string[] commandlineArguments)
		{
			#region Capture

			var startDate = DateTime.Today;

			var patientRegistrations = new[]
			{
				new Registration<IPatient>(new Patient("Lucas", new HeadNeckCancer()), startDate.AddDays(0)),
				new Registration<IPatient>(new Patient("Sandra", new HeadNeckCancer()), startDate.AddDays(0)),
				new Registration<IPatient>(new Patient("Regina", new BreastCancer()), startDate.AddDays(1)),
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
				new Doctor("John", new[] { TreaterQualification.Oncologist }),
				new Doctor("Anna", new[] { TreaterQualification.GeneralPractitioner }),
				new Doctor("Laura", new[] { TreaterQualification.Oncologist, TreaterQualification.GeneralPractitioner })
			};

			var consultations = new List<Consultation>();

			#endregion

			#region Side effects

			void ProduceConsultation(IPatient patient, ITreater treater, ITreatmentLocation treatmentLocation, DateTime date)
			{
				var consultation = new Consultation(patient, treater, treatmentLocation, date);
				consultations.Add(consultation);
				Console.WriteLine($"Consultation scheduled for {patient.Name} with Dr {treater.Name} in {treatmentLocation.Name} at {date.ToShortDateString()}");
			}

			#endregion

			foreach(var patientRegistration in patientRegistrations)
			{
				Scheduling.ScheduleConsultation(ProduceConsultation, patientRegistration, consultations, doctors, rooms, startDate, startDate.AddYears(1));
			}
		}
	}
}
