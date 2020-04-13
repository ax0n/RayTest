using RayHospital.Interfaces;

namespace RayHospital.App
{
	public class Room : ITreatmentLocation
	{
		public Room(string name, TreatmentMachineCapability treatmentMachineCapability)
		{
			Name = name;
			TreatmentMachineCapability = treatmentMachineCapability;
		}

		public string Name { get; }

		public TreatmentMachineCapability TreatmentMachineCapability { get; }
	}
}