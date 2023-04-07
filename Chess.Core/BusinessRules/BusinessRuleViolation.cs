using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core.BusinessRules
{
    public class BusinessRuleViolation
    {
        public string ViolationMessage { get; }

        public BusinessRuleViolation(string violationMessage)
        {
            ViolationMessage = violationMessage;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (!(obj is BusinessRuleViolation brv))
                return false;

            return brv.ViolationMessage.Equals(ViolationMessage);
        }

        public override int GetHashCode()
        {
            return this.ViolationMessage.GetHashCode();
        }
    }
}
