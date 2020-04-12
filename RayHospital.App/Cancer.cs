using System;
using RayHospital.Interfaces;

namespace RayHospital.App
{
	public class Cancer : ITreatableCondition
	{
		public Cancer(CancerTopography topography)
		{
			Topography = topography;
		}

		public string Name { get; }

		public CancerTopography Topography { get; }

		public TreaterQualifications RequiredTreaterQualifications => TreaterQualifications.Oncologist;

		public TreatmentMachineCapability MinimumTreatmentMachineCapability
		{
			get
			{
				switch(Topography)
				{
					case CancerTopography.Breast:
						return TreatmentMachineCapability.Simple;
					case CancerTopography.HeadNeck:
						return TreatmentMachineCapability.Advanced;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}