using System;
namespace SuperPong.Processes
{
    public class DelegateCommand : Command
    {
        readonly Action _action;

        public DelegateCommand(Action action)
        {
            _action = action;
        }

        protected override void OnTrigger()
        {
            _action.Invoke();
        }
    }
}
