using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core.BusinessRules
{
    internal class BusinessRuleViolation
    {
        public string ViolationMessage { get; }

        public BusinessRuleViolation(string violationMessage)
        {
            ViolationMessage = violationMessage;
        }
    }
}
