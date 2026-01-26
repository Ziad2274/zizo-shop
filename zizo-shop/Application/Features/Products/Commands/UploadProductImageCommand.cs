using MediatR;

namespace zizo_shop.Application.Features.Products.Commands
{
    public class UploadProductImageCommand : IRequest<string>
    {
        public Guid ProductId { get; set; }
        public IFormFile File { get; set; } = default!;
    }
}
