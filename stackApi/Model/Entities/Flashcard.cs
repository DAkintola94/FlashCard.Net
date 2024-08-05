namespace stackApi.Model.Entities
{
    public class Flashcard
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsVisible { get; set; }

        public bool CorrectAnswer { get; set; }
        public bool IncorrectAnswer { get; set; }
        

    }
}
