namespace RayHospital.Interfaces
{
	public interface ITreatableCondition : INameable
	{
		TreaterQualifications RequiredTreaterQualifications { get; }

		TreatmentMachineCapability MinimumTreatmentMachineCapability { get; }
	}
}