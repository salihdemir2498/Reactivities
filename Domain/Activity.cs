namespace Domain
{
    public class Activity
    {
        public Guid Id { get; set; } 
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public bool IsCancelled { get; set; }
        public ICollection<ActivityAttendee> Attendees { get; set; } = new List<ActivityAttendee>();//Bu deðiþiklikle, Activity varlýðý içindeki attendees koleksiyonu, herhangi bir öðe eklemeye hazýr bir þekilde baþlatýlmýþ olacaktýr. Artýk bu koleksiyona öðeler ekleyebilir ve null reference hatasýný önleriz.

    }
}