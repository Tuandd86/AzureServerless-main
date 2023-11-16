namespace ProductReviewApp.Models
{
    public class Review
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
    }
}