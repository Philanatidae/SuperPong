using System;
using SuperPong.Input;

namespace SuperPong
{
    public class AIPlayer : Player
    {
        public AIInputMethod AIInputMethod;

        public AIPlayer(int index, string name)
            : base(index, name, new AIInputMethod())
        {
            AIInputMethod = InputMethod as AIInputMethod;
        }
    }
}
