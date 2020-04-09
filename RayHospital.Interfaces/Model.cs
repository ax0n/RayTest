using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RayHospital.Interfaces
{

	public enum DoctorRole
	{
		GeneralPractitioner,
		Oncologist
	}

	public class Doctor
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


	public class Patient
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

	public class Room
	{
		public Room(string name, TreatmentMachineCapability treatmentMachineCapability)
		{
			Name = name;
			TreatmentMachineCapability = treatmentMachineCapability;

		}

		public string Name { get; }

		public TreatmentMachineCapability TreatmentMachineCapability { get; }
	}
	

	public class PatientRegistration
	{
		public PatientRegistration(Patient patient, DateTime timeOfRegistration)
		{
			Patient = patient;
			TimeOfRegistration = timeOfRegistration;
		}

		public Patient Patient { get; }

		public DateTime TimeOfRegistration { get; }
	}


	public class Consultation
	{
		public Consultation(Patient patient, Doctor doctor, Room room, DateTime scheduledDate)
		{
			Patient = patient;
			Doctor = doctor;
			Room = room;
			ScheduledDate = scheduledDate;
		}

		public Patient Patient { get; }

		public Doctor Doctor { get; }

		public Room Room { get; }

		public DateTime ScheduledDate { get; set; }
	}

	
	public interface IConsultationScheduler
	{
		Consultation ScheduleConsultation(PatientRegistration patientRegistration);

		IEnumerable<Consultation> ScheduledConsultations { get; }
	}

	public class Hospital : IConsultationScheduler
	{
		private readonly IEnumerable<Doctor> _doctors;
		private readonly IEnumerable<Room> _rooms;
		private readonly ICollection<Consultation> _consultations;

		public Hospital(IEnumerable<Doctor> doctors, IEnumerable<Room> rooms)
		{
			_rooms = rooms;
			_doctors = doctors;
			_consultations = new List<Consultation>();
		}

		public IEnumerable<Doctor> Doctors { get; }

		public IEnumerable<Room> Rooms { get; }

		public IEnumerable<Consultation> ScheduledConsultations => _consultations;

		public Consultation ScheduleConsultation(PatientRegistration patientRegistration)
		{
			var requestedConsultationDateOffset = 1;
			
			while(true)
			{
				var requestedDate = patientRegistration.TimeOfRegistration.AddDays(requestedConsultationDateOffset).Date;

				var sameDayConsultations = ScheduledConsultations.Where(consultation => consultation.ScheduledDate.Date.Equals(requestedDate)).ToList();

				var firstAvailableDoctor = _doctors.Except(sameDayConsultations.Select(sameDayConsultation => sameDayConsultation.Doctor)).FirstOrDefault(doctor => doctor.Roles.Contains(patientRegistration.Patient.TreatableCondition.RequiredDoctorRole));

				if(firstAvailableDoctor != null)
				{
					var firstAvailableRoom = _rooms.Except(sameDayConsultations.Select(sameDayConsultation => sameDayConsultation.Room)).FirstOrDefault(room => room.TreatmentMachineCapability >= patientRegistration.Patient.TreatableCondition.MinimumTreatmentMachineCapability);

					if(firstAvailableRoom != null)
					{
						var consultation = new Consultation(patientRegistration.Patient, firstAvailableDoctor, firstAvailableRoom, requestedDate);
						_consultations.Add(consultation);
						return consultation;
					}
				}

				requestedConsultationDateOffset++;
			}
		}
	}
}