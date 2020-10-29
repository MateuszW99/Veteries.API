using FluentValidation;
using MediatR.Pipeline;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Pipeline
{
    public class ModelValidationPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly IServiceProvider _serviceProvider;

        public ModelValidationPreProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var validator = (AbstractValidator<TRequest>)this._serviceProvider.GetService(typeof(AbstractValidator<TRequest>));

            if (validator != null)
            {
                await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);
            }
        }
    }
}