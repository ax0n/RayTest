using RayHospital.Interfaces;

namespace RayHospital.App
{
	public class Flu : ITreatableCondition
	{
		public string Name { get; }

		public TreaterQualifications RequiredTreaterQualifications => TreaterQualifications.GeneralPractitioner;

		public TreatmentMachineCapability MinimumTreatmentMachineCapability => TreatmentMachineCapability.None;
	}
}