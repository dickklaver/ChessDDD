namespace Chess.Core.BusinessRules
{
    internal class AndBusinessRule : BusinessRule
    {
        private readonly BusinessRule firstRule;
        private readonly BusinessRule secondRule;

        public AndBusinessRule(BusinessRule firstRule, BusinessRule secondRule)
        {
            this.firstRule = firstRule;
            this.secondRule = secondRule;
        }

        public override IEnumerable<BusinessRuleViolation> CheckRule()
        {
            return firstRule.CheckRule().Concat(secondRule.CheckRule());
        }
    }
}
