namespace RayHospital.Interfaces
{
	public interface IPatient : INameable
	{
		ITreatableCondition TreatableCondition { get; }
	}
}