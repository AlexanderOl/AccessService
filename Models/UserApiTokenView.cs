namespace AccessService.Models
{
    public record UserApiTokenView
    {
        public required string Token { get; set; }
        public required string Status { get; set; }
        public required string LastUsage { get; set; }
        public required List<string> Permissions { get; set; }
    }
}
