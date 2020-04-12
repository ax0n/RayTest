using System.Collections.Generic;

namespace RayHospital.Interfaces
{
	public interface ITreater : INameable
	{
		IEnumerable<TreaterQualifications> Roles { get; }
	}
}