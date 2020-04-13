using System.Collections.Generic;

namespace RayHospital.Interfaces
{
	public interface ITreater : INameable
	{
		IEnumerable<TreaterQualification> Qualifications { get; }
	}
}