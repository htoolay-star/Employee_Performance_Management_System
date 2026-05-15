using EPMS.Domain.Contracts;
using EPMS.Domain.Entities.Performance;
using EPMS.Domain.Interface.IService.Performance;
using EPMS.Shared.DTOs.Common;
using EPMS.Shared.DTOs.PerformanceDTOs.FormTemplateDTOs;
using EPMS.Shared.Enums;
using static EPMS.Shared.Constants.ServiceResponseMessages;

using Mapster;
namespace EPMS.Domain.Services.Performance
{
    public class FormTemplateService : IFormTemplateService
    {
        private readonly IUnitOfWork _uow;
        
        public FormTemplateService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<SuccessResponse<IEnumerable<FormTemplateDto>>> GetAllAsync()
        {
            var templates = await _uow.Perf.FormTemplates.GetAllAsync();
            var dtos = templates.Adapt<IEnumerable<FormTemplateDto>>();
            return SuccessResponse<IEnumerable<FormTemplateDto>>.Ok(dtos, FormTemplateMsg.RetrievedAll);
        }

        public async Task<SuccessResponse<IEnumerable<FormTemplateDto>>> GetActiveAsync()
        {
            var templates = await _uow.Perf.FormTemplates.GetActiveAsync();
            var dtos = templates.Adapt<IEnumerable<FormTemplateDto>>();
            return SuccessResponse<IEnumerable<FormTemplateDto>>.Ok(dtos, FormTemplateMsg.RetrievedActive);
        }

        public async Task<SuccessResponse<FormTemplateDto>> GetByIdAsync(long id)
        {
            var template = await _uow.Perf.FormTemplates.GetByIdAsync(id);

            if (template == null)
                return SuccessResponse<FormTemplateDto>.Fail(FormTemplateMsg.NotFound(id), ErrorType.NotFound);

            var dto = template.Adapt<FormTemplateDto>();
            dto.QuestionCount = template.Questions?.Count ?? 0;
            return SuccessResponse<FormTemplateDto>.Ok(dto, FormTemplateMsg.Retrieved);
        }

        public async Task<SuccessResponse<long>> CreateAsync(CreateFormTemplateDto dto)
        {
            if (await _uow.Perf.FormTemplates.NameExistsAsync(dto.Name))
            {
                return SuccessResponse<long>.Fail(string.Format(FormTemplateMsg.DuplicateName, dto.Name), ErrorType.Conflict);
            }

            var template = new FormTemplate(dto.Name, dto.FormType);

            _uow.Perf.FormTemplates.Add(template);
            await _uow.CompleteAsync();

            return SuccessResponse<long>.Ok(template.Id, FormTemplateMsg.Created);
        }

        public async Task<SuccessResponse> UpdateAsync(long id, UpdateFormTemplateDto dto)
        {
            var template = await _uow.Perf.FormTemplates.GetByIdAsync(id);

            if (template == null)
                return SuccessResponse.Fail(FormTemplateMsg.NotFound(id), ErrorType.NotFound);

            if (await _uow.Perf.FormTemplates.NameExistsAsync(dto.Name, id))
            {
                return SuccessResponse.Fail(string.Format(FormTemplateMsg.DuplicateName, dto.Name), ErrorType.Conflict);
            }

            template.Update(dto.Name, dto.FormType);

            _uow.Perf.FormTemplates.Update(template);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(FormTemplateMsg.Updated);
        }

        public async Task<SuccessResponse> DeleteAsync(long id)
        {
            var template = await _uow.Perf.FormTemplates.GetByIdAsync(id);

            if (template == null)
                return SuccessResponse.Fail(FormTemplateMsg.NotFound(id), ErrorType.NotFound);

            _uow.Perf.FormTemplates.Delete(template);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(FormTemplateMsg.Deleted);
        }

        public async Task<SuccessResponse> DeactivateAsync(long id)
        {
            var template = await _uow.Perf.FormTemplates.GetByIdAsync(id);

            if (template == null)
                return SuccessResponse.Fail(FormTemplateMsg.NotFound(id), ErrorType.NotFound);

            template.Deactivate();

            _uow.Perf.FormTemplates.Update(template);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(FormTemplateMsg.Deactivated);
        }

        public async Task<SuccessResponse> ReactivateAsync(long id)
        {
            var template = await _uow.Perf.FormTemplates.GetByIdAsync(id);

            if (template == null)
                return SuccessResponse.Fail(FormTemplateMsg.NotFound(id), ErrorType.NotFound);

            template.Reactivate();

            _uow.Perf.FormTemplates.Update(template);
            await _uow.CompleteAsync();

            return SuccessResponse.Ok(FormTemplateMsg.Reactivated);
        }
    }
}
