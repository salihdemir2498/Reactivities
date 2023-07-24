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
        public ICollection<ActivityAttendee> Attendees { get; set; } = new List<ActivityAttendee>();//Bu de�i�iklikle, Activity varl��� i�indeki attendees koleksiyonu, herhangi bir ��e eklemeye haz�r bir �ekilde ba�lat�lm�� olacakt�r. Art�k bu koleksiyona ��eler ekleyebilir ve null reference hatas�n� �nleriz.

    }
}