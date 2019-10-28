// -----------------------------------------------------------------------
//  <copyright file="FluentModelValidator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-27 19:11</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;
using FluentValidation.Results;

using Stylet;


namespace OSharp.Wpf.FluentValidation
{
    public class FluentModelValidator<T> : IModelValidator<T>
    {
        private readonly IValidator<T> _validator;
        private T _subject;

        public FluentModelValidator(IValidator<T> validator)
        {
            _validator = validator;
        }

        /// <summary>
        /// Called by ValidatingModelBase, which passes in an instance of itself.
        /// This allows the IModelValidator to specialize to validating that particular ValidatingModelBase instance
        /// </summary>
        /// <param name="subject">Subject to initialize</param>
        public void Initialize(object subject)
        {
            _subject = (T)subject;
        }

        /// <summary>
        /// Validate a single property by name, and return an array of validation errors for that property (or null if validation was successful)
        /// </summary>
        /// <param name="propertyName">Property to validate, or <see cref="F:System.String.Empty" /> to validate the entire model</param>
        /// <returns>Array of validation errors, or null / empty if validation was successful</returns>
        public async Task<IEnumerable<string>> ValidatePropertyAsync(string propertyName)
        {
            ValidationResult result = await _validator.ValidateAsync(_subject, CancellationToken.None, propertyName);
            return result.Errors.Select(m => m.ErrorMessage);
        }

        /// <summary>
        /// Validate all properties, and return the results for all properties
        /// </summary>
        /// <remarks>
        /// Use a key of <see cref="F:System.String.Empty" /> to indicate validation errors for the entire model.
        /// If a property validates successfully, you MUST return a null entry for it in the returned dictionary!
        /// </remarks>
        /// <returns>A dictionary of property name =&gt; array of validation errors (or null if that property validated successfully)</returns>
        public async Task<Dictionary<string, IEnumerable<string>>> ValidateAllPropertiesAsync()
        {
            ValidationResult result = await _validator.ValidateAsync(_subject);
            return result.Errors.GroupBy(m => m.PropertyName).ToDictionary(m => m.Key, m => m.Select(n => n.ErrorMessage));
        }
    }
}