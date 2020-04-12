using System.Collections.Generic;
using RayHospital.Interfaces;

namespace RayHospital.App
{
	public class Doctor : ITreater
	{
		public Doctor(string name, IEnumerable<TreaterQualifications> roles)
		{
			Name = name;
			Roles = roles;
		}

		public string Name { get; }

		public IEnumerable<TreaterQualifications> Roles { get; }
	}
}