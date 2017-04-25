using System;
using SuperPong.Input;

namespace SuperPong
{
    public class Player
    {
        public int Index
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public InputMethod InputMethod
        {
            get;
            private set;
        }

        public Player(int index, string name) : this(index, name, null)
        {
        }

        public Player(int index, string name, InputMethod inputMethod)
        {
            Index = index;
            Name = name;
            InputMethod = inputMethod;
        }
    }
}
