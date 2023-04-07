namespace Chess.Core.BusinessRules
{
    internal abstract class BusinessRule
    {
        public abstract IEnumerable<BusinessRuleViolation> CheckRule();

        public void ThrowIfNotSatisfied()
        {
            IEnumerable<BusinessRuleViolation> violations = CheckRule();

            if (violations != null && violations.Any())
            {
                throw new BusinessRuleViolationException(violations);
            }
        }

        public static void ThrowIfNotSatisfied(BusinessRule rule)
        {
            rule.ThrowIfNotSatisfied();
        }

        public BusinessRule And(BusinessRule rule)
        {
            return new AndBusinessRule(this, rule);
        }

        public static BusinessRule operator &(BusinessRule firstRule, BusinessRule secondRule)
        {
            return new AndBusinessRule(firstRule, secondRule);
        }

        public static bool operator true(BusinessRule rule)
        {
            return false;
        }

        public static bool operator false(BusinessRule rule)
        {
            return false;
        }
    }
}
