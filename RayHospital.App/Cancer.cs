using RayHospital.Interfaces;

namespace RayHospital.App
{
	public abstract class Cancer : ITreatableCondition
	{
		protected Cancer(string name, CancerTopography topography, TreatmentMachineCapability minimumTreatmentMachineCapability)
		{
			Name = name;
			Topography = topography;
			MinimumTreatmentMachineCapability = minimumTreatmentMachineCapability;
		}

		public string Name { get; }

		public CancerTopography Topography { get; }

		public TreatmentMachineCapability MinimumTreatmentMachineCapability { get; }

		public TreaterQualification RequiredTreaterQualification => TreaterQualification.Oncologist;
	}

	public class BreastCancer : Cancer
	{
		public BreastCancer() : base("Breast cancer", CancerTopography.Breast, TreatmentMachineCapability.Simple)
		{

		}
	}

	public class HeadNeckCancer : Cancer
	{
		public HeadNeckCancer() : base("Head & Neck cancer", CancerTopography.HeadNeck, TreatmentMachineCapability.Advanced)
		{

		}
	}
}