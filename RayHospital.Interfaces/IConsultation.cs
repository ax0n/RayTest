using System;

namespace RayHospital.Interfaces
{
	public interface IConsultation
	{
		IPatient Patient { get; }

		ITreater Treater { get; }

		ITreatmentLocation TreatmentLocation { get; }

		DateTime Date { get; }
	}
}