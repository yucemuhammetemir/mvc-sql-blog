namespace blogv1.Models
{
    public class Contact
    {
        public int Id { get; set; }



        public DateTime CreaatedAt { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public string Message { get; set; }

    }
}
