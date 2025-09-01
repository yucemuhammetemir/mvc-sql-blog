namespace blogv1.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int BlogId { get; set; } //hangisini yaptıysak ona


        public DateTime PublishDate { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public string Message { get; set; }


    }
}
