using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace OSharp.Caching
{
	/// <summary>
	/// Enables the partial evaluation of queries.
	/// </summary>
	/// <remarks>
	/// From http://msdn.microsoft.com/en-us/library/bb546158.aspx
	/// Copyright notice http://msdn.microsoft.com/en-gb/cc300389.aspx#O
	/// </remarks>
	internal static class Evaluator
	{
		/// <summary>
		/// Performs evaluation replacement of independent sub-trees
		/// </summary>
		/// <param name="expression">The root of the expression tree.</param>
		/// <param name="fnCanBeEvaluated">A function that decides whether a given expression node can be part of the local function.</param>
		/// <returns>A new tree with sub-trees evaluated and replaced.</returns>
		public static Expression PartialEval(Expression expression, Func<Expression, bool> fnCanBeEvaluated)
		{
			return new SubtreeEvaluator(new Nominator(fnCanBeEvaluated).Nominate(expression)).Eval(expression);
		}

		/// <summary>
		/// Evaluates replaces sub-trees when first candidate is reached (top-down)
		/// </summary>
		class SubtreeEvaluator : ExpressionVisitor
		{
			readonly HashSet<Expression> _candidates;

			internal SubtreeEvaluator(HashSet<Expression> candidates)
			{
				_candidates = candidates;
			}

			internal Expression Eval(Expression exp)
			{
				return Visit(exp);
			}

            // Solves the projection problem, https://github.com/loresoft/EntityFramework.Extended/issues/19 
            // and https://github.com/osjoberg/LinqCache/issues/3, thank you @agnauck and @geriadejes.
            protected override Expression VisitMemberInit(MemberInitExpression node)
            {
                if (node.NewExpression.NodeType == ExpressionType.New)
                {
                    return node;
                }

                return base.VisitMemberInit(node);
            }

			public override Expression Visit(Expression exp)
			{
				if (exp == null)
				{
					return null;
				}
				if (_candidates.Contains(exp))
				{
					return Evaluate(exp);
				}
				return base.Visit(exp);
			}

			private static Expression Evaluate(Expression e)
			{
				if (e.NodeType == ExpressionType.Constant)
				{
					return e;
				}
				var lambda = Expression.Lambda(e);
				var fn = lambda.Compile();
				return Expression.Constant(fn.DynamicInvoke(null), e.Type);
			}

		}

		/// <summary>
		/// Performs bottom-up analysis to determine which nodes can possibly
		/// be part of an evaluated sub-tree.
		/// </summary>
		class Nominator : ExpressionVisitor
		{
			readonly Func<Expression, bool> _fnCanBeEvaluated;
			HashSet<Expression> _candidates;
			bool _cannotBeEvaluated;

			internal Nominator(Func<Expression, bool> fnCanBeEvaluated)
			{
				_fnCanBeEvaluated = fnCanBeEvaluated;
			}

			internal HashSet<Expression> Nominate(Expression expression)
			{
				_candidates = new HashSet<Expression>();
				Visit(expression);
				return _candidates;
			}

			public override Expression Visit(Expression expression)
			{
				if (expression != null)
				{
					var saveCannotBeEvaluated = _cannotBeEvaluated;
					_cannotBeEvaluated = false;
					base.Visit(expression);
					if (!_cannotBeEvaluated)
					{
						if (_fnCanBeEvaluated(expression))
						{
							_candidates.Add(expression);
						}
						else
						{
							_cannotBeEvaluated = true;
						}
					}
					_cannotBeEvaluated |= saveCannotBeEvaluated;
				}
				return expression;
			}
		}
	}
}



