namespace medical_project.Dtos
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
