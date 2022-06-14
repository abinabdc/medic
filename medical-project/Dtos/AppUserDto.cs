namespace medical_project.Dtos
{
    public class AppUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string BloodGroup { get; set; } // O+ve, O-ve, AB+ve
        public int Weight { get; set; } //in kg
        public int Height { get; set; } // in cm
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DOB { get; set; }

    }
}
