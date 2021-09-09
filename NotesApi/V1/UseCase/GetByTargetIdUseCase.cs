using System.Collections.Generic;
using Hackney.Core.DynamoDb;
using Hackney.Core.Logging;
using NotesApi.V1.Boundary.Request;
using NotesApi.V1.Boundary.Response;
using NotesApi.V1.Factories;
using NotesApi.V1.Gateways;
using NotesApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;
using NotesApi.V1.Infrastructure;

namespace NotesApi.V1.UseCase
{
    public class GetByTargetIdUseCase : IGetByTargetIdUseCase
    {
        private readonly INotesGateway _gateway;
        private readonly IExcludedCategoriesFactory _excludedCategoriesFactory;

        public GetByTargetIdUseCase(INotesGateway gateway, IExcludedCategoriesFactory excludedCategoriesFactory)
        {
            _gateway = gateway;
            _excludedCategoriesFactory = excludedCategoriesFactory;
        }

        [LogCall]
        public async Task<PagedResult<NoteResponseObject>> ExecuteAsync(GetNotesByTargetIdQuery query, List<string> excludedCategoriesNames)
        {
            var excludedCategories = _excludedCategoriesFactory.Create(excludedCategoriesNames);

            var gatewayResult = await _gateway.GetByTargetIdAsync(query, excludedCategories).ConfigureAwait(false);
            return new PagedResult<NoteResponseObject>(gatewayResult.Results.ToResponse(), gatewayResult.PaginationDetails);
        }
    }
}
