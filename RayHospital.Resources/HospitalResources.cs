namespace RayHospital.Resources
{
  public class HospitalResources
  {
      public static (string Name, string[] Roles)[] Doctors => new[]
      {
        ("John", new [] { "Oncologist" }),
        ("Anna", new [] { "GeneralPractitioner" }),
        ("Laura", new [] { "Oncologist", "GeneralPractitioner" })
      };

      public static (string Name, string MachineName)[] TreatmentRooms => new[]
      {
        ("RoomOne", null),
        ("RoomTwo", null),
        ("RoomThree", "MachineA"),
        ("RoomFour", "MachineB")
      };

      public static (string Name, string Capability)[] TreatmentMachines => new[]
      {
        ("MachineA", "Advanced"),
        ("MachineB", "Simple")
      };
  }
}