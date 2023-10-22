using MediatR;

namespace CrudMediatR.Domain.Command
{
    public class CustomerDeleteCommand : IRequest<string>
    {
        public int Id { get; set; }
    }
}
