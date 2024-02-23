namespace PatientPortalSystem.Models
{
	public class User
	{
		public int Id { get; set; }
		public string NameOf {  get; set; }
		public DateOnly DOB { get; set; }
		public string Username {  get; set; }
		public string Password { get; set; }
		public string Address {  get; set; }
		public string Email { get; set; }	
		public string Phone { get; set; }
		public DateTime DateRegistered { get; set; } = DateTime.Now;
		public string SSN { get; set; }
	}
}
