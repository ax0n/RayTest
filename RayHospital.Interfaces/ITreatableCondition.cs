namespace RayHospital.Interfaces
{
	public interface ITreatableCondition : INameable
	{
		TreaterQualification RequiredTreaterQualification { get; }

		TreatmentMachineCapability MinimumTreatmentMachineCapability { get; }
	}
}