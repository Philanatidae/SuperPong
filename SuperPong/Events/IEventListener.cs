namespace Events
{
    public interface IEventListener
    {

        bool Handle(IEvent evt);

    }
}
