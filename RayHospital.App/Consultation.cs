using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RayHospital.Interfaces;

namespace RayHospital.App
{
	public static class ConsultationModule
	{
		public static Consultation CreateConsultation
		(
			IRegistration<IPatient> patientRegistration,
			IEnumerable<Consultation> scheduledConsultations,
			IEnumerable<ITreater> doctors,
			IEnumerable<ITreatmentLocation> rooms
		)
		{
			var requestedConsultationDateOffset = 1;

			while(true)
			{
				var requestedDate = patientRegistration.TimeOfRegistration.AddDays(requestedConsultationDateOffset).Date;

				var sameDayConsultations = scheduledConsultations.Where(consultation => consultation.ScheduledDate.Date.Equals(requestedDate)).ToList();

				var busyDoctors = sameDayConsultations.Select(sameDayConsultation => sameDayConsultation.Doctor);
				var firstAvailableViableDoctor = doctors.Except(busyDoctors).FirstOrDefault(doctor => doctor.Roles.Contains(patientRegistration.Item.TreatableCondition.RequiredDoctorRole));

				if(!ReferenceEquals(firstAvailableViableDoctor, null))
				{
					var busyRooms = sameDayConsultations.Select(sameDayConsultation => sameDayConsultation.Room);
					var firstAvailableViableRoom = rooms.Except(busyRooms).FirstOrDefault(room => room.TreatmentMachineCapability >= patientRegistration.Item.TreatableCondition.MinimumTreatmentMachineCapability);

					if(!ReferenceEquals(firstAvailableViableRoom, null))
					{
						return new Consultation(patientRegistration.Item, firstAvailableViableDoctor, firstAvailableViableRoom, requestedDate);
					}
				}

				requestedConsultationDateOffset++;
			}
		}

		public static IEnumerable<Consultation> CreateConsultations(IEnumerable<IRegistration<IPatient>> patientRegistrations, Hospital hospital)
		{
			var previouslyScheduledConsultations = hospital.ScheduledScheduledConsultations;
			var scheduledConsultations = new List<Consultation>(previouslyScheduledConsultations);

			foreach(var patientRegistration in patientRegistrations)
			{
				var newConsultation = CreateConsultation(patientRegistration, scheduledConsultations, hospital.Doctors, hospital.Rooms);
				scheduledConsultations.Add(newConsultation);
			}

			var newScheduledConsultations = scheduledConsultations.Except(previouslyScheduledConsultations);
			return newScheduledConsultations;
		}
	}
}
