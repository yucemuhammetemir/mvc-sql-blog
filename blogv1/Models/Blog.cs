﻿namespace blogv1.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public  string Descripton { get; set; }

        public string ImageUrl { get; set; }
        public DateTime PublishDate { get; set; }

        public string Tags { get; set; }

        public int LikeCount { get; set; }
        public int CommentCount { get; set; }

        public int ViewCount { get; set; }
        public int Status { get; set; }

    }
}
