using RayHospital.Interfaces;
using System;

namespace RayHospital.App
{
	class Program
	{
		static void Main(string[] args)
		{
			var startDate = DateTime.Today;

			var patientRegistrations = new[]
			{
				new PatientRegistration(new Patient("Lucas", new Cancer(CancerTopography.HeadNeck)), startDate.AddDays(0)),
				new PatientRegistration(new Patient("Sandra", new Cancer(CancerTopography.HeadNeck)), startDate.AddDays(0)),
				new PatientRegistration(new Patient("Regina", new Cancer(CancerTopography.Breast)), startDate.AddDays(1)),
				new PatientRegistration(new Patient("Jane", new Flu()), startDate.AddDays(1)),
				new PatientRegistration(new Patient("Jack", new Flu()), startDate.AddDays(2)),
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
				new Doctor("John", new[] {DoctorRole.Oncologist}),
				new Doctor("Anna", new[] {DoctorRole.GeneralPractitioner}),
				new Doctor("Laura", new[] {DoctorRole.Oncologist, DoctorRole.GeneralPractitioner})
			};

			var hospital = new Hospital(doctors, rooms);

			foreach(var patientRegistration in patientRegistrations)
			{
				var bookedConsultation = hospital.ScheduleConsultation(patientRegistration);

				Console.WriteLine($"Booked consultation for {patientRegistration.Patient.Name} with {bookedConsultation.Doctor.Name} in room {bookedConsultation.Room.Name} at {bookedConsultation.ScheduledDate.ToShortDateString()}");
			}
		}
	}
}
