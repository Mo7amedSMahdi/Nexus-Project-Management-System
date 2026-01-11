using FluentValidation;
using Nexus.Core.DTOs.Tickets;

namespace Nexus.Api.Validators.Tickets;

public class CreateTicketRequestValidator : AbstractValidator<CreateTicketRequest>
{
    public CreateTicketRequestValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Project ID is required");
        
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required").Length(3,100).WithMessage("Title must be between 3 and 100 characters");
        
        RuleFor(x => x.Priority).IsInEnum().WithMessage("Priority must be one of the following: Low, Medium, High, Default: Medium");
    
        RuleFor(x => x.Status).IsInEnum().WithMessage("Status must be one of the following: Todo, InProgress, InReview, Done, Cancelled, Default: Todo");
    }
}