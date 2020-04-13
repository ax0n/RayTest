using System;
using RayHospital.Interfaces;

namespace RayHospital.App
{
	public class Registration<T> : IRegistration<T>
	{
		public Registration(T item, DateTime timeOfRegistration)
		{
			Item = item;
			TimeOfRegistration = timeOfRegistration;
		}

		public T Item { get; }

		public DateTime TimeOfRegistration { get; }
	}
}