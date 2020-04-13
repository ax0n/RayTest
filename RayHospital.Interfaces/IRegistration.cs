using System;

namespace RayHospital.Interfaces
{
	public interface IRegistration<out T>
	{
		T Item { get; }

		DateTime TimeOfRegistration { get; }
	}
}