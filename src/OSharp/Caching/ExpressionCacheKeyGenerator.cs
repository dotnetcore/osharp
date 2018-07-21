using System.Linq;
using System.Linq.Expressions;

using OSharp.Collections;


namespace OSharp.Caching
{
    /// <summary>
    /// 表达式缓存键生成器
    /// </summary>
    public class ExpressionCacheKeyGenerator : ICacheKeyGenerator
    {
        private readonly Expression _expression;

        /// <summary>
        /// 初始化一个<see cref="ExpressionCacheKeyGenerator"/>类型的新实例
        /// </summary>
        public ExpressionCacheKeyGenerator(Expression expression)
        {
            _expression = expression;
        }

        #region Implementation of ICacheKeyGenerator

        /// <summary>
        /// 生成缓存键
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public string GetKey(params object[] args)
        {
            Expression expression = _expression;
            expression = Evaluator.PartialEval(expression, CanBeEvaluatedLocally);
            expression = LocalCollectionExpressionVisitor.Rewrite(expression);
            string key = expression.ToString();
            return key + args.ExpandAndToString();
        }

        #endregion

        private static bool CanBeEvaluatedLocally(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Parameter)
            {
                return false;
            }
            if (typeof(IQueryable).IsAssignableFrom(expression.Type))
            {
                return false;
            }
            return true;
        }
    }
}
