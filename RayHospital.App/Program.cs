using RayHospital.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;

namespace RayHospital.App
{
	internal class Program
	{
		private static void Main(string[] commandlineArguments)
		{
			#region Capture

			var startDate = DateTime.Today;

			var patientRegistrations = new ConcurrentQueue<IRegistration<IPatient>>
			(
				new[]
				{
					new Registration<IPatient>(new Patient("Lucas", new Cancer(CancerTopography.HeadNeck)), startDate.AddDays(0)),
					new Registration<IPatient>(new Patient("Sandra", new Cancer(CancerTopography.HeadNeck)), startDate.AddDays(0)),
					new Registration<IPatient>(new Patient("Regina", new Cancer(CancerTopography.Breast)), startDate.AddDays(1)),
					new Registration<IPatient>(new Patient("Jane", new Flu()), startDate.AddDays(1)),
					new Registration<IPatient>(new Patient("Jack", new Flu()), startDate.AddDays(2)),
				}
			);

			var rooms = new[]
			{
				new Room("RoomOne", TreatmentMachineCapability.None),
				new Room("RoomTwo", TreatmentMachineCapability.None),
				new Room("RoomThree", TreatmentMachineCapability.Advanced),
				new Room("RoomFour", TreatmentMachineCapability.Simple)
			};

			var doctors = new[]
			{
				new Doctor("John", new[] { ITreaterQualifications.Oncologist }),
				new Doctor("Anna", new[] { ITreaterQualifications.GeneralPractitioner }),
				new Doctor("Laura", new[] { ITreaterQualifications.Oncologist, ITreaterQualifications.GeneralPractitioner })
			};

			var consultations = new List<IConsultation>
			{
				new Consultation(new Patient("Ray", new Cancer(CancerTopography.HeadNeck)), doctors.First(), rooms.First(), startDate.AddDays(1))
			};

			#endregion

			#region Side effects

			void ConsultationFound(IPatient patient, ITreater treater, ITreatmentLocation treatmentLocation, DateTime date)
			{
				consultations.Add(new Consultation(patient, treater, treatmentLocation, date));
				Console.WriteLine($"Consultation scheduled for {patient.Name} with {treater.Name} in room {treatmentLocation.Name} at {date.ToShortDateString()}");
			}

			#endregion

			while(patientRegistrations.TryDequeue(out var patientRegistration))
			{
				ScheduleConsultation(ConsultationFound, patientRegistration, consultations, doctors, rooms, startDate, startDate.AddYears(1));
			}
		}

		public delegate void ProduceConsultation(IPatient patient, ITreater treater, ITreatmentLocation treatmentLocation, DateTime scheduledDate);

		public static void ScheduleConsultation
		(
			ProduceConsultation produceConsultation,
			IRegistration<IPatient> patientRegistration,
			IEnumerable<IConsultation> scheduledConsultations,
			IEnumerable<ITreater> doctors,
			IEnumerable<ITreatmentLocation> rooms,
			DateTime scheduleFrom,
			DateTime scheduleTo
		)
		{
			var earliestSchedulableDate = patientRegistration.TimeOfRegistration.AddDays(1).Date;
			var date = scheduleFrom.Date < earliestSchedulableDate ? earliestSchedulableDate.Date : scheduleFrom.Date;

			while(date <= scheduleTo)
			{
				var sameDayConsultations = scheduledConsultations.Where(consultation => consultation.Date.Date.Equals(date)).ToList();

				var busyDoctors = sameDayConsultations.Select(sameDayConsultation => sameDayConsultation.Treater);
				var firstAvailableViableDoctor = doctors.Except(busyDoctors).FirstOrDefault(doctor => doctor.Roles.Contains(patientRegistration.Item.TreatableCondition.RequiredTreaterQualifications));

				if(!ReferenceEquals(firstAvailableViableDoctor, null))
				{
					var busyRooms = sameDayConsultations.Select(sameDayConsultation => sameDayConsultation.TreatmentLocation);
					var firstAvailableViableRoom = rooms.Except(busyRooms).FirstOrDefault(room => room.TreatmentMachineCapability >= patientRegistration.Item.TreatableCondition.MinimumTreatmentMachineCapability);

					if(!ReferenceEquals(firstAvailableViableRoom, null))
					{
						produceConsultation(patientRegistration.Item, firstAvailableViableDoctor, firstAvailableViableRoom, date);
						return;
					}
				}

				date = date.AddDays(1);
			}
		}
	}
}
