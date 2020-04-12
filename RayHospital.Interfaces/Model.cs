using System;
using System.Collections.Generic;

namespace RayHospital.Interfaces
{
	public interface ITreater : INameable
	{
		IEnumerable<DoctorRole> Roles { get; }
	}

	public enum DoctorRole
	{
		GeneralPractitioner,
		Oncologist
	}

	public class Doctor : ITreater
	{
		public Doctor(string name, IEnumerable<DoctorRole> roles)
		{
			Name = name;
			Roles = roles;
		}

		public string Name { get; }

		public IEnumerable<DoctorRole> Roles { get; }
	}


	public interface ITreatableCondition
	{
		DoctorRole RequiredDoctorRole { get; }

		TreatmentMachineCapability MinimumTreatmentMachineCapability { get; }
	}

	public class Flu : ITreatableCondition
	{
		public string Name { get; }

		public DoctorRole RequiredDoctorRole => DoctorRole.GeneralPractitioner;

		public TreatmentMachineCapability MinimumTreatmentMachineCapability => TreatmentMachineCapability.None;
	}

	public enum CancerTopography
	{
		Breast,
		HeadNeck
	}

	public class Cancer : ITreatableCondition
	{
		public Cancer(CancerTopography topography)
		{
			Topography = topography;
		}

		public string Name { get; }

		public CancerTopography Topography { get; }

		public DoctorRole RequiredDoctorRole => DoctorRole.Oncologist;

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

	public interface IPatient : INameable
	{
		ITreatableCondition TreatableCondition { get; }
	}

	public class Patient : IPatient
	{
		public Patient(string name, ITreatableCondition treatableCondition)
		{
			Name = name;
			TreatableCondition = treatableCondition;
		}

		public string Name { get; }

		public ITreatableCondition TreatableCondition { get; }
	}

	public enum TreatmentMachineCapability
	{
		None = 0,
		Simple = 1,
		Advanced = 2
	}

	public interface ITreatmentLocation : INameable 
	{
		TreatmentMachineCapability TreatmentMachineCapability { get; }
	}

	public interface INameable
	{
		string Name { get; }
	}

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

	public interface IRegistration<out T>
	{
		T Item { get; }

		DateTime TimeOfRegistration { get; }
	}
	
	public interface IConsultation
	{
		IPatient Patient { get; }

		ITreater Treater { get; }

		ITreatmentLocation TreatmentLocation { get; }

		DateTime Date { get; }
	}

	public class Consultation : IConsultation
	{
		public Consultation(IPatient patient, ITreater treater, ITreatmentLocation treatmentLocation, DateTime date)
		{
			Patient = patient;
			Treater = treater;
			TreatmentLocation = treatmentLocation;
			Date = date;
		}

		public IPatient Patient { get; }

		public ITreater Treater { get; }

		public ITreatmentLocation TreatmentLocation { get; }

		public DateTime Date { get; set; }
	}

	public class Hospital
	{
		public Hospital(IReadOnlyCollection<ITreater> doctors, IReadOnlyCollection<ITreatmentLocation> rooms, ICollection<Consultation> scheduledConsultations)
		{
			Doctors = doctors;
			Rooms = rooms;
			ScheduledScheduledConsultations = scheduledConsultations;
		}

		public IReadOnlyCollection<ITreater> Doctors { get; }

		public IReadOnlyCollection<ITreatmentLocation> Rooms { get; }

		public ICollection<Consultation> ScheduledScheduledConsultations { get; }
	}
}