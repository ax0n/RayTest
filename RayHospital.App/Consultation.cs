using System;
using RayHospital.Interfaces;

namespace RayHospital.App
{
	public class Consultation : IConsultation
	{
		public Consultation(IPatient patient, ITreater treater, ITreatmentLocation treatmentLocation, DateTime date)
		{
			Patient = patient;
			Treater = treater;
			TreatmentLocation = treatmentLocation;
			Date = date;
		}

		public IPatient Patient { get; }

		public ITreater Treater { get; }

		public ITreatmentLocation TreatmentLocation { get; }

		public DateTime Date { get; set; }
	}
}