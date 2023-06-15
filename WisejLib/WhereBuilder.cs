using Dapper;
using System;
using System.Text;
using Wisej.Web;

namespace WisejLib
{
    /// <summary>
    /// Separator between where clauses
    /// </summary>
    public enum WhereAppendMode { 
        /// <summary>
        /// Concatenate where clause parts by the sql command AND
        /// </summary>
        And,
        /// <summary>
        /// Concatenate where clause parts by the sql command OR
        /// </summary>
        Or,
        /// <summary>
        /// Concatenate where clause parts by using WhereBuilder's DefaultAppendMode
        /// </summary>
        Default
    }

    /// <summary>
    /// Helper class for generating where clauses in dynamic sql statements
    /// </summary>
    public class WhereBuilder
    {
        /// <summary>
        /// Default separator to separate the where clause parts. This is used when 
        /// WhereAppendMode.Default is passed to one of the Add methods
        /// </summary>
        public WhereAppendMode DefaultAppendMode { get; set; } = WhereAppendMode.And;

        /// <summary>
        /// Returns true is the WhereBuilder contains something
        /// </summary>
        public bool HasData => Content.Length > 0;

        /// <summary>
        /// Returns the sql where clause, preceeded by " where ".
        /// If the WhereBuilder doesn't contain any where clause, this returns string.Empty
        /// </summary>
        public string Where => Content.Length > 0 ? $" where {Content}" : string.Empty;

        /// <summary>
        /// THis is where the Add methods store the where clauses
        /// </summary>
        private readonly StringBuilder Content = new StringBuilder();

        /// <summary>
        /// Add another WhereBuilder's content to this one.
        /// This is usually hepful when AND and OR both exists in the same where expression
        /// </summary>
        public WhereBuilder Add(WhereBuilder whereBuilder, WhereAppendMode appendMode = WhereAppendMode.Default)
        {
            if (!whereBuilder.HasData)
                return this;
            if (appendMode == WhereAppendMode.Default)
                appendMode = DefaultAppendMode;
            return Add(whereBuilder.Content.ToString(), appendMode);
        }

        /// <summary>
        /// Adds a simple sql expression
        /// </summary>
        public WhereBuilder Add(string simpleWhere, WhereAppendMode appendMode = WhereAppendMode.Default)
        {
            if (string.IsNullOrEmpty(simpleWhere))
                return this;

            if (appendMode == WhereAppendMode.Default)
                appendMode = DefaultAppendMode;

            if (Content.Length > 0)
                _ = Content.Append($" {appendMode} ");
            _ = Content.Append($"({simpleWhere})");
            return this;
        }

        /// <summary>
        /// Adds a date range to the sql expression and defines parameters
        /// </summary>
        /// <param name="startDate">The minimum date</param>
        /// <param name="endDate">The maximum date</param>
        /// <param name="parameters">The DynamicParameters are filled with parameters @StartDate and @ENdDate</param>
        /// <param name="fieldName">The name of the table field that is checked against the start/end date</param>
        /// <param name="appendMode">How to concatenate the parts of the where clause</param>
        public WhereBuilder Add(DateTime? startDate, DateTime? endDate, DynamicParameters parameters, string fieldName, WhereAppendMode appendMode = WhereAppendMode.Default)
        {
            if (startDate is null && endDate is null)
                return this;

            if (appendMode == WhereAppendMode.Default)
                appendMode = DefaultAppendMode;

            const string startParam = "StartDate";
            const string endParam = "EndDate";
            if (startDate != null && endDate == null)
            {
                Add($"{fieldName} >= @{startParam}", appendMode);
                parameters.Add(startParam, value: ((DateTime)startDate).Date);
            }
            else if (startDate == null && endDate != null)
            {
                Add($"{fieldName} < @{endParam}", appendMode);
                parameters.Add(endParam, value: ((DateTime)endDate).Date.AddDays(1));
            }
            else if (startDate == endDate)
            {
                Add($"{fieldName} = @{startParam}", appendMode);
                parameters.Add(startParam, value: ((DateTime)startDate).Date);
            }
            else
            {
                Add($"{fieldName} >= @{startParam} and {fieldName} < @{endParam}", appendMode);
                parameters.Add(startParam, value: ((DateTime)startDate).Date);
                parameters.Add(endParam, value: ((DateTime)endDate).Date.AddDays(1));
            }

            return this;
        }

        /// <summary>
        /// Adds a date range from 2 DateTimePicker controls to the sql expression and defines parameters
        /// </summary>
        /// <param name="startPicker">The minimum date</param>
        /// <param name="endPicker">The maximum date</param>
        /// <param name="parameters">The DynamicParameters are filled with parameters @StartDate and @ENdDate</param>
        /// <param name="fieldName">The name of the table field that is checked against the start/end date</param>
        /// <param name="appendMode">How to concatenate the parts of the where clause</param>
        public WhereBuilder Add(DateTimePicker startPicker, DateTimePicker endPicker, DynamicParameters parameters,
            string fieldName, WhereAppendMode appendMode = WhereAppendMode.Default)
        {
            DateTime? startDate = (string.IsNullOrEmpty(startPicker.Text) ? default(DateTime?) : startPicker.Value);
            DateTime? endDate = (string.IsNullOrEmpty(endPicker.Text) ? default(DateTime?) : endPicker.Value);
            return Add(startDate, endDate, parameters, fieldName, appendMode);
        }
    }
}
