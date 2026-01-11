using FluentValidation;
using Nexus.Core.DTOs.Projects;

namespace Nexus.Api.Validators.Projects;

public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.")
            .Length(3,100).WithMessage("Name must be between 3 and 100 characters.");
        
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(10).WithMessage("Project Code cannot exceed 10 characters.")
            .Matches("^[a-zA-Z0-9]*$").WithMessage("Project Code must be alphanumeric. example: A1B2C3");
    }
}