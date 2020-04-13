using RayHospital.Interfaces;

namespace RayHospital.App
{
	public class Flu : ITreatableCondition
	{
		public string Name => "Flu";

		public TreaterQualification RequiredTreaterQualification => TreaterQualification.GeneralPractitioner;

		public TreatmentMachineCapability MinimumTreatmentMachineCapability => TreatmentMachineCapability.None;
	}
}