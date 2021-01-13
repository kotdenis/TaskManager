using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfWorkManager.Infrastrucure
{
    public interface IMessageSender
    {
        string Label { get; set; }
        string Message { get; set; }
        int EnumCount { get; set; }
        Action CloseWindow { get; set; }
    }

    public class MessageSender : IMessageSender
    {
        public string Label { get; set; }
        public string Message { get; set; }
        public int EnumCount { get; set; }
        public Action CloseWindow { get; set; }
    }
}
