namespace Order.PubSub
{
    public class ReservedEvent
    {
        public int Id { get; set; }

        public ReservedEvent(int Id)
        {
            this.Id = Id;
        }
    }
}