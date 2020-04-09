namespace RayHospital.Resources
{
  public class PatientRegistrations
  {
      public static (string Name, string Condition, string Topography, int OffsetDays)[] Patients => new[]
      {
        ("Lucas", "Cancer", "HeadNeck", 0),
        ("Sandra", "Cancer", "HeadNeck", 0),
        ("Regina","Cancer", "Breast", 1),
        ("Jane","Flu", null, 1),
        ("Jack","Flu", null, 2)
      };
  }
}