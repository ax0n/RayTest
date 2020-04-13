using RayHospital.Interfaces;

namespace RayHospital.App
{
	public class Flu : ITreatableCondition
	{
		public string Name { get; }

		public TreaterQualification RequiredTreaterQualification => TreaterQualification.GeneralPractitioner;

		public TreatmentMachineCapability MinimumTreatmentMachineCapability => TreatmentMachineCapability.None;
	}
}