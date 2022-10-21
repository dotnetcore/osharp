// Author:
// Leszek Ciesielski (skolima@gmail.com)
// Manuel Josupeit-Walter (josupeit-walter@cis-gmbh.de)
//
// (C) 2013 Cognifide
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
using System.Reflection;


namespace OSharp.Dynamic
{
    /// <summary>
    /// Represents the dynamic binding and a binding logic of an object participating in the dynamic binding.
    /// </summary>
    internal sealed class MetaObject : DynamicMetaObject
    {
        /// <summary>
        /// Should this <see cref="MetaObject"/> bind to <see langword="static"/> or instance methods and fields.
        /// </summary>
        private readonly bool isStatic;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaObject"/> class.
        /// </summary>
        /// <param name="expression">
        /// The expression representing this <see cref="DynamicMetaObject"/> during the dynamic binding process.
        /// </param>
        /// <param name="value">
        /// The runtime value represented by the <see cref="DynamicMetaObject"/>.
        /// </param>
        /// <param name="staticBind">
        /// Should this MetaObject bind to <see langword="static"/> or instance methods and fields.
        /// </param>
        public MetaObject(Expression expression, object value, bool staticBind) :
            base(expression, BindingRestrictions.Empty, value)
        {
            isStatic = staticBind;
        }

        /// <summary>
        /// Performs the binding of the dynamic invoke member operation.
        /// </summary>
        /// <param name="binder">
        /// An instance of the <see cref="InvokeMemberBinder"/> that represents the details of the dynamic operation.
        /// </param>
        /// <param name="args">
        /// An array of <see cref="DynamicMetaObject"/> instances - arguments to the invoke member operation.
        /// </param>
        /// <returns>
        /// The new <see cref="DynamicMetaObject"/> representing the result of the binding.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// There is an attempt to dynamically access a class member that does not exist.
        /// </exception>
        public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        {
            var self = Expression;
            var exposed = (Exposed)Value;

            var argTypes = new Type[args.Length];
            var argExps = new Expression[args.Length];

            for (int i = 0; i < args.Length; ++i)
            {
                argTypes[i] = args[i].LimitType;
                argExps[i] = args[i].Expression;
            }

            var type = exposed.SubjectType;
            var declaringType = type;
            MethodInfo method;
            do
            {
                method = declaringType.GetMethod(binder.Name, GetBindingFlags(), null, argTypes, null);
            }
            while (method == null && (declaringType = declaringType.BaseType) != null);

            if (method == null)
            {
                throw new MissingMemberException(type.FullName, binder.Name);
            }

            var @this = isStatic
                            ? null
                            : Expression.Convert(Expression.Field(Expression.Convert(self, typeof(Exposed)), "value"), type);

            var target = Expression.Call(@this, method, argExps);
            var restrictions = BindingRestrictions.GetTypeRestriction(self, typeof(Exposed));

            return new DynamicMetaObject(ConvertExpressionType(binder.ReturnType, target), restrictions);
        }

        /// <summary>
        /// Performs the binding of the dynamic get member operation.
        /// </summary>
        /// <param name="binder">
        /// An instance of the <see cref="GetMemberBinder"/> that represents the details of the dynamic operation.
        /// </param>
        /// <returns>
        /// The new <see cref="DynamicMetaObject"/> representing the result of the binding.
        /// </returns>
        public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            var self = Expression;

            var memberExpression = GetMemberExpression(self, binder.Name);

            var target = Expression.Convert(memberExpression, binder.ReturnType);
            var restrictions = BindingRestrictions.GetTypeRestriction(self, typeof(Exposed));

            return new DynamicMetaObject(target, restrictions);
        }

        /// <summary>
        /// Performs the binding of the dynamic set member operation.
        /// </summary>
        /// <param name="binder">
        /// An instance of the <see cref="SetMemberBinder"/> that represents the details of the dynamic operation.
        /// </param>
        /// <param name="value">
        /// The <see cref="DynamicMetaObject"/> representing the value for the set member operation.
        /// </param>
        /// <returns>
        /// The new <see cref="DynamicMetaObject"/> representing the result of the binding.
        /// </returns>
        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            var self = Expression;

            var memberExpression = GetMemberExpression(self, binder.Name);

            var target =
                Expression.Convert(
                    Expression.Assign(memberExpression, Expression.Convert(value.Expression, memberExpression.Type)),
                    binder.ReturnType);
            var restrictions = BindingRestrictions.GetTypeRestriction(self, typeof(Exposed));

            return new DynamicMetaObject(target, restrictions);
        }

        /// <summary>
        /// Generates the <see cref="Expression"/> for accessing a member by name.
        /// </summary>
        /// <param name="self">
        /// The <see cref="Expression"/> for accessing the <see cref="Exposed"/> instance.
        /// </param>
        /// <param name="memberName">
        /// The member name.
        /// </param>
        /// <returns>
        /// <see cref="MemberExpression"/> for accessing a member by name.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// </exception>
        private MemberExpression GetMemberExpression(Expression self, string memberName)
        {
            MemberExpression memberExpression = null;
            var type = ((Exposed)Value).SubjectType;
            var @this = isStatic
                            ? null
                            : Expression.Convert(Expression.Field(Expression.Convert(self, typeof(Exposed)), "value"), type);
            var declaringType = type;

            do
            {
                var property = declaringType.GetProperty(memberName, GetBindingFlags());
                if (property != null)
                {
                    memberExpression = Expression.Property(@this, property);
                }
                else
                {
                    var field = declaringType.GetField(memberName, GetBindingFlags());
                    if (field != null)
                    {
                        memberExpression = Expression.Field(@this, field);
                    }
                }
            }
            while (memberExpression == null && (declaringType = declaringType.BaseType) != null);

            if (memberExpression == null)
            {
                throw new MissingMemberException(type.FullName, memberName);
            }

            return memberExpression;
        }

        /// <summary>
        /// Coerces the expression type into the expected return type.
        /// </summary>
        /// <param name="expectedType">Type expeted at the dispatch site.</param>
        /// <param name="target">Expression to coerce.</param>
        /// <remarks>Dynamic dispatch expects a <see langword="void"/> method to return <see langword="null"/>.</remarks>
        /// <returns>Target expression coerced to the required type.</returns>
        private static Expression ConvertExpressionType(Type expectedType, Expression target)
        {
            if (target.Type == expectedType)
            {
                return target;
            }
            if (target.Type == typeof(void))
            {
                return Expression.Block(target, Expression.Default(expectedType));
            }
            if (expectedType == typeof(void))
            {
                return Expression.Block(target, Expression.Empty());
            }
            return Expression.Convert(target, expectedType);
        }

        /// <summary>
        /// Returns <see cref="BindingFlags"/> for member search.
        /// </summary>
        /// <returns>
        /// Static or instance flags depending on <see cref="isStatic"/>.
        /// </returns>
        private BindingFlags GetBindingFlags()
        {
            return isStatic
                       ? BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
                       : BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        }
    }
}
