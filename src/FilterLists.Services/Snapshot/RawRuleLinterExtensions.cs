﻿namespace FilterLists.Services.Snapshot
{
    public static class RawRuleLinterExtensions
    {
        public static string LintRawRule(this string rule)
        {
            rule = rule?.TrimLeadingAndTrailingWhitespace();
            rule = rule?.DropIfEmpty();
            rule = rule?.DropIfComment();
            rule = rule?.DropIfTooLong();
            rule = rule?.DropIfContainsBackslashSingleQuote();
            rule = rule?.TrimSingleBackslashFromEnd();
            return rule;
        }

        private static string TrimLeadingAndTrailingWhitespace(this string rule)
        {
            char[] charsToTrim = {' ', '\t'};
            return rule.Trim(charsToTrim);
        }

        private static string DropIfEmpty(this string rule) => rule == "" ? null : rule;

        private static string DropIfComment(this string rule) =>
            rule.StartsWith(@"!") && !rule.StartsWith(@"!#") || rule.StartsWith(@"!##") ? null : rule;

        private static string DropIfTooLong(this string rule) => rule.Length > 8192 ? null : rule;

        private static string DropIfContainsBackslashSingleQuote(this string rule) =>
            rule.Contains(@"\'") ? null : rule;

        private static string TrimSingleBackslashFromEnd(this string rule) =>
            rule.EndsWith(@"\") && !rule.EndsWith(@"\\") ? rule.Remove(rule.Length - 1) : rule;
    }
}