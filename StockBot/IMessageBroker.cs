using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBotFunction
{
    public interface IMessageBroker
    {
        void SendMessage(string message);
        void ReceiveMessage();

    }
}
