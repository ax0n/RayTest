using System;
using System.Collections.Generic;
using System.Linq;
using RayHospital.Interfaces;

namespace RayHospital.Lib
{
	public static class Scheduling
	{
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
				var firstAvailableViableDoctor = doctors.Except(busyDoctors).FirstOrDefault(doctor => doctor.Qualifications.Contains(patientRegistration.Item.TreatableCondition.RequiredTreaterQualification));

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
