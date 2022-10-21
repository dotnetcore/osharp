// Author:
// Leszek Ciesielski (skolima@gmail.com)
//
// (C) 2011 Cognifide
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Dynamic;
using System.Linq.Expressions;


namespace OSharp.Dynamic
{
    /// <summary>
    /// Exposes hidden (private/protected/internal) members of an
    /// <see langword="object"/> or <see cref="Type"/> through a <see langword="dynamic"/> wrapper.
    /// </summary>
    public class Exposed : DynamicObject
    {
        /// <summary>
        /// The <see langword="object"/> that is being exposed.
        /// <see langword="null"/> if static members of a <see cref="Type"/> are being exposed.
        /// </summary>
        private readonly object value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Exposed"/> class. 
        /// Creates a new wrapper for accessing members of subject.
        /// </summary>
        /// <param name="subject">
        /// The object which will have it's members exposed.
        /// </param>
        /// <returns>
        /// A new wrapper around the subject.
        /// </returns>
        private Exposed(object subject)
        {
            value = subject;
            SubjectType = subject.GetType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exposed"/> class. 
        /// Creates a new wrapper for accessing hidden static members of a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// The <see cref="Type"/> which will have it's static members exposed.
        /// </param>
        /// <returns>
        /// A new wrapper around a <see cref="Type"/>.
        /// </returns>
        private Exposed(Type type)
        {
            SubjectType = type;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of the exposed object.
        /// </summary>
        internal Type SubjectType { get; private set; }

        /// <summary>
        /// Creates a new wrapper for accessing members of subject.
        /// </summary>
        /// <param name="subject">
        /// The object which will have it's members exposed.
        /// </param>
        /// <returns>
        /// A new wrapper around the subject.
        /// </returns>
        public static dynamic From(object subject)
        {
            return new Exposed(subject);
        }

        /// <summary>
        /// Creates a new wrapper for accessing hidden static members of a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// The <see cref="Type"/> which will have it's static members exposed.
        /// </param>
        /// <returns>
        /// A new wrapper around a <see cref="Type"/>.
        /// </returns>
        public static dynamic From(Type type)
        {
            return new Exposed(type);
        }

        /// <summary>
        /// Creates a new wrapper for accessing members of a new instance of <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// The <see cref="Type"/> of which an instance will have it's members exposed.
        /// </param>
        /// <returns>
        /// A new wrapper around a new instance of <see cref="Type"/>.
        /// </returns>
        public static dynamic New(Type type)
        {
            return new Exposed(Activator.CreateInstance(type));
        }

        /// <summary>
        /// Returns the <see cref="DynamicMetaObject"/> responsible for binding operations performed on this object.
        /// </summary>
        /// <param name="parameter">
        /// The expression tree representation of the runtime value.
        /// </param>
        /// <returns>
        /// The <see cref="DynamicMetaObject"/> to bind this object.
        /// </returns>
        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new MetaObject(parameter, this, value == null);
        }
    }
}
