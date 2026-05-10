namespace zizo_shop.Application.DTOs.Category
{
    public record CategoryDto(Guid Id, string Name, string Slug, Guid? ParentCategoryId, List<CategoryDto> SubCategories);
}
