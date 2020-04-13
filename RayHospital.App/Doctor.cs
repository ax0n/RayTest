using System.Collections.Generic;
using RayHospital.Interfaces;

namespace RayHospital.App
{
	public class Doctor : ITreater
	{
		public Doctor(string name, IEnumerable<TreaterQualification> roles)
		{
			Name = name;
			Qualifications = roles;
		}

		public string Name { get; }

		public IEnumerable<TreaterQualification> Qualifications { get; }
	}
}