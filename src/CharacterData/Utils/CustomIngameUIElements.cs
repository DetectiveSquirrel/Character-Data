using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoeHUD.Controllers;
using PoeHUD.Poe;
using PoeHUD.Poe.RemoteMemoryObjects;

namespace Flipping_Bot.RemoteMemoryObjects
{
    public class CustomIngameUIElements : IngameUIElements
    {
        public CustomIngameUIElements(long address)
        {
            Address = address;
            Game = GameController.Instance.Game;
            M = GameController.Instance.Memory;
        }

        public Element PartyList => ReadObjectAt<Element>(0x3A0);
        public Element ChatWindow => ReadObjectAt<Element>(0x3F8);
        public Element PlayerTradeWindow => ReadObjectAt<Element>(0x648);
        public Element WaitingTradeWindow => ReadObjectAt<Element>(0x7A8);
    }
}