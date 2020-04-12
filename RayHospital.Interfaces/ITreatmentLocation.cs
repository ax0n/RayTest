namespace RayHospital.Interfaces
{
	public interface ITreatmentLocation : INameable 
	{
		TreatmentMachineCapability TreatmentMachineCapability { get; }
	}
}