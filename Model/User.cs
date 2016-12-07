namespace Model
{
    public class User
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
        public UserRole UserRole { get; set; }
    }
}
