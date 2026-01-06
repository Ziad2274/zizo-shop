namespace zizo_shop.Infrastructure.Identity
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }

}
