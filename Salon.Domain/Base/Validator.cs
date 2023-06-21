//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace Salon.Domain.Base
//{
//    public class Validator<TEntity> : IValidator<TEntity> where TEntity : class
//    {
//        private readonly Dictionary<Expression<Func<TEntity, bool>>, string> _validations;

//        public Validator()
//        {
//            _validations = new Dictionary<Expression<Func<TEntity, bool>>, string>();
//        }

//        public async Task<(bool, string)> IsValid(TEntity entity)
//        {
//            var errorMessages = new StringBuilder();
//            var isValid = true;

//            foreach (var validation in _validations)
//            {
//                var entityValid = await Task.Run(() => validation.Key.Compile());

//                if (!entityValid(entity))
//                {
//                    errorMessages.AppendLine(validation.Value);
//                    isValid = false;
//                }
//            }

//            return (isValid, errorMessages.ToString());
//        }

//        public Validator<TEntity> Must(Expression<Func<TEntity, bool>> validation, string errorMessage)
//        {
//            _validations.Add(validation, errorMessage);
//            return this;
//        }
//    }
//}
