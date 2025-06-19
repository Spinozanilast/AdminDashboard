using AdminDashboard.Contracts.Tags;

namespace AdminDashboard.Contracts.Clients;

public record ClientDto(Guid Id, string Name, string Email, decimal BalanceT, List<TagDto> Tags);