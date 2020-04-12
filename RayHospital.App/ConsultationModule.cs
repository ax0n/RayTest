using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RayHospital.Interfaces;

namespace RayHospital.App
{
	public static class ConsultationModule
	{
		public delegate void ProduceConsultation(IPatient patient, ITreater treater, ITreatmentLocation treatmentLocation, DateTime scheduledDate);

		public static void ScheduleConsultation
		(
			Action<IPatient, ITreater, ITreatmentLocation, DateTime> produceConsultation,
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
				var firstAvailableViableDoctor = doctors.Except(busyDoctors).FirstOrDefault(doctor => doctor.Roles.Contains(patientRegistration.Item.TreatableCondition.RequiredDoctorRole));

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

		//public static IEnumerable<IConsultation> CreateConsultations(IEnumerable<IRegistration<IPatient>> patientRegistrations, Hospital hospital, DateTime scheduleFrom, DateTime scheduleTo)
		//{
		//	var previouslyScheduledConsultations = hospital.ScheduledScheduledConsultations;
		//	var scheduledConsultations = new List<Consultation>(previouslyScheduledConsultations);

		//	foreach(var patientRegistration in patientRegistrations)
		//	{
		//		var newConsultation = CreateFirstAvailableConsultation( patientRegistration, scheduledConsultations, hospital.Doctors, hospital.Rooms, scheduleFrom, scheduleTo);
		//		scheduledConsultations.Add(newConsultation);
		//	}

		//	var newScheduledConsultations = scheduledConsultations.Except(previouslyScheduledConsultations);
		//	return newScheduledConsultations;
		//}
	}
}
