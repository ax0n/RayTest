using RayHospital.Interfaces;

namespace RayHospital.App
{
	public class Patient : IPatient
	{
		public Patient(string name, ITreatableCondition treatableCondition)
		{
			Name = name;
			TreatableCondition = treatableCondition;
		}

		public string Name { get; }

		public ITreatableCondition TreatableCondition { get; }
	}
}