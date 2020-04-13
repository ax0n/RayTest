using RayHospital.Interfaces;
using System;
using System.Collections.Generic;
using Xunit;

namespace RayHospital.Lib.Tests
{
	public class ScheduleConsultationTests
	{
		#region TestTypes

		private class FakeCondition : ITreatableCondition
		{
			public FakeCondition(string name, TreaterQualification requiredTreaterQualification, TreatmentMachineCapability minimumTreatmentMachineCapability)
			{
				Name = name;
				RequiredTreaterQualification = requiredTreaterQualification;
				MinimumTreatmentMachineCapability = minimumTreatmentMachineCapability;
			}

			public string Name { get; }

			public TreaterQualification RequiredTreaterQualification { get; }

			public TreatmentMachineCapability MinimumTreatmentMachineCapability { get; }
		}

		private class FakePatient : IPatient
		{
			public FakePatient(string name, ITreatableCondition treatableCondition)
			{
				Name = name;
				TreatableCondition = treatableCondition;
			}

			public string Name { get; }

			public ITreatableCondition TreatableCondition { get; }
		}

		private class FakePatientRegistration : IRegistration<IPatient>
		{
			public FakePatientRegistration(IPatient item, DateTime timeOfRegistration)
			{
				Item = item;
				TimeOfRegistration = timeOfRegistration;
			}

			public IPatient Item { get; }

			public DateTime TimeOfRegistration { get; }
		}

		private class FakeDoctor : ITreater
		{
			public FakeDoctor(string name, IEnumerable<TreaterQualification> qualifications)
			{
				Name = name;
				Qualifications = qualifications;
			}

			public string Name { get; }

			public IEnumerable<TreaterQualification> Qualifications { get; }
		}

		private class FakeRoom : ITreatmentLocation
		{
			public FakeRoom(string name, TreatmentMachineCapability treatmentMachineCapability)
			{
				Name = name;
				TreatmentMachineCapability = treatmentMachineCapability;
			}

			public string Name { get; }

			public TreatmentMachineCapability TreatmentMachineCapability { get; }
		}

		private class FakeConsultation : IConsultation
		{
			public FakeConsultation(IPatient patient, ITreater treater, ITreatmentLocation treatmentLocation, DateTime date)
			{
				Patient = patient;
				Treater = treater;
				TreatmentLocation = treatmentLocation;
				Date = date;
			}

			public IPatient Patient { get; }
		
			public ITreater Treater { get; }
			
			public ITreatmentLocation TreatmentLocation { get; }
			
			public DateTime Date { get; }
		}

		#endregion

		[Theory]
		[InlineData(-100)]
		[InlineData(-1)]
		[InlineData(0)]
		[InlineData(1)]
		public void Consultation_can_only_be_scheduled_at_least_one_day_after_registration_date(int registrationDateOffsetInDays)
		{
			var registrationDate = DateTime.UtcNow.Date;
			var treaterQualification = TreaterQualification.GeneralPractitioner;
			var treatmentMachineCapability = TreatmentMachineCapability.Simple;

			var patientRegistration = new FakePatientRegistration(new FakePatient("FakePatientA", new FakeCondition("FakeCondition", treaterQualification, treatmentMachineCapability)), registrationDate);
			var scheduledConsultations = new List<IConsultation>();
			var doctors = new[] {new FakeDoctor("FakeDoctor", new[] {treaterQualification})};
			var rooms = new[] {new FakeRoom("FakeRoom", treatmentMachineCapability)};

			Scheduling.ScheduleConsultation
			(
				ProduceConsultation,
				patientRegistration, 
				scheduledConsultations,
				doctors,
				rooms, 
				registrationDate.AddDays(registrationDateOffsetInDays), 
				registrationDate.AddYears(1)
			);

			void ProduceConsultation(IPatient patient, ITreater treater, ITreatmentLocation treatmentLocation, DateTime date)
			{
				Assert.True((date.Date - registrationDate).TotalDays >= 1);
			}
		}

		[Fact]
		public void Consultation_treater_may_not_be_double_booked()
		{
			var registrationDate = DateTime.UtcNow.Date;
			var collidingDate = registrationDate.AddDays(10).Date;
			var treaterQualification = TreaterQualification.GeneralPractitioner;
			var treatmentMachineCapability = TreatmentMachineCapability.Simple;

			var patientRegistration = new FakePatientRegistration(new FakePatient("FakePatientA", new FakeCondition("FakeCondition", treaterQualification, treatmentMachineCapability)), registrationDate);
			var fakeDoctor = new FakeDoctor("FakeDoctor", new[] {treaterQualification});
			var doctors = new[] { fakeDoctor };
			var rooms = new[] { new FakeRoom("FakeRoomA", treatmentMachineCapability) };
			var scheduledConsultations = new List<IConsultation>
			{
				new FakeConsultation(new FakePatient("FakePatientA", new FakeCondition("FakeCondition", treaterQualification, treatmentMachineCapability)), fakeDoctor, new FakeRoom("FakeRoomB", treatmentMachineCapability), collidingDate)
			};

			Scheduling.ScheduleConsultation
			(
				ProduceConsultation,
				patientRegistration,
				scheduledConsultations,
				doctors,
				rooms,
				collidingDate,
				collidingDate.AddYears(1)
			);

			void ProduceConsultation(IPatient patient, ITreater treater, ITreatmentLocation treatmentLocation, DateTime date)
			{
				Assert.True(date.Date != collidingDate.Date);
			}
		}

		[Fact]
		public void Consultation_treatment_location_may_not_be_double_booked()
		{
			var registrationDate = DateTime.UtcNow.Date;
			var collidingDate = registrationDate.AddDays(10).Date;
			var treaterQualification = TreaterQualification.GeneralPractitioner;
			var treatmentMachineCapability = TreatmentMachineCapability.Simple;

			var patientRegistration = new FakePatientRegistration(new FakePatient("FakePatientA", new FakeCondition("FakeCondition", treaterQualification, treatmentMachineCapability)), registrationDate);
			var doctors = new[] { new FakeDoctor("FakeDoctorA", new[] { treaterQualification }) };
			var fakeRoom = new FakeRoom("FakeRoomA", treatmentMachineCapability);
			var rooms = new[] { fakeRoom };
			var scheduledConsultations = new List<IConsultation>
			{
				new FakeConsultation(new FakePatient("FakePatientA", new FakeCondition("FakeCondition", treaterQualification, treatmentMachineCapability)), new FakeDoctor("FakeDoctorB", new[] { treaterQualification }), fakeRoom, collidingDate)
			};

			Scheduling.ScheduleConsultation
			(
				ProduceConsultation,
				patientRegistration,
				scheduledConsultations,
				doctors,
				rooms,
				collidingDate,
				collidingDate.AddYears(1)
			);

			void ProduceConsultation(IPatient patient, ITreater treater, ITreatmentLocation treatmentLocation, DateTime date)
			{
				Assert.True(date.Date != collidingDate.Date);
			}
		}

	}
}
